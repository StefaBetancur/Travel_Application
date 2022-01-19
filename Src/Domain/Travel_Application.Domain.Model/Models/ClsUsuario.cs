using Travel_Application.Domain.UseCase.ContrasenaValidate;
using System;
using System.ComponentModel.DataAnnotations;

namespace Travel_Application.Domain.Model.Models
{
    public class ClsUsuario
    {
        public int id { get; set; }
   
        [Required]
        public string usuario { get; set; }

        [Required]
        [ContrasenaValidate(ErrorMessage = "Contraseña no valida")]
        public string contrasena { get; set; }

        public int intentos { get; set; }

        public decimal nivelSeg { get; set; }

        public DateTime? fechaReg { get; set; }
    }
}