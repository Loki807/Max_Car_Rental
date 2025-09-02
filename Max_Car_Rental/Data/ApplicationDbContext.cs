using Max_Car_Rental.Models;
using Microsoft.AspNetCore.Server.Kestrel.Transport.NamedPipes;
using Microsoft.EntityFrameworkCore;

namespace Max_Car_Rental.Data
{
    public class ApplicationDbContext:DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        
        
        }
        public DbSet<Brand> Brands { get; set; }
       
    }
}
