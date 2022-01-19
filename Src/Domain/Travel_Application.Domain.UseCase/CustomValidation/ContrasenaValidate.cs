using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace Travel_Application.Domain.UseCase.ContrasenaValidate
{
    public class ContrasenaValidate : Attribute, IModelValidator
    {
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Función para la retornar validación - función con inyección de dependencia
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
                return new List<ModelValidationResult>
                {
                    new ModelValidationResult("", ErrorMessage)
                };
        }
    }
}