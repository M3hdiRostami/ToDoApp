using System;
using System.Collections.Generic;
namespace ToDoAPI.Models
{
    public record ToDoTask
    {

        public Guid ID { get; set; }

        public string Description { get; set; }

        public DateTime DueTime { get; set; }

        public string Status { get; set; }

        public List<string> Tags { get; set; }


    }
}
