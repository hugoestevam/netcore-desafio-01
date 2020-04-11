using System;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Netcore.Desafio
{
    class Program
    {
        static readonly List<Repository> Repositories = new List<Repository>{};

        static async Task Main(string[] args)
        {
            var app = WebApplication.Create(args);            

            app.MapGet("/api/repositories", async (context) => {
                await context.Response.WriteJsonAsync(Repositories);
            });

            app.MapPost("/api/repositories", async (context) => {
                var repo = await JsonSerializer.DeserializeAsync<Repository>(context.Request.Body, _options);            
            
                repo.Id = Guid.NewGuid();

                Repositories.Add(repo);
                                
                await context.Response.WriteJsonAsync(repo);
            });

            app.MapPut("/api/repositories/{id}", async (context) => {
                var id = Guid.Parse((string)context.Request.RouteValues["id"]);                

                var repoIndex = Repositories.FindIndex(repo => repo.Id == id);

                if (repoIndex < 0) {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteJsonAsync(new { Error = "Repository not found." });
                    return;
                }

                var repo = await JsonSerializer.DeserializeAsync<Repository>(context.Request.Body, _options);

                var repoUpdated = new Repository {
                    Id = id,
                    Url = repo.Url,
                    Title = repo.Title,
                    Techs = repo.Techs,
                    Likes = 0
                }; 

                Repositories[repoIndex] = repoUpdated;

                await context.Response.WriteJsonAsync(repoUpdated);                
            });

            app.MapDelete("/api/repositories/{id}", async (context) => {
                var id = Guid.Parse((string)context.Request.RouteValues["id"]);

                var repoIndex = Repositories.FindIndex(repo => repo.Id == id);

                if (repoIndex < 0) {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteJsonAsync(new { Error = "Repository not found." });
                    return;
                }
                
                Repositories.RemoveAt(repoIndex);
                
                context.Response.StatusCode = StatusCodes.Status204NoContent;
            });

            app.MapPost("/api/repositories/{id}/like", async (context) => {
                var id = Guid.Parse((string)context.Request.RouteValues["id"]);
                Console.WriteLine($"Repo ID: {id}");
                var repoIndex = Repositories.FindIndex(repo => repo.Id == id);

                if (repoIndex < 0) {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteJsonAsync(new { Error = "Repository not found." });
                    return;
                }
                
                var likes = Repositories[repoIndex].Likes;

                Repositories[repoIndex].Likes = ++likes;

                await context.Response.WriteJsonAsync(new { Likes = likes });
            });             

            await app.RunAsync();
        } 

        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
