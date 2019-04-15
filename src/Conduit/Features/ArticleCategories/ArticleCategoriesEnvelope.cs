using Conduit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Features.ArticleCategories
{
    public class ArticleCategoriesEnvelope
    {
        public ArticleCategoriesEnvelope(List<ArticleCategory> categories)
        {
            ArticleCategories = categories;
            ArticleCategoriesCount = categories?.Count();
        }

        public List<ArticleCategory> ArticleCategories { get; set; }

        public int? ArticleCategoriesCount { get; set; }
    }
}
