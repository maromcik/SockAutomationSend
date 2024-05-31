using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SockAutomationSend;

public class ServiceBusQueueTriggerSend(ILogger<ServiceBusQueueTriggerSend> logger, ServiceBusClient serviceBusClient)
{
    private const string QueueName = "pa200-hw3-snapshots";


    [Function(nameof(ServiceBusQueueTriggerSend))]
    public void Run([ServiceBusTrigger(QueueName,  Connection = "CUSTOMCONNSTR_SERVICE_BUS")] ServiceBusReceivedMessage message)
    {
        logger.LogInformation("Message ID: {id}", message.MessageId);
        logger.LogInformation("Message Body: {body}", message.Body);
        logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

    }


}
