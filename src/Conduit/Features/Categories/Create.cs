using Conduit.Domain;
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

namespace Conduit.Features.Categories
{
    public class Create
    {

        public class CategoryData
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public int ParentCategory { get; set; }


            public CategoryData()
            {
                

            }
        }
        public class CategoryDataValidator : AbstractValidator<CategoryData>
        {
            public CategoryDataValidator()
            {

                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Description).NotNull().NotEmpty();
                //RuleFor(x => _context.Categories.Where(y => y.CategoryId == x.ParentCategory).FirstOrDefault()).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<CategoryEnvelope>
        {
            public CategoryData Category { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Category).NotNull().SetValidator(new CategoryDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, CategoryEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<CategoryEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {

                //check if parent category exists
                if ((message.Category.ParentCategory != 0 && !_context.Categories.Any(x => x.CategoryId == message.Category.ParentCategory)))
                {
                    string msg = $"Parent Category with ID [{message.Category.ParentCategory}] not Found";
                    throw new RestException(HttpStatusCode.NotFound, msg);
                }

                //success...we can create the category
                var Category = new Category()
                {

                    ParentCategoryId = message.Category.ParentCategory,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CategoryDescription = message.Category.Description,
                    CategoryName = message.Category.Name,
                };

                await _context.Categories.AddAsync(Category, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return new CategoryEnvelope(Category);
            }
        }
    }
}
