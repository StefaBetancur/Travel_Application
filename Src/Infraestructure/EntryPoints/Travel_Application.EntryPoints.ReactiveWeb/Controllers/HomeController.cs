using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Impexium_Application.Domain.Model.Models;

namespace Impexium_Application.EntryPoints.ReactiveWeb.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }

        public HomeController(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IActionResult Producto() {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<ClsProducto> ProductoList = new List<ClsProducto>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString)) {

                connection.Open();

                string sql = "select * from Productos";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ClsProducto clsProducto = new ClsProducto();
                        clsProducto.id = Convert.ToInt32(dataReader["Id"]);
                        clsProducto.nombre = Convert.ToString(dataReader["Nombre"]);
                        clsProducto.descripcion = Convert.ToString(dataReader["Descripcion"]);
                        clsProducto.cantidad = Convert.ToInt32(dataReader["Cantidad"]);
                  
                        ProductoList.Add(clsProducto);
                    }
                }

                connection.Close();
            }
            return View(ProductoList);
        }

        /// <summary>
        /// Action para limpiar y cerrar la sesión de la aplicación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Product()
        {
            return RedirectToAction("Product", "Product");//Redireccionar a la vista login
        }
    }
}