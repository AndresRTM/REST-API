
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

            app.MapGet("/Persons", (RestApiDBContext context) =>
            {
                var persons = context.Persons.ToList();

                return Results.Ok(persons);
            });

            app.MapGet("/PersonalInterests", (RestApiDBContext context, string name) =>
            {
                var personalInterests = context.Persons.Include(p => p.PersonInterestLink).ThenInclude(link => link.Interest).FirstOrDefault(p => p.FirstName == name);

                if (personalInterests == null)
                {
                    return Results.NotFound($"No person with the name {name} was found");
                }

                var linksList = personalInterests.PersonInterestLink.Select(link => new
                {
                    InterestName = link.Interest.Title,
                    InterestDescription = link.Interest.Description,
                    Url = link.Url
                });

                return Results.Ok(linksList);
            });

            app.MapGet("/PersonalLinks", (RestApiDBContext context, string name) =>
            {
                var personalLinks = context.Persons.Include(p => p.PersonInterestLink).FirstOrDefault(p => p.FirstName == name);
                if (personalLinks == null)
                {
                    return Results.NotFound($"No person with the name {name} was found");
                }
                var linksList = personalLinks.PersonInterestLink.Select(link => new
                {
                    Url = link.Url
                });
                return Results.Ok(linksList);
            });

            app.MapPost("/persons/{id}/AddInterest", (RestApiDBContext context, int id, string title, string description, string url) =>
            {
                var person = context.Persons.Find(id);
                var interest = context.Interests.FirstOrDefault(t => t.Title.ToLower() == title.ToLower());

                //if person does not exist, return not found.
                if (person == null)
                {
                    return Results.NotFound($"No person with the Id {id} was found");
                }

                //if interest does not exist, create it and link to person.
                if (interest == null)
                {
                    var newInterest = new Interest { Title = title, Description = description };
                    context.Interests.Add(newInterest);

                    context.PersonInterestLinks.Add(new PersonInterestLink
                    {
                        PersonId = id,
                        InterestId = newInterest.Id,
                        Url = url
                    });
                    context.SaveChanges();
                    Results.Ok($"Interest {title} added to {person.FirstName + person.LastName} ");
                }

                //If interest is already linked to person, return conflict.
                if (context.PersonInterestLinks.Any(link => link.PersonId == person.Id && link.InterestId == interest.Id))
                {
                    return Results.Conflict($"The interest {title} is already linked to {person.FirstName + person.LastName}");
                }

                //If interest exists but not linked to person, create link
                context.PersonInterestLinks.Add(new PersonInterestLink { Url = url, PersonId = person.Id, InterestId = interest.Id });
                return Results.Ok();
            });


            app.Run();
        }
    }
}
