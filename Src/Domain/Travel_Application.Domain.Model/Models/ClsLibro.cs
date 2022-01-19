using System;
using System.ComponentModel.DataAnnotations;

namespace Travel_Application.Domain.Model.Models
{
    public class ClsLibro
    {
        public int id { get; set; }
   
        [Required]
        public string nombre { get; set; }

        [Required]
        public string apellidos { get; set; }


        [Required]
        public string titulo { get; set; }

        [Required]
        public string sinopsis { get; set; }

        [Required]
        public int n_paginas { get; set; }

        [Required]
        public string nombre_editorial { get; set; }

        [Required]
        public string sede { get; set; }

    }
}