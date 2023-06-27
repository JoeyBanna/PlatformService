using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
using PlatformService.Models.DTOs;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        public readonly IConfiguration _configuration;
        public readonly IConnection _connection;
        public readonly IModel _chanel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() 
            {
               // HostName = "localhost",

               HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                Console.WriteLine(factory.Port);
                _connection = factory.CreateConnection();
                _chanel = _connection.CreateModel();
                _chanel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("---> connected to message bus"); 



            }
            catch (Exception ex)
            { 
                Console.WriteLine($"--> Could not connect to the message bus: {ex.Message}");

            }
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            var connectCheck = _connection.IsOpen;
            if (connectCheck)
            {
                Console.WriteLine("--->  RabbitMQ Connection Open....Sending message.....");
                SendMessage(message);
            }
            else {
                Console.WriteLine("--->  RabbitMQ Connection Is Closed....Not Sending message.....");

            }
        }
        private void  SendMessage(string message)
        { 
            var body =Encoding.UTF8.GetBytes(message);
            Console.WriteLine(body);
            _chanel.BasicPublish(exchange: "trigger", routingKey: "",basicProperties:null,body:body);
            Console.WriteLine($"We have sent {message}");
        }

        public void Dispose()
        {
            if (_connection.IsOpen)
            {
                _chanel?.Close();
                _connection?.Close();
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---> RabiitMQ connection shutdown");

        }
    }
}
