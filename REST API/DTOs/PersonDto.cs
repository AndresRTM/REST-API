using System.ComponentModel.DataAnnotations;

namespace REST_API.DTOs
{
    public class PersonDto
    {
        [Required]
        [MinLength (2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]       
        public string LastName { get; set; }

        [MinLength(9)]
        public string PhoneNumber { get; set; }
    }
}
