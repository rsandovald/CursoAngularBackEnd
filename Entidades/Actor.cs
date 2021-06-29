using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Entidades
{
    public class Actor : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [StringLength (maximumLength:200)]
        public string Nombre { get; set; }
        public string Biografia { get; set; }
        
        public DateTime FechaNacimiento { get; set; }
        public string Foto{ get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
