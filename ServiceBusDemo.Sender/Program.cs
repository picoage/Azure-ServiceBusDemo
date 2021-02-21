using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

string connectionString = "";
string QueueName = "";
SendMessage().GetAwaiter().GetResult();


async Task SendMessage()
{
    var queueClient = new QueueClient(connectionString, QueueName);



    for (int i = 0; i < 10; i++)
    {
        var content = $"Message{i}";

        var message = new Message(Encoding.UTF8.GetBytes(content));

       await queueClient.SendAsync(message);     

    }
  Console.WriteLine("MessageSent");
   await queueClient.CloseAsync();
}





