using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Netcore.Desafio.Tests
{
    public class LikeTest : IClassFixture<BaseTestFixture>
    {
        private readonly HttpClient _client;
        private readonly Repository _repo;

        public LikeTest(BaseTestFixture fixture)
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
        public async Task PostLikeInRepositoryTest()
        {
            var content = new StringContent(JsonSerializer.Serialize(_repo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/repositories", content);
            var body = await response.Content.ReadAsStringAsync();
            var repo = JsonSerializer.Deserialize<Repository>(body);

            response = await _client.PostAsync($"/api/repositories/{repo.Id}/like", content);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());
            body = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<LikeResult>(body);
            Assert.Equal(1, result.Likes);

            //Again
            response = await _client.PostAsync($"/api/repositories/{repo.Id}/like", content);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());
            body = await response.Content.ReadAsStringAsync();

            result = JsonSerializer.Deserialize<LikeResult>(body);
            Assert.Equal(2, result.Likes);
        }  

        [Fact]
        public async Task PostLikeInRepositoryThatDoesNotTest()
        {       
            var content = new StringContent(JsonSerializer.Serialize(_repo), Encoding.UTF8, "application/json");     
            var response = await _client.PostAsync($"/api/repositories/{Guid.NewGuid()}/like", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }      
    }
    
    public class LikeResult {
        public int Likes { get; set; }
    }
}