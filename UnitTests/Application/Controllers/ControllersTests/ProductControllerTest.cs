using Travel_Application.Controllers;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using Travel_Application.Domain.Model.Models;
using HelpersModel.ObjectUtils.Test;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace ControllersTests
{
    /// <summary>
    /// ProductControllerTest
    /// </summary>
    public class ProductControllerTest
    {

        private readonly ProductController productController;
        private readonly IConfiguration configuration;
        Mock<ProductController> mockproductController = new Mock<ProductController>();
       

        public ProductControllerTest()
        {
            productController = new ProductController(configuration);
        }
        /// <summary>
        /// ShouldProductTest
        /// </summary>
        [Fact]
        public void ShouldProductTest()
        {
            
            ClsLibro clsLibro = ClsLibroHelperModel.GetClsLibro;
            // Creamos el mock sobre nuestra interfaz
            var mock = new Mock<ProductController>();

            // Definimos un comportamiento específico con parameter-matching
            //mock.Verify(x => x.Product(clsLibro), Times.Never());
           // mock.Setup(ps => ps.Product(It.IsAny<ClsLibro>()));
           // mock.Verify(ps => ps.Product(It.IsAny<ClsLibro>()), Times.Never());
            // Obtenemos una instancia del objeto mockeado
            var r = productController.Product();
            // Comprobamos el comportamiento genérico
            Assert.Equal(1, clsLibro.id);
            Assert.NotNull(r);

        }

        /// <summary>
        /// ShouldLogOffTest
        /// </summary>
        [Fact]
        public void ShouldLogOffTest()
        {
             ClsLibro clsLibro = ClsLibroHelperModel.GetClsLibro;
            // Creamos el mock sobre nuestra interfaz
            var mock = new Mock<ProductController>();

            // Definimos un comportamiento específico con parameter-matching
            //mockproductController.Verify(x => x.LogOff(), Times.Once());
           // Comprobamos el comportamiento genérico
            Assert.Equal(1, clsLibro.id);
        }
    }
}
