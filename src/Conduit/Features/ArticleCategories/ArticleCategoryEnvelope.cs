using Conduit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Features.ArticleCategories
{
    public class ArticleCategoryEnvelope
    {
        public ArticleCategoryEnvelope(ArticleCategory article)
        {
            Article = article;
        }

        public ArticleCategory Article { get; }
    }
}
