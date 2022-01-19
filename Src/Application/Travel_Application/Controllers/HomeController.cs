using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Travel_Application.Domain.Model.Models;

namespace Travel_Application.Controllers
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
            List<ClsLibro> LibroList = new List<ClsLibro>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString)) {

                connection.Open();

                string sql = "select l.ISBN,a.nombre, a.apellidos,l.titulo,l.sinopsis,l.n_paginas, e.nombre as nombre_editorial, e.sede " +
                            " from autores a inner join autores_has_libros al on al.autores_id = a.id "+
                            " inner join libros l on l.ISBN = al.libros_ISBN "+
                            " inner join editoriales e on e.id = l.editoriales_id";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ClsLibro clsLibro = new ClsLibro();
                        clsLibro.id = Convert.ToInt32(dataReader["ISBN"]);
                        clsLibro.nombre = Convert.ToString(dataReader["nombre"]);
                        clsLibro.apellidos = Convert.ToString(dataReader["apellidos"]);
                        clsLibro.titulo = Convert.ToString(dataReader["titulo"]);
                        clsLibro.sinopsis = Convert.ToString(dataReader["sinopsis"]);
                        clsLibro.n_paginas = Convert.ToInt32(dataReader["n_paginas"]);
                        clsLibro.nombre_editorial = Convert.ToString(dataReader["nombre_editorial"]);
                        clsLibro.sede = Convert.ToString(dataReader["sede"]);

                        LibroList.Add(clsLibro);
                    }
                }

                connection.Close();
            }
            return View(LibroList);
        }

        /// <summary>
        /// Action para redireccionar a la pantalla de registro y modificacion de productos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Product()
        {
            return RedirectToAction("Product", "Product");
        }
    }
}