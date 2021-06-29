﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PeliculasAPI.Utilidades
{
    public class AlmacenadorAzureStorage : IAlmacenadorArchivos
    {
        private string connectionString;
        public AlmacenadorAzureStorage(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo)
        {
            var cliente = new BlobContainerClient(this.connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extesion = Path.GetExtension(archivo.FileName);
            var archivoNombre = $"{Guid.NewGuid()} {extesion}";
            var blob = cliente.GetBlobClient(archivoNombre);
            await blob.UploadAsync(archivo.OpenReadStream());
            return blob.Uri.ToString();
        }

        public async Task BorrarArchivo(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var cliente = new BlobContainerClient(this.connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            var archivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta)
        {
            await this.BorrarArchivo(ruta, contenedor);
            return await this.GuardarArchivo(contenedor, archivo);
        }



    }
}
