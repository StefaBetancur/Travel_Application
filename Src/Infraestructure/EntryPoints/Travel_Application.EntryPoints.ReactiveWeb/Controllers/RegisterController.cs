using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Impexium_Application.Domain.Model.Models;

namespace Impexium_Application.EntryPoints.ReactiveWeb.Controllers
{
    public class RegisterController : Controller
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
        public RegisterController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public ActionResult Login(string returnUrl)
        /// <summary>
        /// Action para inicializar la carga de la vista del Login en base a los atributos de modelo usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {   
            return View(new ClsUsuario());
        }

        /// <summary>
        /// Action de tipo POST para  para inicializar el proceso de validación e iniciar sessión en  base a los datos del modelo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(ClsUsuario model)
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
                        SqlCommand com = new SqlCommand("PROCEDURE_IMPEXIUM", connection);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@usuario", model.usuario);
                        com.Parameters.AddWithValue("@contrasena", model.contrasena);
                        com.Parameters.AddWithValue("@nombre", string.Empty);
                        com.Parameters.AddWithValue("@descripcion", string.Empty);
                        com.Parameters.AddWithValue("@cantidad", 0);
                    com.Parameters.AddWithValue("@operacion", "I");
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
                            HttpContext.Session.SetString(SessionUser, model.usuario);
                            return RedirectToAction("Index", "Home");
                            
                        }
                        else
                        {
                            ModelState.AddModelError("", "Registro no exitoso intenten de nuevo");
                           
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