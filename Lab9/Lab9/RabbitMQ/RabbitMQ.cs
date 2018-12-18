using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using RabbitMQ.Util;
using RabbitMQ.Client.Events;
using System.Text;
using Lab9.Models;
using Newtonsoft.Json;

namespace Lab9.RabbitMQ
{
    public class Sender
    {

        public void Send(Tariff tariff)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "tariffqueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var command = new Tariff()
                {
                    Name = tariff.Name,
                    Operator = tariff.Operator,
                    Payroll = tariff.Payroll,
                    InnerCallsMinutes = tariff.InnerCallsMinutes,
                    OuterCallsMinutes = tariff.OuterCallsMinutes,
                    InternetMB = tariff.InternetMB,
                    SMSCount = tariff.SMSCount
                };
                string message = JsonConvert.SerializeObject(command);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "tariffqueue",
                    basicProperties: null,
                    body: body);
            }
        }

    }

}