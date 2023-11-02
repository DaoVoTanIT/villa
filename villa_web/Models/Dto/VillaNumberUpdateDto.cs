using System.ComponentModel.DataAnnotations;

namespace villa_web.Dto;

public class VillaNumberUpdateDto
{
    [Required]

    public int VillaNo { get; set; }
    [Required]
    public int VillaID { get; set; }
    public string SpecialDetails { get; set; }

}