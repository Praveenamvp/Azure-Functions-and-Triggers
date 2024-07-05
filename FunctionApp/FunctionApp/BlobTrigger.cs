using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues;
using System.Text;

namespace FunctionApp
{
    public class BlobTrigger
    {

        private static readonly string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private static readonly string queueName = "move-queue";
        [FunctionName("BlobTrigger")]
        public  async Task Run([BlobTrigger("containerone/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
        string name,
        ILogger log)
        {
            log.LogInformation($"Blob trigger function processed blob\n Name: {name} \n Size: {myBlob.Length} Bytes");

            QueueClient queueClient = new QueueClient(connectionString, queueName);
            if (!queueClient.Exists())
            {
                await queueClient.CreateAsync();
            }

            string message = name;
            TimeSpan visibilityTimeout = TimeSpan.FromMinutes(1);
            log.LogInformation($"before");

            var msg = Encoding.UTF8.GetBytes(message);
            await queueClient.SendMessageAsync(Convert.ToBase64String(msg), visibilityTimeout);
            log.LogInformation($"after");
        }
    }
}


