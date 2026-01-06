using System.ComponentModel.DataAnnotations;

namespace CitiesApi.Models;

public class City {
   [Required(ErrorMessage = "Id is required")]
   [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
   public int Id {get; set;}
   [Required(ErrorMessage = "Name is required")]
   [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be atleast 2 characters.")]
   public string Name {get; set;} = string.Empty;
   [Required(ErrorMessage = "Population is required")]
   [Range(1,100000000, ErrorMessage = " Population must be between 1 and 100 million")]
   public int Population {get; set;}
}
