using Microsoft.EntityFrameworkCore;

namespace Vaccination.Registration.Data
{
    public class VaccinationContext: DbContext
    {
        public DbSet<RegistrationProcessor.Models.Domain.Vaccination> Vaccinations { get; set; }
                
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Vaccination; Trusted_Connection=True");
        }
        
    }
}
