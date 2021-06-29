using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PeliculasAPI.DTOs
{
    public class GeneroDTO
    {
        public int Id
        {
            get; set;
        }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]         
        public string Nombre
        {
            get;
            set;
        }

    }
}
