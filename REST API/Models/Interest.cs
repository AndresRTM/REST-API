using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class Interest
    {
        public int Id { get; set; }

        [StringLength (100)]
        public string Title  { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
        public List<PersonInterestLink> Links { get; set; }
    }
}
