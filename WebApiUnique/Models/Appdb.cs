using Microsoft.EntityFrameworkCore;

namespace WebApiUnique.Models
{
    public class Appdb : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=HDC3-L-94S8B54;Initial Catalog=db-webapiunique;Integrated security=true;TrustServerCertificate=true");
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Emp> Employees { get; set; } // define entity , table
    }
}
