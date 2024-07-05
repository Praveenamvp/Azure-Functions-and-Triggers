using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues.Models;

public static class QueueTrigger
{
    private static readonly string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    private static readonly string sourceContainerName = "containerone";
    private static readonly string destinationContainerName = "containertwo";

    [FunctionName("QueueTriggerFunction")]
    public static async Task Run([QueueTrigger("move-queue", Connection = "AzureWebJobsStorage")] QueueMessage myQueueItem, ILogger log)
    {
        log.LogInformation($"Queue trigger function processed: {myQueueItem}");

        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient sourceContainerClient = blobServiceClient.GetBlobContainerClient(sourceContainerName);
        BlobContainerClient destinationContainerClient = blobServiceClient.GetBlobContainerClient(destinationContainerName);
        BlobClient sourceBlobClient = sourceContainerClient.GetBlobClient(myQueueItem.Body.ToString());
        BlobClient destinationBlobClient = destinationContainerClient.GetBlobClient(myQueueItem.Body.ToString()); 
        BlobDownloadInfo download = await sourceBlobClient.DownloadAsync();
        await destinationBlobClient.UploadAsync(download.Content, overwrite: true);
        await sourceBlobClient.DeleteAsync();
    }
}
