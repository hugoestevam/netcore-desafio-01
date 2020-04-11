using System;
using System.Text.Json.Serialization;

namespace Netcore.Desafio
{
    public class Repository
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("techs")]
        public string[] Techs { get; set; }
        
        [JsonPropertyName("likes")]
        public int Likes { get; set; }
    }
}