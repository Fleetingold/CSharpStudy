using System;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQDemo
{
    class Send
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "10.168.4.114", UserName= "guest", Password= "guest", Port = AmqpTcpEndpoint.UseDefaultPort, Protocol = Protocols.DefaultProtocol };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
