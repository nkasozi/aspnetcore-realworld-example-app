using Conduit.Domain;
using Conduit.Features.Categories;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Features.ArticleCategories
{
    public class Create
    {
        public class ArticleCategoryDataValidator : AbstractValidator<ArticleCategory>
        {
            public ArticleCategoryDataValidator()
            {

                RuleFor(x => x.CategoryId).NotNull().NotEmpty();
                RuleFor(x => x.ArticleId).NotNull().NotEmpty();

            }
        }

        public class Command : IRequest<ArticleCategory>
        {
            public ArticleCategory articleCategory { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.articleCategory).NotNull().SetValidator(new ArticleCategoryDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, ArticleCategory>
        {
            private readonly ConduitContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<ArticleCategory> Handle(Command message, CancellationToken cancellationToken)
            {

                //check if parent category exists
                if ((!_context.Categories.Any(x => x.CategoryId == message.articleCategory.CategoryId)))
                {
                    string msg = $"ArticleCategory with ID [{message.articleCategory.CategoryId}] not Found";
                    throw new RestException(HttpStatusCode.NotFound, msg);
                }

                //check if parent category exists
                if ((!_context.Articles.Any(x => x.ArticleId == message.articleCategory.ArticleId)))
                {
                    string msg = $"ArticleCategory with ID [{message.articleCategory.CategoryId}] not Found";
                    throw new RestException(HttpStatusCode.NotFound, msg);
                }

                //success...we can create the category
                var Category = new ArticleCategory()
                {
                    CategoryId = message.articleCategory.CategoryId,
                    ArticleId = message.articleCategory.ArticleId,
                    UpdatedAt=DateTime.Now,
                    CreatedAt=DateTime.Now
                };

                await _context.ArticleCategories.AddAsync(Category, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Category;
            }

            
        }
    }
}
