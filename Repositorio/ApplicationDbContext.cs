﻿using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace PeliculasAPI.Repositorio
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PeliculasActores>()
                .HasKey(x => new { x.ActorId, x.PeliculaId });

            modelBuilder.Entity<PeliculasGeneros>()
                .HasKey(x => new { x.PeliculaId, x.GeneroId });

            modelBuilder.Entity<PeliculasCines>()
                .HasKey(x => new { x.PeliculaId, x.CineId });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Genero> Generos
        {
            get; set;
        }

        public DbSet<Actor> Actores
        {
            get; set;
        }

        public DbSet<Cine> Cines
        {
            get; set;
        }

        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
        public DbSet<PeliculasCines> PeliculasCines { get; set; }
    }
}
