using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Domain
{
    public class Category
    {

        [JsonProperty("id")]
        public int CategoryId { get; set; }

        [JsonProperty("Name")]
        public string CategoryName { get; set; }

        [JsonProperty("Description")]
        public string CategoryDescription { get; set; }

        [JsonProperty("ParentId")]
        public int ParentCategoryId { get; set; }

        public List<Article> Articles { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
