using HelpersModel.ObjectUtils.Test;
using Travel_Application.Controllers;
using Travel_Application.Domain.Model.Models;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using Moq;

namespace ControllersTests
{
    /// <summary>
    /// HomeControllerTest
    /// </summary>
    public class HomeControllerTest
    {

        private readonly HomeController homeController;
        private readonly IConfiguration configuration;

        public HomeControllerTest()
        {
            ClsLibro clsLibro = ClsLibroHelperModel.GetClsLibro;
            homeController = new HomeController(configuration);
        }
        /// <summary>
        /// ShouldLoginTest
        /// </summary>
        [Fact]
        public void ShouldLoginTest()
        {
       
            ClsUsuario clsUsuario = ClsUsuarioHelperModel.GetClsUsuario;
            // Creamos el mock sobre nuestra interfaz
            var mock = new Mock<HomeController>();

            // Definimos un comportamiento específico con parameter-matching
            //mock.Verify(x => x.Index(), Times.Never());
            // Obtenemos una instancia del objeto mockeado
            //homeController.Index();
            // Comprobamos el comportamiento genérico
            Assert.Equal(1, clsUsuario.id);
        }

        /// <summary>
        /// ShouldProductTest
        /// </summary>
        [Fact]
        public void ShouldProductTest()
        {


            ClsLibro clsLibro = ClsLibroHelperModel.GetClsLibro;
            var r = homeController.Product();
            // Comprobamos el comportamiento genérico
            Assert.Equal(1, clsLibro.id);
            Assert.NotNull(r);
        }
    }
}
