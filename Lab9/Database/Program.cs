using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Util;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Lab9.Models;
using System.Data.SqlClient;

namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {
            DB db = new DB();
            db.Tariffs.Local.Count();
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            Console.WriteLine("DB started");

            while (true)
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "tariffqueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, eventArgs) =>
                    {
                        var body = eventArgs.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var deserialized = JsonConvert.DeserializeObject<Tariff>(message);
                        if (!db.Tariffs.Local.Any(item => item.Name == deserialized.Name))
                        {
                            db.Tariffs.Local.Add(deserialized);
                            db.SaveChanges();
                            foreach (var tariff in db.Tariffs.Local)
                            {
                                Console.Write(tariff.Name + " ");
                            }
                            Console.WriteLine();
                        }

                       
                    };
                    channel.BasicConsume(queue: "tariffqueue",
                        autoAck: false,
                        consumer: consumer);
                }

                Thread.Yield();
            }
        }
    }
}
