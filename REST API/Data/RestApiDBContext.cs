using Microsoft.EntityFrameworkCore;
using REST_API.Models;

namespace REST_API.Data
{
    public class RestApiDBContext : DbContext
    {
        public RestApiDBContext(DbContextOptions<RestApiDBContext> options) : base(options)
        {
            
        }
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<PersonInterestLink> PersonInterestLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersonInterestLink>().HasData
                (
                    new PersonInterestLink { Id = 1, Url = "https://example.com/johns-coding-blog", PersonId = 1, InterestId = 1 },
                    new PersonInterestLink { Id = 2, Url = "https://example.com/janes-travel-vlog", PersonId = 2, InterestId = 2 },
                    new PersonInterestLink { Id = 3, Url = "https://example.com/alices-photo-gallery", PersonId = 3, InterestId = 3 },
                    new PersonInterestLink { Id = 4, Url = "https://example.com/bobs-cooking-recipes", PersonId = 4, InterestId = 4 },
                    new PersonInterestLink { Id = 5, Url = "https://example.com/charlies-music-playlist", PersonId = 5, InterestId = 5 }
                );

            modelBuilder.Entity<Person>().HasData
                (
                    new Person { Id = 1, FirstName = "John", LastName = "Doe", PhoneNumber = "123-456-7890" },
                    new Person { Id = 2, FirstName = "Jane", LastName = "Smith", PhoneNumber = "987-654-3210" },
                    new Person { Id = 3, FirstName = "Alice", LastName = "Johnson", PhoneNumber = "555-123-4567" },
                    new Person { Id = 4, FirstName = "Bob", LastName = "Brown", PhoneNumber = "444-987-6543" },
                    new Person { Id = 5, FirstName = "Charlie", LastName = "Davis", PhoneNumber = "333-222-1111" },
                    new Person { Id = 6, FirstName = "Eve", LastName = "Wilson", PhoneNumber = "777-888-9999" },
                    new Person { Id = 7, FirstName = "Frank", LastName = "Miller", PhoneNumber = "666-555-4444" },
                    new Person { Id = 8, FirstName = "Grace", LastName = "Taylor", PhoneNumber = "222-333-4444" },
                    new Person { Id = 9, FirstName = "Hank", LastName = "Anderson", PhoneNumber = "111-222-3333" },
                    new Person { Id = 10, FirstName = "Ivy", LastName = "Thomas", PhoneNumber = "888-777-6666" }
                );
            modelBuilder.Entity<Interest>().HasData
                (
                    new Interest { Id = 1, Title = "Coding", Description = "All about programming and software development." },
                    new Interest { Id = 2, Title = "Traveling", Description = "Exploring new places and cultures around the world." },
                    new Interest { Id = 3, Title = "Photography", Description = "Capturing moments through the lens." },
                    new Interest { Id = 4, Title = "Cooking", Description = "Creating delicious meals and culinary experiences." },
                    new Interest { Id = 5, Title = "Music", Description = "Enjoying and creating music across various genres." },
                    new Interest { Id = 6, Title = "Sports", Description = "Participating in and watching various sports activities." },
                    new Interest { Id = 7, Title = "Reading", Description = "Diving into books and expanding knowledge." },
                    new Interest { Id = 8, Title = "Gaming", Description = "Engaging in video games and interactive entertainment." },
                    new Interest { Id = 9, Title = "Hiking", Description = "Exploring nature through hiking and outdoor adventures." },
                    new Interest { Id = 10, Title = "Art", Description = "Creating and appreciating various forms of art." }

                );
        }
    }
}
