using Microsoft.AspNetCore.Mvc;
using CitiesApi.Models;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using CitiesApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CitiesApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CitiesController: ControllerBase
{


   private readonly AppDbContext _context;

   public CitiesController(AppDbContext context)
   {
      _context = context;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
   {
      var cities = await _context.Cities.ToListAsync();
      if(cities.Count == 0)
      {
         return NotFound();
      }
      return Ok(cities);
   }

   [HttpGet("{id}")]
   public async Task<ActionResult<City>> GetCity(int id)
   {
      var city = await _context.Cities.FindAsync(id);

      if(city == null)
      {
         return NotFound();
      }
      return Ok(city);
   }

   [HttpPost]
   public async Task<ActionResult<City>> AddCity(City city)
   {
      if(await _context.Cities.FindAsync(city.Id) != null)
      {
         return BadRequest(new {error= $"City with ID {city.Id} already exists"});
      }
      try
      {
         _context.Cities.Add(city);
         await _context.SaveChangesAsync();
         return CreatedAtAction(nameof(GetCity),new {id = city.Id}, city);
      }
      catch (Exception ex)
      {
         return BadRequest(new {error = ex.Message});
      }
   }

   [HttpDelete("{id}")]
   public async Task<ActionResult<City>> RemoveCity(int id)
   {
      var city = await _context.Cities.FindAsync(id);
      if(city == null)
      {
         return NotFound();
      }

      try
      {
        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
         return Ok(new {message = "City Deleted", city});
      }
      catch (Exception ex)
      {

         return BadRequest(new {error = ex.Message});
      }
   }

   [HttpPut("{id}")]
   public async Task<ActionResult<City>> UpdateCity(int id, City city)
   {
      if(id != city.Id)
      {
         return BadRequest(new {error = "Id error"});
      }

      var existingCity = await _context.Cities.FindAsync(id);
      if(existingCity == null)
      {
         return NotFound();
      }

      try
      {
         existingCity.Name = city.Name;
         existingCity.Population = city.Population;
         await _context.SaveChangesAsync();
         return Ok(new {message = "City Updated"});
      }
      catch (Exception ex)
      {
         return BadRequest(new {error = ex.Message});
      }
   }
}
