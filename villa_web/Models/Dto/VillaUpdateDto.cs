using System.ComponentModel.DataAnnotations;

namespace villa_web.Dto;

public class VillaUpdateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(300)]
    public string Name { get; set; }
    public string Details { get; set; }
    [Required]
    public double Rate { get; set; }
    [Required] public int Occupancy { get; set; }
    [Required] public int Sqft { get; set; }
    [Required] public string ImageUrl { get; set; }
    public string Amenity { get; set; }

}