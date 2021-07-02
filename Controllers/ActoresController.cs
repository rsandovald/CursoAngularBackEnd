using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Repositorio;
using PeliculasAPI.Utilidades;

namespace PeliculasAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/actores")]
    public class ActoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores"; 

        public ActoresController (ApplicationDbContext context, 
            IMapper mapper
            , IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpPost]
        public async Task <ActionResult> Post ([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
          
            var  actor = this.mapper.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.foto != null)
            {
                actor.Foto = await this.almacenadorArchivos.GuardarArchivo( this.contenedor, actorCreacionDTO.foto); 
            }
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent(); 
        }

        /*

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var actores = await this.context.Actores.ToListAsync();
            var resultado = this.mapper.Map<List<ActorDTO>>(actores);
            return resultado;
        }

        */

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = this.context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var actores = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            var resultado = this.mapper.Map<List<ActorDTO>>(actores);
            return resultado;
        }



        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int Id)
        {
            var actor = await this.context.Actores.FirstOrDefaultAsync(x => x.Id == Id);

            if (actor == null)
            {
                return NotFound();
            }

            return this.mapper.Map<ActorDTO>(actor);

        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int Id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = await this.context.Actores.FirstOrDefaultAsync(x => x.Id == Id);

            if (actor == null)
            {
                return NotFound();
            }

            actor = this.mapper.Map(actorCreacionDTO, actor);

            if (actorCreacionDTO.foto != null)
            {
                actor.Foto = await this.almacenadorArchivos.EditarArchivo(this.contenedor, actorCreacionDTO.foto, actor.Foto);
            }

            await this.context.SaveChangesAsync();  
            return NoContent();

        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var actor = await this.context.Actores.FirstOrDefaultAsync(x => x.Id == Id);

            if ( actor == null)
            {
                return NotFound();
            }

            this.context.Remove(actor);
            await this.context.SaveChangesAsync();

            await this.almacenadorArchivos.BorrarArchivo(actor.Foto, this.contenedor); 
            return NoContent();

        }

        [HttpPost("buscarPorNombre")]
        public async Task<ActionResult<List<PeliculaActorDTO>>> BuscarPorNombre([FromBody] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) { return new List<PeliculaActorDTO>(); }
            return await context.Actores
                .Where(x => x.Nombre.Contains(nombre))
                .Select(x => new PeliculaActorDTO { Id = x.Id, Nombre = x.Nombre, Foto = x.Foto })
                .Take(5)
                .ToListAsync();
        }

    }
}
