using Microsoft.AspNetCore.Mvc;
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
            Thread.Sleep(3000);

            var vaccination = new Models.Domain.Vaccination
            {
                FirstName = createRegistrationDto.FirstName,
                LastName = createRegistrationDto.LastName,
                SocialSecurityNumber = createRegistrationDto.SocialSecurityNumber
            };
            context.Vaccinations.Add(vaccination);

            context.SaveChanges();

            return Ok(); // 201 OK

        }
    }
}
