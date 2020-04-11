using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Netcore.Desafio;

namespace Netcore.Desafio.Tests
{
    public class BaseTestFixture
    {
        public HttpClient Client { get; set; }

        public BaseTestFixture()
        {
            var type = typeof(Repository).Assembly.GetType("Netcore.Desafio.Program");
            var methodInfo = type.GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Static);

            string[] args = { "urls=http://localhost:5000" };
            methodInfo.Invoke(null, new[] { args });

            Client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }        
    }
}