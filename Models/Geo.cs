using System.ComponentModel.DataAnnotations;

namespace UserDirectory.Models
{
    public class Geo
    {
        [Required]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Latitude must be a valid number.")]
        public string Lat { get; set; }

        [Required]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "Longitude must be a valid number.")]
        public string Lng { get; set; }
    }
}
