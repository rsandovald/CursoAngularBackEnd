using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int cantidadMaximaRecordsPorPagina = 50; 

        public int RecordPorPagina
        {
            get
            {
                return this.recordsPorPagina; 
            }
            set
            {
                recordsPorPagina = (value > this.cantidadMaximaRecordsPorPagina ? this.cantidadMaximaRecordsPorPagina : value); 
            }
        
        }

    }
}
