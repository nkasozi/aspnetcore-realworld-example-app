using Conduit.Domain;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Features.ArticleCategories
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Command(int categoryId)
            {
                ArticleCategoryId = categoryId;
            }

            public int ArticleCategoryId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ArticleCategoryId).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Command>
        {
            private readonly ConduitContext _context;

            public QueryHandler(ConduitContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                ArticleCategory articleCategory = await _context.ArticleCategories
                    .FirstOrDefaultAsync(x => x.ArticleCategoryId == message.ArticleCategoryId, cancellationToken);

                if (articleCategory == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { ArticleCategory = Constants.NOT_FOUND });
                }

                _context.ArticleCategories.Remove(articleCategory);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
