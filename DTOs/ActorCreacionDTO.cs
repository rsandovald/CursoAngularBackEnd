using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace PeliculasAPI.DTOs
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string nombre { get; set; }
        public string biografia { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public IFormFile foto { get; set;  }   
    }
}
