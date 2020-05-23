using Microsoft.EntityFrameworkCore;
using Practical.Models;

namespace Practical.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> option) : base(option)
        {

        }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Club> Club { get; set; }

        public DbQuery<AvarageAnnualBudget> AvarageAnuuaBudget { get; set; }
    }

    public class AvarageAnnualBudget
    {
        
        public decimal AvarageBudget { get; set; }
    }
}
