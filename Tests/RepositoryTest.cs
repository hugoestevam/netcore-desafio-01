using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Netcore.Desafio.Tests
{
    public class RepositoryTest : IClassFixture<BaseTestFixture>
    {
        private readonly HttpClient _client;
        private readonly Repository _repo;

        public RepositoryTest(BaseTestFixture fixture)
        {
            _client = fixture.Client;
            _repo = new Repository
            {
                Url = "https://github.com/hugoestevam/OptimusPrime",
                Title = "OptimusPrime",
                Techs = new string[] {"HTML", "CSS", "TypeScript", "C#"}
            };
        }

        [Fact]
        public async Task GetRepositoriesTest()
        {
            var content = new StringContent(JsonSerializer.Serialize(_repo), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/repositories", content);

            var response = await _client.GetAsync("/api/repositories");

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());
            
            var body = await response.Content.ReadAsStringAsync();

            var repositories = JsonSerializer.Deserialize<IEnumerable<Repository>>(body);
            Assert.All<Repository>(repositories, 
                repo => Assert.Contains("OptimusPrime", repo.Title));
        }

        [Fact]
        public async Task PostRepositoriesTest()
        {
            var content = new StringContent(JsonSerializer.Serialize(_repo), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/repositories", content);
            var body = await response.Content.ReadAsStringAsync();
            var newRepo = JsonSerializer.Deserialize<Repository>(body);            

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());
            
            Assert.True(Guid.TryParse(newRepo.Id.ToString(), out var guidOutput));
            Assert.Equal(newRepo.Url, _repo.Url);
            Assert.Equal(newRepo.Title, _repo.Title);
            Assert.Equal(newRepo.Techs, _repo.Techs);
        }

        [Fact]
        public async Task PutRepositoriesTest()
        {
            var content = new StringContent(JsonSerializer.Serialize(_repo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/repositories", content);
            var body = await response.Content.ReadAsStringAsync();

            var newRepo = JsonSerializer.Deserialize<Repository>(body);

            newRepo.Url = "https://github.com/hugoestevam/DiarioAcademia";
            newRepo.Title = "DiarioAcademia";

            content = new StringContent(JsonSerializer.Serialize(newRepo), Encoding.UTF8, "application/json");
            response = await _client.PutAsync($"/api/repositories/{newRepo.Id}", content);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());
            
            body = await response.Content.ReadAsStringAsync();

            var updatedRepository = JsonSerializer.Deserialize<Repository>(body);
            Assert.True(Guid.TryParse(updatedRepository.Id.ToString(), out var guidOutput));
            Assert.Equal(updatedRepository.Url, newRepo.Url);
            Assert.Equal(updatedRepository.Title, newRepo.Title);
            Assert.Equal(updatedRepository.Techs, newRepo.Techs);
        }

        [Fact]
        public async Task PutRepositoriesThatDoesNotExistTest()
        {
            var content = new StringContent(JsonSerializer.Serialize(string.Empty), Encoding.UTF8, "application/json");       

            var response = await _client.PutAsync($"/api/repositories/{Guid.NewGuid()}", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // Status Code 400            
        }

        [Fact]
        public async Task PutRepositoriesNotBeAbleLikeManuallyTest()
        {            
            var content = new StringContent(JsonSerializer.Serialize(_repo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/repositories", content);
            var body = await response.Content.ReadAsStringAsync();

            var newRepo = JsonSerializer.Deserialize<Repository>(body);

            newRepo.Likes = 15;

            content = new StringContent(JsonSerializer.Serialize(newRepo), Encoding.UTF8, "application/json");
            response = await _client.PutAsync($"/api/repositories/{newRepo.Id}", content);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());
            
            body = await response.Content.ReadAsStringAsync();
            var updatedRepository = JsonSerializer.Deserialize<Repository>(body);
            Assert.Equal(0, updatedRepository.Likes);
        }

        [Fact]
        public async Task DeleteRepositoriesTest()
        {          
            var content = new StringContent(JsonSerializer.Serialize(_repo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/repositories", content);
            var newRepo = JsonSerializer.Deserialize<Repository>(await response.Content.ReadAsStringAsync());

            response = await _client.DeleteAsync($"/api/repositories/{newRepo.Id}");
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            response = await _client.GetAsync("/api/repositories");
            var body = await response.Content.ReadAsStringAsync();
            var repositories = JsonSerializer.Deserialize<IEnumerable<Repository>>(body);            
            Assert.DoesNotContain(repositories, repo => repo.Id == newRepo.Id);
        }

        [Fact]
        public async Task DeleteRepositoriesThatDoesNotTest()
        {            
            var response = await _client.DeleteAsync($"/api/repositories/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}