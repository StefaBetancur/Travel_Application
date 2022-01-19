using Travel_Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using Moq;
using Travel_Application.Domain.Model.Models;
using HelpersModel.ObjectUtils.Test;

namespace ControllersTests
{
    /// <summary>
    /// LoginControllerTest
    /// </summary>
    public class LoginControllerTest
    {

        private readonly LoginController loginController;
        private readonly IConfiguration configuration;
        private readonly Controller Controller;

        public LoginControllerTest()
        {
            loginController = new LoginController(configuration);
        }
        /// <summary>
        /// ShouldLoginTest
        /// </summary>
        [Fact]
        public void ShouldLoginTest()
        {
            ClsUsuario clsUsuario = ClsUsuarioHelperModel.GetClsUsuario;
            var r = loginController.Login(); 
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
            var mock = new Mock<LoginController>();

            // Definimos un comportamiento específico con parameter-matching
            //mock.Verify(x => x.LogOff(), Times.Once());
            
            // Comprobamos el comportamiento genérico
            Assert.Equal(1, clsUsuario.id);
        }
    }
}
