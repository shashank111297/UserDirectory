using System.ComponentModel.DataAnnotations;

namespace UserDirectory.Models
{
    public class Company
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string CatchPhrase { get; set; }

        [Required]
        [StringLength(100)]
        public string Bs { get; set; }
    }
}
