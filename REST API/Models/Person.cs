using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class Person
    {
        public int Id { get; set; }
        [StringLength (50)]

        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(11)]
        public string PhoneNumber { get; set; }

        public List<PersonInterestLink> PersonInterestLink { get; set; }
    }
}
