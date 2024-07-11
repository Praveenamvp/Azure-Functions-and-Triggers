using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobClient;

        public ContainerService(BlobServiceClient blobServiceClient)
        {
            _blobClient=blobServiceClient;
        }

        public async Task<List<string>> GetAllContainers()
        {
            var containerNames = new List<string>();
            await foreach (BlobContainerItem container in _blobClient.GetBlobContainersAsync())
            {
                containerNames.Add(container.Name);
            }
            return containerNames;
        }

        public async Task<Dictionary<string, List<string>>> GetAllContainersAndBlobs()
        {
            var containersAndBlobs = new Dictionary<string, List<string>>();

            await foreach (BlobContainerItem container in _blobClient.GetBlobContainersAsync())
            {
                if(container.Name =="containerone"|| container.Name == "containertwo")
                {
                    var blobNames = new List<string>();

                    BlobContainerClient containerClient = _blobClient.GetBlobContainerClient(container.Name);
                    await foreach (BlobItem blob in containerClient.GetBlobsAsync())
                    {
                        blobNames.Add(blob.Name);
                    }
                    containersAndBlobs[container.Name] = blobNames;
                }
                
            }

            return containersAndBlobs;
        }

       
    }
}
