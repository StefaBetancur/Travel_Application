using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Application.Domain.Model.Models;

namespace Travel_Application.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// Constante para Inicializar la Sesión _User
        /// </summary>
        const string SessionUser = "_User";
        public IConfiguration Configuration { get; }
        /// <summary>
        /// Interfaz para acceder a los valores del archivo de configuración
        /// </summary>
        /// <param name="configuration"></param>
        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public ActionResult Login(string returnUrl)
        /// <summary>
        /// Action para inicializar la carga de la vista del Login en base a los atributos de modelo usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {   
            //ViewBag.ReturnUrl = returnUrl;
            return View(new ClsUsuario());
        }

        /// <summary>
        /// Action de tipo POST para  para inicializar el proceso de validación e iniciar sessión en  base a los datos del modelo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(ClsUsuario model)
        {   
            //Conexión a la base de datos
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var list_users = new List<ClsUsuario>();
                if (model.usuario == null || model.usuario.Equals("") ||
                    model.contrasena == null || model.contrasena.Equals(""))
                {
                    ModelState.AddModelError("", "Ingresar los datos solictiados");
                }
                else
                {
                    connection.Open();//Abrir la conexión a la base de datos
                    SqlCommand com = new SqlCommand("PROCEDURE_TRAVEL", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@usuario", model.usuario);
                    com.Parameters.AddWithValue("@contrasena", model.contrasena);
                    com.Parameters.AddWithValue("@ISBN", 0);
                    com.Parameters.AddWithValue("@nombre", string.Empty);
                    com.Parameters.AddWithValue("@apellidos", string.Empty);
                    com.Parameters.AddWithValue("@nombre_editorial", string.Empty);
                    com.Parameters.AddWithValue("@sede", string.Empty);
                    com.Parameters.AddWithValue("@titulo", string.Empty);
                    com.Parameters.AddWithValue("@sinopsis", string.Empty);
                    com.Parameters.AddWithValue("@n_paginas", 0);
                    com.Parameters.AddWithValue("@operacion", "C");
                    SqlDataReader dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        ClsUsuario clsUsuario = new ClsUsuario();
                        clsUsuario.usuario = Convert.ToString(dr["Usuario"]);
                        clsUsuario.contrasena = Convert.ToString(dr["Contrasena"]);


                        list_users.Add(clsUsuario);
                    }


                    if (list_users.Count > 0)
                    {

                        if (list_users.Any(p => p.usuario == model.usuario && p.contrasena == model.contrasena))
                        {
                            
                            HttpContext.Session.SetString(SessionUser, model.usuario);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Datos ingresado no válido.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "El usuario no existe, por favor registrese.");
                        HttpContext.Session.Clear();
                        return RedirectToAction("Register", "Register");
                    }
                }
                return View(model);
            }
        }

        /// <summary>
        /// Action para limpiar y cerrar la sesión de la aplicación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }

    }
}