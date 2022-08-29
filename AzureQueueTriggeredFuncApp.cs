using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Azure.Function
{
    [Disable("AzureQueueTriggeredFuncApp_ISDISABLED")]
    public class AzureQueueTriggeredFuncApp
    {
        [FunctionName("AzureQueueTriggeredFuncApp")]
        public void Run([QueueTrigger("outqueue", Connection = "azFuncQueueStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"Message retrieved from queue: {myQueueItem}");
        }
    }
}
