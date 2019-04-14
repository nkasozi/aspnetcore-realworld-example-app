using Conduit.Domain;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Features.Categories
{
    public class List
    {
        public class Query : IRequest<CategoriesEnvelope>
        {
            public Query(int id, string name, int? limit = 20, int? offset = 0)
            {
                Id = id;
                Name = name;
                Limit = limit;
                Offset = offset;
            }

            public int Id { get; }
            public string Name { get; }
            public int? Limit { get; }
            public int? Offset { get; }
        }

        public class QueryHandler : IRequestHandler<Query, CategoriesEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public QueryHandler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<CategoriesEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                IQueryable<Category> queryable = _context.Categories.AsQueryable();

                //get all categories where the name like the name sent
                //if it sent
                if (!string.IsNullOrEmpty(message.Name))
                {
                    queryable = queryable.Where(i => i.CategoryName.ToUpper().Contains(message.Name.ToUpper()));
                }

                //get all categories where the Id is the Id sent
                //if it sent
                if (message.Id != 0)
                {
                    queryable = queryable.Where(i => i.CategoryId == message.Id);
                }
                
                //order the results
                var categories = await queryable
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(message.Offset ?? 0)
                    .Take(message.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                //go back
                return new CategoriesEnvelope(categories);
            }
        }
    }
}
