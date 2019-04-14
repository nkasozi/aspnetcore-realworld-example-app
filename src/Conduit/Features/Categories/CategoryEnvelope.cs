using Conduit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Features.Categories
{
    public class CategoryEnvelope
    {
        public CategoryEnvelope(Category category)
        {
            Category = category;
        }

        public Category Category { get; }
    }
}
