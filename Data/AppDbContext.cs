using Microsoft.EntityFrameworkCore;
using CitiesApi.Models;

namespace CitiesApi.Data;

public class AppDbContext: DbContext
{
   public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
   {

   }

   public DbSet<City> Cities {get; set;}
}
