using Travel_Application.Domain.Model.Models;
using System;

namespace HelpersModel.ObjectUtils.Test
{
    /// <summary>
    /// ClsUsuarioHelperModel
    /// </summary>
    public class ClsUsuarioHelperModel
    {
        public static ClsUsuario GetClsUsuario => new ClsUsuario()
        {
            id = 1,
            usuario = "pedro",
            contrasena = "123",
            intentos = 0,
            nivelSeg = 0,
            fechaReg = DateTime.Now

        };
    }
}
