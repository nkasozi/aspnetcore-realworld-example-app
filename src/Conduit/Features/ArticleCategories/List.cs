using Conduit.Domain;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Features.ArticleCategories
{
    public class List
    {
        public class Query : IRequest<ArticleCategoriesEnvelope>
        {
            public Query(int categoryId, int articleId, int? limit = 20, int? offset = 0)
            {
                CategoryId = categoryId;
                ArticleId = articleId;
                Limit = limit;
                Offset = offset;
            }

            public int CategoryId { get; }
            public int ArticleId { get; }
            public int? Limit { get; }
            public int? Offset { get; }
        }

        public class QueryHandler : IRequestHandler<Query, ArticleCategoriesEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public QueryHandler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<ArticleCategoriesEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                IQueryable<ArticleCategory> queryable = _context.ArticleCategories.AsQueryable();

                //if CategoryId is set
                //Get all categories matching that ID
                if (message.CategoryId != 0)
                {
                    queryable = queryable.Where(i => i.CategoryId==message.CategoryId);
                }

                //if CategoryId is set
                //Get all categories matching that ID
                if (message.ArticleId != 0)
                {
                    queryable = queryable.Where(i => i.ArticleId == message.ArticleId);
                }

                //order the results
                var categories = await queryable
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(message.Offset ?? 0)
                    .Take(message.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                //go back
                return new ArticleCategoriesEnvelope(categories);
            }
        }
    }
}

