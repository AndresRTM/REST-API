using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class Person : IdentityUser<int>
    {      

        [StringLength (50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(11)]
        public string PhoneNumber { get; set; }

        public List<PersonInterestLink> PersonInterestLink { get; set; }
    }
}
