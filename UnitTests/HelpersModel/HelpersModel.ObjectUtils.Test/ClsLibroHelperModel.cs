using Travel_Application.Domain.Model.Models;
using System;

namespace HelpersModel.ObjectUtils.Test
{
    /// <summary>
    /// ClsLibroHelperModel
    /// </summary>
    public class ClsLibroHelperModel
    {
        public static ClsLibro GetClsLibro => new ClsLibro()
        {
            id = 1,
            nombre = "GABRIEL",
            apellidos = "GARCIA",
            titulo= "100 AÑOS DE SOLEDAD",
            sinopsis ="PRUEBA",
            n_paginas = 200,
            nombre_editorial ="PRUEBA",
            sede ="COLOMBIA",

        };
    }
}
