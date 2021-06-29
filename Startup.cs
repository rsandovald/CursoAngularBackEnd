using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using PeliculasAPI.Filtros;
using Microsoft.Extensions.Options;
using PeliculasAPI.Repositorio;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Utilidades;
using AutoMapper;
using NetTopologySuite.Geometries;
using NetTopologySuite;

namespace Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton(provider =>
               new MapperConfiguration(config =>
               {
                   var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                   config.AddProfile(new AutoMapperProfiles(geometryFactory));
               }).CreateMapper());

            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

            services.AddTransient<IAlmacenadorArchivos, AlmacenadorAzureStorage>();
            services.AddHttpContextAccessor();                
            

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"),
                sqlServer => sqlServer.UseNetTopologySuite()).EnableSensitiveDataLogging());


            services.AddCors(Options =>
           {
               string frontEndURL = Configuration.GetValue<string>("frontEndURL");
               Options.AddDefaultPolicy(builder =>
              {
                  builder.WithOrigins(frontEndURL).AllowAnyMethod().AllowAnyHeader()
                  .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
              }); 
           }); 
            

            services.AddControllers( options =>
            {
                options.Filters.Add(typeof(FiltroDeExcepcion)); 
            }
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(); 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
