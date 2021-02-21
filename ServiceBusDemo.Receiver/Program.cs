using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ServiceBusDemo.Receiver
{
    class Program
    {
        const string ServiceBusConnectionString = "";
        const string QueueName = "";
        static IQueueClient queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

        static void Main(string[] args)
        {
       
          ReceiveMessageAsync().GetAwaiter().GetResult(); 
            Console.WriteLine("Completed!");          
        }

        public static async Task ReceiveMessageAsync()
        {
         
            RegisterMessageHandler();
            Console.ReadLine(); 
            //Thread.Sleep(10000);
            await queueClient.CloseAsync();
        }   

        private static void RegisterMessageHandler()
        {
            var messageHandlerOption = new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 1, AutoComplete = false };
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOption); 
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            if (message.Body != null)
                Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} body:{Encoding.UTF8.GetString(message.Body)}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"An Error {arg.Exception}");

            return Task.CompletedTask;
        }
    }
}
