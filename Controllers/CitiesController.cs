using Microsoft.AspNetCore.Mvc;
using CitiesApi.Models;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace CitiesApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CitiesController: ControllerBase
{
   private readonly Dictionary<int, City> _cities;
   private const string FilePath = "Data/Cities.json";

   public CitiesController()
   {
      try
      {
         var jsonString = System.IO.File.ReadAllText(FilePath);
         var citiesList = JsonSerializer.Deserialize<List<City>>(jsonString) ?? new List<City>();
         _cities = citiesList.ToDictionary(c => c.Id);
      }
      catch (FileNotFoundException)
      {
      Console.WriteLine("File not found, making a new dict.");
       _cities = new Dictionary<int, City>();
      }
      catch (JsonException)
      {
      Console.WriteLine("Json is bad. Making a new empty dict.");
      _cities = new Dictionary<int, City>();
      }
   }

   [HttpGet]
   public ActionResult<Dictionary<int, City>> GetAllCities()
   {
      if(_cities.Count == 0)
      {
         return NotFound();
      }
      return Ok(_cities);
   }

   [HttpGet("{id}")]
   public ActionResult<City> GetCity(int id)
   {
      if(!_cities.ContainsKey(id))
      {
         return NotFound();
      }
      return Ok(_cities[id]);
   }

   [HttpPost]
   public async Task<ActionResult<City>> AddCity(City city)
   {
      if(_cities.ContainsKey(city.Id))
      {
         return BadRequest(new {error= $"City with ID {city.Id} already exists"});
      }
      try
      {
         _cities[city.Id] = city;
         await SaveToFileAsync();

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
      if(!_cities.ContainsKey(id))
      {
         return NotFound();
      }

      try
      {
         var deletedCity = _cities[id];
         _cities.Remove(id);
         await SaveToFileAsync();

         return Ok(new {message = "City Deleted", city = deletedCity});
      }
      catch (Exception ex)
      {

         return BadRequest(new {error = ex.Message});
      }
   }

   [HttpPut("{id}")]
   public async Task<ActionResult<City>> UpdateCity(int id, City city)
   {
      if(!_cities.ContainsKey(id))
      {
         return NotFound();
      }

      try
      {
         _cities[id] = city;
         await SaveToFileAsync();

         return Ok(new {message = "City Updated"});
      }
      catch (Exception ex)
      {
         return BadRequest(new {error = ex.Message});
      }
   }

   private async Task SaveToFileAsync()
   {
      var citiesList = _cities.Values.ToList();
      var jsonString = JsonSerializer.Serialize(citiesList, new JsonSerializerOptions {WriteIndented = true});
      await System.IO.File.WriteAllTextAsync(FilePath, jsonString);
   }

}
