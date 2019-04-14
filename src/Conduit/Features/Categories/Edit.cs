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

namespace Conduit.Features.Categories
{
    public class Edit
    {
        public class Command : IRequest<CategoryEnvelope>
        {
            public Category Category { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Category.CategoryDescription).NotNull();
                RuleFor(x => x.Category.CategoryName).NotNull();
                RuleFor(x => x.Category.CategoryId).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, CategoryEnvelope>
        {
            private readonly ConduitContext _context;

            public Handler(ConduitContext context)
            {
                _context = context;
            }

            public async Task<CategoryEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var categoryToEdit = await _context.Categories   
                    .Where(x => x.CategoryId == message.Category.CategoryId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (categoryToEdit == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Category = Constants.NOT_FOUND });
                }

                categoryToEdit.CategoryName = message.Category.CategoryName;
                categoryToEdit.CategoryDescription = message.Category.CategoryDescription;
                categoryToEdit.ParentCategoryId = message.Category.ParentCategoryId;

                if (_context.ChangeTracker.Entries().First(x => x.Entity == categoryToEdit).State == EntityState.Modified)
                {
                    categoryToEdit.UpdatedAt = DateTime.UtcNow;
                }

                // update the category
                _context.Categories.Update(categoryToEdit);

                await _context.SaveChangesAsync(cancellationToken);

                return new CategoryEnvelope(await _context.Categories
                    .Where(x => x.CategoryId == message.Category.CategoryId)
                    .FirstOrDefaultAsync(cancellationToken));
            }
        }
    }
}
