using System.ComponentModel.DataAnnotations;

namespace UserDirectory.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required]
        public string Website { get; set; }

        [Required]
        public Company Company { get; set; }
    }
}
