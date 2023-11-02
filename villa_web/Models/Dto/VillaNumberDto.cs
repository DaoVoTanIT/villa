using System.ComponentModel.DataAnnotations;

namespace villa_web.Dto;

public class VillaNumberDto
{
    [Required]

    public int VillaNo { get; set; }
    public string SpecialDetails { get; set; }

}