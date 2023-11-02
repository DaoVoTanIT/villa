using System.ComponentModel.DataAnnotations;

namespace Villa_API.Dto;

public class VillaNumberDto
{
    [Required]

    public int VillaNo { get; set; }
    public string SpecialDetails { get; set; }

}