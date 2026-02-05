using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.DTOs;
using REST_API.Models;

namespace REST_API.Endpoints
{
    public class PersonalInterestEndponts
    {
        public static  void RegisterEndpoint(WebApplication app)
        {
            //Get all interests linked to a person, including the url for each interest.
            app.MapGet("/Persons/Interests", async (RestApiDBContext context, string name) =>
            {
                var personalInterests = await context.Persons.Include(p => p.PersonInterestLink).ThenInclude(link => link.Interest).FirstOrDefaultAsync(p => p.FirstName == name);

                if (personalInterests == null)
                {
                    return Results.NotFound($"No person with the name \"{name}\" was found");
                }

                var linksList = personalInterests.PersonInterestLink.Select(link => new PersonalInterestsDto
                {
                    Title = link.Interest.Title,
                    Description = link.Interest.Description,
                    Url = link.Url
                });

                return Results.Ok(linksList);
            });

            //Get all links for a person, including the interest title for each link.
            app.MapGet("/Persons/Links", async (RestApiDBContext context, string name) =>
            {
                var personalLinks = await context.Persons.Include(p => p.PersonInterestLink).FirstOrDefaultAsync(p => p.FirstName == name);
                if (personalLinks == null)
                {
                    return Results.NotFound($"No person with the name \"{name}\" was found");
                }

                var linksList = personalLinks.PersonInterestLink.Select(link => new
                {
                    Url = link.Url
                });
                return Results.Ok(linksList);
            });

            // Endpoint to add an interest to a person. If the interest does not exist, it will be created and linked to the person.
            // If the interest already exists but is not linked to the person, it will be linked. If the interest is already linked
            // to the person, a conflict response will be returned.
            app.MapPost("/Persons/{id}/AddInterest", async (RestApiDBContext context, int id, PersonalInterestsDto userInterest) =>
            {
                var person = await context.Persons.FindAsync(id);
                var interest = await context.Interests.FirstOrDefaultAsync(t => t.Title.ToLower() == userInterest.Title.ToLower());

                //if person does not exist, return not found.
                if (person == null)
                {
                    return Results.NotFound($"No person with the Id \"{id}\" was found");
                }

                //if interest does not exist, create it and link to person.
                if (interest == null)
                {
                    var newInterest = new Interest { Title = userInterest.Title, Description = userInterest.Description };
                    context.Interests.Add(newInterest);
                    await context.SaveChangesAsync();

                    context.PersonInterestLinks.Add(new PersonInterestLink
                    {
                        PersonId = id,
                        InterestId = newInterest.Id,
                        Url = userInterest.Url
                    });
                    await context.SaveChangesAsync();
                    Results.Ok($"Interest \"{userInterest.Title}\" added to \"{person.FirstName} {person.LastName}\"");
                }

                //If interest is already linked to person, return conflict.
                if (await context.PersonInterestLinks.AnyAsync(link => link.PersonId == person.Id && link.InterestId == interest.Id))
                {
                    return Results.Conflict($"The interest \"{userInterest.Title}\" is already linked to \"{person.FirstName} {person.LastName}\"");
                }

                //If interest exists but not linked to person, create link
                context.PersonInterestLinks.Add(new PersonInterestLink { Url = userInterest.Url, PersonId = person.Id, InterestId = interest.Id });
                await context.SaveChangesAsync();
                return Results.Ok($"Interest \"{userInterest.Title}\" added to \"{person.FirstName} {person.LastName}\"");
            });

            // Endpoint to add a link to an existing interest for a person. If the interest does not exist, a not found response will be returned.
            // If the link already exists for the person and interest, a conflict response will be returned.
            app.MapPost("/Persons/{id}/Interests", async (RestApiDBContext context, int id, ConnectInterestToLinkDto connectInterest) =>
            {
                var person = await context.Persons.FindAsync(id);
                var interest = await context.Interests.FirstOrDefaultAsync(i => i.Title.ToLower() == connectInterest.Title.ToLower());
                //Check if person exists
                if (person == null)
                {
                    return Results.NotFound($"No person with the Id \"{id}\" was found");
                }
                //Check if interest exists
                if (interest == null)
                {
                    return Results.NotFound($"The interest \"{connectInterest.Title}\" doesn´t exist in the database");
                }
                //Check if link already exists for the person and interest
                if (await context.PersonInterestLinks.AnyAsync(link => link.Url == connectInterest.Url && link.InterestId == interest.Id && link.PersonId == person.Id))
                {
                    return Results.Conflict($"Person \"{person.FirstName} {person.LastName}\" already has the link \"{connectInterest.Url}\" for the interest \"{connectInterest.Title}\"");
                }

                context.PersonInterestLinks.Add(new PersonInterestLink
                {
                    PersonId = person.Id,
                    InterestId = interest.Id,
                    Url = connectInterest.Url
                });
                await context.SaveChangesAsync();
                return Results.Ok($"Link added to interest \"{connectInterest.Title}\" for person \"{person.FirstName} {person.LastName}\"");
            });
        }
    }
}
