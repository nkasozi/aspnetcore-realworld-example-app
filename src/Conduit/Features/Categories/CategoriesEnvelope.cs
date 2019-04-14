using Conduit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Features.Categories
{
    public class CategoriesEnvelope
    {
        public CategoriesEnvelope(List<Category> categories)
        {
            Categories = categories;
            CategoriesCount = categories?.Count();
        }

        public List<Category> Categories { get; set; }
        public int? CategoriesCount { get; set; }
    }
}
