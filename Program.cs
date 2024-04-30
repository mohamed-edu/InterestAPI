

using InterestAPI.Models;
using LeaveAPI.Data;
using LeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            // get all people
            app.MapGet("/People", async (ApplicationDbContext context) =>
            {
                var people = await context.People.ToListAsync();
                if(people == null||!people.Any())
                {
                    return Results.NotFound("Not Found");
                }
                return Results.Ok(people);
            });

            // Add a new person
            app.MapPost("/people", async (Person person, ApplicationDbContext context) =>
            {
                context.People.Add(person);
                await context.SaveChangesAsync();
                return Results.Created($"/People/{person.PersonId}", person);
            });

            //return interest of 1 person
            app.MapGet("/people/{personId}/interestsDescription", async (int personId, ApplicationDbContext context) =>
            {
                var person = await context.People.FindAsync(personId);
                if (person == null)
                {
                    return Results.NotFound("Not Found");
                }

                var interests = await context.PersonInterests
                    .Where(p => p.FkPersonId == personId)
                    .Select(p => p.InterestDescription)
                    .ToListAsync();

                if (interests == null || !interests.Any())
                {
                    return Results.NotFound("Not Found");
                }
                return Results.Ok(interests);
            });

            // add new interest
            app.MapPost("/people/{personId}/interestsDescription", async (int personId, InterestDescription newInterestDescription, ApplicationDbContext context) =>
            {
                var person = await context.People.FindAsync(personId);
                if (person == null)
                {
                    return Results.NotFound("Person not found.");
                }

                var interestDescription = await context.InterestDescriptions
                    .FirstOrDefaultAsync(i => i.InterestName == newInterestDescription.InterestName);

                if (interestDescription == null)
                {
                    interestDescription = new InterestDescription
                    {
                        InterestName = newInterestDescription.InterestName,
                        Description = newInterestDescription.Description
                    };
                    context.InterestDescriptions.Add(interestDescription);
                    await context.SaveChangesAsync();
                }
                if (interestDescription != null)
                {
                    var newPersonInterest = new PersonInterest
                    {
                        FkPersonId = personId,
                        FKInterestDescriptionId = interestDescription.InterestDescriptionId
                    };
                    context.PersonInterests.Add(newPersonInterest);
                    await context.SaveChangesAsync();
                }

                return Results.Created($"/people/{personId}/interestsDescription", interestDescription);
            });

            // get link for 1 interest
            app.MapGet("/people/{personId}/URL", async (int personId, ApplicationDbContext context) =>
            {
                var person = await context.People.FindAsync(personId);

                if (person == null)
                {
                    return Results.NotFound("ID: Not Found");
                }

                var links = await context.PersonInterests
                    .Where(l => l.FkPersonId == personId && l.Link != null)
                    .Select(l => l.Link)
                    .ToListAsync();

                if (links == null || !links.Any())
                {
                    return Results.NotFound("Not Found");
                }

                return Results.Ok(links);
            });
            // ADD new Link
            app.MapPost("/people/{personId}/interestDescriptions/{InterestDescriptionId}/links", async (int personId, int InterestDescriptionId, Link newURL, ApplicationDbContext context) =>
            {
                var person = await context.People.FindAsync(personId);
                if (person == null)
                {
                    return Results.NotFound("ID: Not Found");
                }

                var Theinterest = await context.InterestDescriptions.FindAsync(InterestDescriptionId);
                if (Theinterest == null)
                {
                    return Results.NotFound("Hittar inga intressen för denna person.");
                }

                var link = new Link
                {
                    URL = newURL.URL,
                    LinkName = newURL.LinkName
                };

                context.Links.Add(link);
                await context.SaveChangesAsync();

                var newPersonInterestLink = new PersonInterest
                {
                    FkPersonId = personId,
                    FKInterestDescriptionId = InterestDescriptionId,
                    LinkId = link.LinkId
                };

                context.PersonInterests.Add(newPersonInterestLink);
                await context.SaveChangesAsync();

                return Results.Created($"/persons/{personId}/interests/{InterestDescriptionId}/links", newPersonInterestLink);
            });


            app.Run();
        }
    }
}
