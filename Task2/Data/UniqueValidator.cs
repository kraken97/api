
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WebApplication16.Models;
using System.Linq;
using System.IO;


namespace WebApplication16
{



    public class UniqueAttribute : ValidationAttribute
    {
       
        
      

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {
            Page page = (Page)validationContext.ObjectInstance;
            
            if (!page.UrlName.Equals(page.UrlName))
            {
                return new ValidationResult("urlname must be unique");
            }

            return ValidationResult.Success;
        }
    }
}