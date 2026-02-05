using System.ComponentModel.DataAnnotations;

namespace REST_API.DTOs
{
    public class PersonalInterestsDto
    {
        public string Title { get; set; }      
        public string Description { get; set; }
        public string Url { get; set; }

    }
}
