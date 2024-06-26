using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SockAutomationSend.EmailService;


namespace SockAutomationSend;

public class ServiceBusQueueTriggerSend(ILogger<ServiceBusQueueTriggerSend> logger)
{
    private const string QueueName = "pa200-hw3-snapshots";


    [Function(nameof(ServiceBusQueueTriggerSend))]
    public void Run([ServiceBusTrigger(QueueName)] ServiceBusReceivedMessage message)
    {
        logger.LogInformation("Message ID: {id}", message.MessageId);
        logger.LogInformation("Message Body: {body}", message.Body);
        logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        var body =  message.Body?.ToString();
        EmailController.SendEmail(body ?? "Error, nothing found in queue");
    }


}
