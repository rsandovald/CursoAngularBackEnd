using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Filtros
{
    public class FiltroDeExcepcion: ExceptionFilterAttribute
    {
        public ILogger<FiltroDeExcepcion> Logger { get; }

        public FiltroDeExcepcion(ILogger <FiltroDeExcepcion> logger )
        {
            Logger = logger;             
        }

        public override void OnException(ExceptionContext context)
        {
            this.Logger.LogError(context.Exception, context.Exception.Message); 
            base.OnException(context); 
        }



    }
}
