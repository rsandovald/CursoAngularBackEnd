using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PeliculasAPI.Entidades
{
    public class Genero: IValidatableObject 
    {
        public int Id
        {
            get; set; 
        }

        [Required  (ErrorMessage ="El campo {0} es requerido")]
        [StringLength (maximumLength:50)]
        public string Nombre
        {
            get;
            set; 
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty (this.Nombre))
            {
                var primeraLetra = this.Nombre[0].ToString(); 
                
                if ( primeraLetra != primeraLetra.ToUpper ())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula", new string[] { nameof (Nombre) }); 
                }    

            }
        }
    }
}
