
using Dapper;
using Microsoft.Data.SqlClient;

namespace DockerCourseWebApi
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

            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");

            app.MapGet("/podcasts", async () => {

                var db = new SqlConnection("Server=tcp:localhost;Initial Catalog=podcasts;Persist Security Info=False;User ID=sa;Password=Dometrain#123;TrustServerCertificate=true;");

                var podcasts = (await db.QueryAsync<PodCasts>("SELECT Id, Title FROM Podcasts")).Select(x => x.Title).ToList();              

                return podcasts.Shuffle();

            });

            app.UseCors(b => b.AllowAnyMethod().AllowAnyOrigin());

            app.Run();
        }
    }

    record PodCasts(Guid Id, string Title);


}
