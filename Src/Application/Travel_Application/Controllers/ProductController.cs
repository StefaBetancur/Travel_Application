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
    public class ProductController : Controller
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
        public ProductController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public ActionResult Login(string returnUrl)
        /// <summary>
        /// Action para inicializar la carga de la vista del Login en base a los atributos de modelo usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult Product()
        {   
            //ViewBag.ReturnUrl = returnUrl;
            return View(new ClsLibro());
        }

        /// <summary>
        /// Action de tipo POST para  para inicializar el proceso de validación e iniciar sessión en  base a los datos del modelo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Product(ClsLibro model)
        {
            //Conexión a la base de datos
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var list_libros = new List<ClsLibro>();
                if (model.id == 0 || model.id.Equals("") ||
                    model.nombre == null || model.nombre.Equals("") ||
                    model.apellidos == null || model.apellidos.Equals("") ||
                    model.titulo == null || model.titulo.Equals("") ||
                    model.sinopsis == null || model.sinopsis.Equals("") ||
                    model.n_paginas == 0 || model.n_paginas.Equals("")||
                    model.nombre_editorial == null || model.nombre_editorial.Equals("") ||
                    model.sede == null || model.sede.Equals(""))
                {
                    ModelState.AddModelError("", "Ingresar los datos solicitiados");
                }
                else
                {
                    connection.Open();//Abrir la conexión a la base de datos
                    SqlCommand com = new SqlCommand("PROCEDURE_TRAVEL", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@usuario", string.Empty);
                    com.Parameters.AddWithValue("@contrasena", string.Empty);
                    com.Parameters.AddWithValue("@ISBN", model.id);
                    com.Parameters.AddWithValue("@nombre", model.nombre);
                    com.Parameters.AddWithValue("@apellidos", model.apellidos);
                    com.Parameters.AddWithValue("@nombre_editorial", model.nombre_editorial);
                    com.Parameters.AddWithValue("@sede", model.sede);
                    com.Parameters.AddWithValue("@titulo", model.titulo);
                    com.Parameters.AddWithValue("@sinopsis", model.sinopsis);
                    com.Parameters.AddWithValue("@n_paginas", model.n_paginas);
                    com.Parameters.AddWithValue("@operacion", "P");
                    SqlDataReader dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        ClsLibro clsLibro = new ClsLibro();
                        clsLibro.id = Convert.ToInt32(dr["ISBN"]);
                        clsLibro.nombre = Convert.ToString(dr["nombre"]);
                        clsLibro.apellidos = Convert.ToString(dr["apellidos"]);
                        clsLibro.nombre_editorial = Convert.ToString(dr["nombre_editorial"]);
                        clsLibro.sede = Convert.ToString(dr["sede"]);
                        clsLibro.titulo = Convert.ToString(dr["titulo"]);
                        clsLibro.sinopsis = Convert.ToString(dr["sinopsis"]);
                        clsLibro.n_paginas = Convert.ToInt32(dr["n_paginas"]);


                        list_libros.Add(clsLibro);
                    }


                    if (list_libros.Count > 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al ingresar el producto");
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