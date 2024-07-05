using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient=blobServiceClient;
        }
        public async Task<List<string>> GetAllBobs(string containerName)
        {
          var blobNames=new List<string>();
           BlobContainerClient blobContainerClient=_blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();
            await foreach (var item in blobs)
            {
                blobNames.Add(item.Name);
            }
            return blobNames;

        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(name);
            BlobHttpHeaders httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType,
            };

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);

            return result != null;
        }

    }

}
