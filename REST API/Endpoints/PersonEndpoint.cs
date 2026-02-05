using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.DTOs;

namespace REST_API.Endpoints
{
    public class PersonEndpoint
    {
        public static void RegisterEndpoint(WebApplication app)
        {
            // Endpoint to get all persons
            app.MapGet("/Persons", async (RestApiDBContext context) =>
            {
                var persons = await context.Persons.ToListAsync();

                var personList = new List<PersonDto>();

                //Mapping the person to a personDto and adding it to the personList to only return the
                //needed information to the client. PersonList is a list of personDtos.
                foreach (var person in persons)
                {
                    PersonDto personDto = new PersonDto
                    {
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        PhoneNumber = person.PhoneNumber
                    };
                    personList.Add(personDto);
                }
                return Results.Ok(personList);
            });
        }
    }
}
