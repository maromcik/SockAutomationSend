using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SockAutomationSend;

public class ServiceBusQueueTriggerSend
{
    private readonly ILogger<ServiceBusQueueTriggerSend> _logger;

    public ServiceBusQueueTriggerSend(ILogger<ServiceBusQueueTriggerSend> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ServiceBusQueueTriggerSend))]
    public void Run([ServiceBusTrigger("snapshots", Connection = "")] ServiceBusReceivedMessage message)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

    }
}
