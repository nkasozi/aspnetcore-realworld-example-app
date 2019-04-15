using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Domain
{
    public class ArticleCategory
    {
        public int ArticleCategoryId { get; set; }

        public int CategoryId { get; set; }

        public int ArticleId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
