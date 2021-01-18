using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using Vaccination.Registration.Data;
using Vaccination.RegistrationProcessor.Models.DTO;

namespace Vaccination.RegistrationProcessor
{
    class Program
    {
        static VaccinationContext context = new VaccinationContext();
        static void Main(string[] args)
        {            

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // We might start the consumer before the publisher, so we
            // need to make sure the queue eissts before trying to consume
            // messages from it. 

            channel.QueueDeclare(queue: "vaccinations",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);           

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);                

                var dto = JsonConvert.DeserializeObject<RegistrationDto>(json);

                var vaccination = new Models.Domain.Vaccination
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    SocialSecurityNumber = dto.SocialSecurityNumber
                };

                Console.WriteLine($"Processing vaccination registration... {vaccination.SocialSecurityNumber}");

                Thread.Sleep(3000);

                context.Vaccinations.Add(vaccination);

                context.SaveChanges();                
            };

            channel.BasicConsume(queue: "vaccinations",
            autoAck: true,
            consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();          
        
    }
}
}
