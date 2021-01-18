using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using Vaccination.Registration.Data;
using Vaccination.Registration.Models.Dto;

namespace Vaccination.Registration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly VaccinationContext context;
        public RegistrationController(VaccinationContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public IActionResult CreateRegistration(CreateRegistrationDto createRegistrationDto)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "vaccinations",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null
                                 );
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(createRegistrationDto));

            channel.BasicPublish(exchange: "",
                routingKey: "vaccinations",
                basicProperties: null,
                body: body
                );
            return Accepted(); // 202 Accepted

        }
    }
}
