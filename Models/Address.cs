using System.ComponentModel.DataAnnotations;

namespace UserDirectory.Models
{
    public class Address
    {
        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string Suite { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid zipcode format.")]
        public string Zipcode { get; set; }

        [Required]
        public Geo Geo { get; set; }
    }
}
