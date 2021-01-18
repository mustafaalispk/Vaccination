using Microsoft.EntityFrameworkCore;

namespace Vaccination.Registration.Data
{
    public class VaccinationContext: DbContext
    {
        public DbSet<Models.Domain.Vaccination> Vaccinations { get; set; }
                
        public VaccinationContext(DbContextOptions<VaccinationContext> contextOptions)
            : base(contextOptions)
        { 
           
        }
        
    }
}
