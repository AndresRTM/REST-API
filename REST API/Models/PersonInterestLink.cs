namespace REST_API.Models
{
    public class PersonInterestLink
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int PersonId { get; set; }
        public int InterestId { get; set; }
        public Interest Interest { get; set; }
        public Person Person { get; set; }

    }
}
