using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.ResponseCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeliculasAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeliculasAPI.Repositorio;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PeliculasAPI.DTOs;
using System.Security.AccessControl;
using PeliculasAPI.Utilidades;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;

namespace PeliculasAPI.Controllers 
{
    [Microsoft.AspNetCore.Mvc.Route("api/generos")]
    public class GeneroController : ControllerBase
    {
        public ILogger<GeneroController> Logger { get; }
        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }

        public GeneroController (ILogger<GeneroController> logger, ApplicationDbContext context, IMapper mapper)
        {
            Logger = logger;
            Context = context;
            Mapper = mapper;
        }

        
       
        [HttpGet]
        public async Task < ActionResult < List<GeneroDTO>>> Get   ([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = this.Context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync(); 

            var resultado = this.Mapper.Map<List<GeneroDTO>>(generos);        


            return resultado; 
        }

      

        [HttpGet ("{Id:int}")]
        public async Task <ActionResult <GeneroDTO>>  Get(int Id) 
        {
            var genero = await this.Context.Generos.FirstOrDefaultAsync(x => x.Id == Id); 
            
            if (genero == null)
            {
                return NotFound(); 
            }

            return Mapper.Map<GeneroDTO>(genero); 
             
        }

        [HttpPost]
        public async  Task <ActionResult>  Post([FromBody]GeneroCreacionDTO  generoCreacionDTO)
        {
            var genero = this.Mapper.Map<Genero>(generoCreacionDTO); 
            Context.Add(genero);
            await this.Context.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpPut ("{Id:int}")]
        public async Task <ActionResult> Put (int Id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await this.Context.Generos.FirstOrDefaultAsync(x => x.Id == Id);

            if (genero == null)
            {
                return NotFound();
            }

            genero = this.Mapper.Map(generoCreacionDTO, genero);
            await this.Context.SaveChangesAsync();
            return NoContent(); 
            
        }


        [HttpDelete("{id:int}")]    
        public async Task<ActionResult> Delete(int Id)
        {

            var existe = await   this.Context.Generos.AnyAsync (  x=> x.Id == Id);
            if (! existe)
            {
                return NotFound(); 
            }

            Context.Remove(new Genero { Id = Id });
            await this.Context.SaveChangesAsync();
            return NoContent();

        }
    }
}
