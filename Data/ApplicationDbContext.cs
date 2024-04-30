using InterestAPI.Models;
using LeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {
            
        }
        public DbSet<Person> People { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<InterestDescription> InterestDescriptions { get; set; }
        public DbSet<PersonInterest> PersonInterests { get; set; }
    }
}
