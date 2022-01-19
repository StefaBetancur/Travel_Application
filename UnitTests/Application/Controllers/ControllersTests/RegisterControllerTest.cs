using Travel_Application.Controllers;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using Travel_Application.Domain.Model.Models;
using HelpersModel.ObjectUtils.Test;
using Moq;

namespace ControllersTests
{
    /// <summary>
    /// RegisterControllerTest
    /// </summary>
    public class RegisterControllerTest
    {

        private readonly RegisterController registerController;
        private readonly IConfiguration configuration;

        public RegisterControllerTest()
        {
            ClsUsuario clsUsuario = ClsUsuarioHelperModel.GetClsUsuario;
            registerController = new RegisterController(configuration);
        }
        /// <summary>
        /// ShouldLoginTest
        /// </summary>
        [Fact]
        public void ShouldRegisterTest()
        {
                       
            ClsUsuario clsUsuario = ClsUsuarioHelperModel.GetClsUsuario;
            var r = registerController.Register();
            // Comprobamos el comportamiento genérico
            Assert.Equal(1, clsUsuario.id);
            Assert.NotNull(r);
        }

        /// <summary>
        /// ShouldLogOffTest
        /// </summary>
        [Fact]
        public void ShouldLogOffTest()
        {
         
            ClsUsuario clsUsuario = ClsUsuarioHelperModel.GetClsUsuario;
            // Creamos el mock sobre nuestra interfaz
            var mock = new Mock<RegisterController>();

            // Definimos un comportamiento específico con parameter-matching
            //mock.Verify(x => x.LogOff(), Times.Once());
            // Comprobamos el comportamiento genérico
            Assert.Equal(1,clsUsuario.id);
        }
    }
}
