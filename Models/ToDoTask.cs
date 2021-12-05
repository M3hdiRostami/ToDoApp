using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ToDoAPI.Models
{
    public class ToDoTask
    {
        [JsonPropertyName("ID")]
        public Guid ID { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("DueTime")]
        public DateTime DueTime { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("Tags")]
        public List<string> Tags { get; set; }


    }
}
