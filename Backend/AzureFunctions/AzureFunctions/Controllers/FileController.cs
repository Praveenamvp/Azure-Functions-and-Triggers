using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BusinessLayer.Implementation;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace AzureFunctions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactCORS")]

    public class FileController : ControllerBase
    {
        private readonly IContainerService _containerService;
        private readonly IBlobService _blobService;
        private readonly BlobServiceClient _blobServiceClient;

        public FileController(IContainerService containerService,IBlobService blobService,BlobServiceClient blobServiceClient )
        {
            _containerService=containerService;
            _blobService=blobService;
            _blobServiceClient = blobServiceClient;

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<string>>> GetAllFiles(string containerName )
        {
            var files = _blobService.GetAllBobs(containerName);
            if (files != null)
            {
                return Ok(files);
            }
            return BadRequest(" Unable to fetch files Details");
        }
        [HttpGet("AllContainerandBlobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GetAllContainersAndBlobs()
        {
                var containersAndBlobs = await _containerService.GetAllContainersAndBlobs();

                if (containersAndBlobs.Count == 0)
                {
                    return NotFound("No containers or blobs found.");
                }

                return Ok(containersAndBlobs);
          
        }
        [HttpPost("AddBlobData")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<bool> UploadBlob( IFormFile file)
        {
            try
            {
                string containerName = "containerone";
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);

                var httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType,
                };

                var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
                return result != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading blob: {ex.Message}");
                return false;
            }
        }
    }
}
