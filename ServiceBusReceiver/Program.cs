using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBusShared.Models;

namespace ServiceBusReceiver
{
    class Program
    {
        const string connectionString = "";
        private const string queueName = "personqueue";
        private static IQueueClient queueClient;


        static async Task Main(string[] args)
        {
            queueClient = new QueueClient(connectionString, queueName);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            Console.ReadLine();

            await queueClient.CloseAsync();
        }

        private async static Task ProcessMessagesAsync(Message msg, CancellationToken ct)
        {
            var messageData = Encoding.UTF8.GetString(msg.Body);
            var personModel = JsonSerializer.Deserialize<PersonModel>(messageData);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Received message");
            Console.WriteLine($"First Name:{personModel.FirstName}");
            Console.WriteLine($"Last Name: {personModel.LastName}");
            Console.WriteLine();

            await queueClient.CompleteAsync(msg.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Message handler exception: {arg.Exception}");

            return Task.CompletedTask;
        }
    }
}
