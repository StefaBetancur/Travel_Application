using System;
using System.Collections.Generic;//Lista de colecciones genericas
using System.Data;//Identificar el tipo de objeto a manipular en base de datos
using Microsoft.Data.SqlClient;//Controlador de acceso a datos
using System.Linq;//Para hacer macth entre lista del dataReader y los antributos del modelo (usuario y contrasena)
//using System.Security.Claims;//Extrae las peticiones asociadas al usuario autenticado
using Microsoft.Extensions.Configuration;//Para acceder al archivo de configuración appsettings.json
using Microsoft.AspNetCore.Http;//Para el manejo de solicitudes y respuestas HTTP
using Microsoft.AspNetCore.Mvc;
using Impexium_Application.Domain.Model.Models;

namespace Impexium_Application.EntryPoints.ReactiveWeb.Controllers
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
            return View(new ClsProducto());
        }

        /// <summary>
        /// Action de tipo POST para  para inicializar el proceso de validación e iniciar sessión en  base a los datos del modelo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Product(ClsProducto model)
        {
            //Conexión a la base de datos
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var list_products = new List<ClsProducto>();
                if (model.nombre == null || model.nombre.Equals("") ||
                    model.descripcion == null || model.descripcion.Equals("") ||
                    model.cantidad.ToString() == null || model.cantidad.Equals(""))
                {
                    ModelState.AddModelError("", "Ingresar los datos solictiados");
                }
                else
                {
                    connection.Open();//Abrir la conexión a la base de datos
                    SqlCommand com = new SqlCommand("PROCEDURE_IMPEXIUM", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@usuario", string.Empty);
                    com.Parameters.AddWithValue("@contrasena", string.Empty);
                    com.Parameters.AddWithValue("@nombre", model.nombre);
                    com.Parameters.AddWithValue("@descripcion", model.descripcion);
                    com.Parameters.AddWithValue("@cantidad", model.cantidad);
                    com.Parameters.AddWithValue("@operacion", "P");
                    SqlDataReader dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        ClsProducto clsProducto = new ClsProducto();
                        clsProducto.nombre = Convert.ToString(dr["Nombre"]);
                        clsProducto.descripcion = Convert.ToString(dr["Descripcion"]);
                        clsProducto.cantidad = Convert.ToInt32(dr["Cantidad"]);


                        list_products.Add(clsProducto);
                    }


                    if (list_products.Count > 0)
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
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.Session.Clear();//Limpiar la sesión
            return RedirectToAction("Login", "Login");//Redireccionar a la vista login
        }

    }
}