
using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Models;
using Scalar.AspNetCore;
using System;

namespace REST_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<RestApiDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/Persons", async (RestApiDBContext context) =>
            {
                var persons = await context.Persons.ToListAsync();

                return Results.Ok(persons);
            });

            app.MapGet("/PersonalInterests", async (RestApiDBContext context, string name) =>
            {
                var personalInterests = await context.Persons.Include(p => p.PersonInterestLink).ThenInclude(link => link.Interest).FirstOrDefaultAsync(p => p.FirstName == name);

                if (personalInterests == null)
                {
                    return Results.NotFound($"No person with the name \"{name}\" was found");
                }

                var linksList = personalInterests.PersonInterestLink.Select(link => new
                {
                    InterestName = link.Interest.Title,
                    InterestDescription = link.Interest.Description,
                    Url = link.Url
                });

                return Results.Ok(linksList);
            });

            app.MapGet("/PersonalLinks", async (RestApiDBContext context, string name) =>
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

            app.MapPost("/persons/{id}/AddInterest", async (RestApiDBContext context, int id, string title, string description, string url) =>
            {
                var person = await context.Persons.FindAsync(id);
                var interest = await context.Interests.FirstOrDefaultAsync(t => t.Title.ToLower() == title.ToLower());

                //if person does not exist, return not found.
                if (person == null)
                {
                    return Results.NotFound($"No person with the Id \"{id}\" was found");
                }

                //if interest does not exist, create it and link to person.
                if (interest == null)
                {
                    var newInterest = new Interest { Title = title, Description = description };
                    context.Interests.Add(newInterest);
                    await context.SaveChangesAsync();

                    context.PersonInterestLinks.Add(new PersonInterestLink
                    {
                        PersonId = id,
                        InterestId = newInterest.Id,
                        Url = url
                    });
                    await context.SaveChangesAsync();
                    Results.Ok($"Interest \"{title}\" added to \"{person.FirstName} {person.LastName}\"");
                }

                //If interest is already linked to person, return conflict.
                if (await context.PersonInterestLinks.AnyAsync(link => link.PersonId == person.Id && link.InterestId == interest.Id))
                {
                    return Results.Conflict($"The interest \"{title}\" is already linked to \"{person.FirstName} {person.LastName}\"");
                }

                //If interest exists but not linked to person, create link
                context.PersonInterestLinks.Add(new PersonInterestLink { Url = url, PersonId = person.Id, InterestId = interest.Id });
                await context.SaveChangesAsync();
                return Results.Ok($"Interest \"{title}\" added to \"{person.FirstName} {person.LastName}\"");
            });

            app.MapPost("/addlinkstointerest/{id}", async (RestApiDBContext context, int id, string title, string url) =>
            {
                var person = await context.Persons.FindAsync(id);
                var interest = await context.Interests.FirstOrDefaultAsync(i => i.Title.ToLower() == title.ToLower());
                //Check if person exists
                if (person == null)
                {
                    return Results.NotFound($"No person with the Id \"{id}\" was found");
                }
                //Check if interest exists
                if (interest == null)
                {
                    return Results.NotFound($"The interest \"{title}\" doesn´t exist in the database");
                }
                //Check if link already exists for the person and interest
                if (await context.PersonInterestLinks.AnyAsync(link => link.Url == url && link.InterestId == interest.Id && link.PersonId == person.Id))
                {
                    return Results.Conflict($"Person \"{person.FirstName} {person.LastName}\" already has the link \"{url}\" for the interest \"{title}\"");
                }

                context.PersonInterestLinks.Add(new PersonInterestLink
                {
                    PersonId = person.Id,
                    InterestId = interest.Id,
                    Url = url
                });
                await context.SaveChangesAsync();
                return Results.Ok($"Link added to interest \"{title}\" for person \"{person.FirstName} {person.LastName}\"");
            });

            app.Run();
        }
    }
}
