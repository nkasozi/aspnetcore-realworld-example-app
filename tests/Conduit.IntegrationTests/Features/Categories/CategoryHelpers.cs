using Conduit.Features.Categories;
using Conduit.IntegrationTests.Features.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.IntegrationTests.Features.Categories
{
    public class CategoryHelpers
    {
        public static async Task<Domain.Category> CreateCategory(SliceFixture fixture, Create.Command command)
        {
            // first create the default user
            var user = await UserHelpers.CreateDefaultUser(fixture);

            var dbContext = fixture.GetDbContext();
            var currentAccessor = new StubCurrentUserAccessor(user.Username);

            var categoryCreateHandler = new Create.Handler(dbContext, currentAccessor);
            var created = await categoryCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbCategory = await fixture.ExecuteDbContextAsync(db => db.Categories.Where(a => a.CategoryId == created.Category.CategoryId)
                .SingleOrDefaultAsync());

            return dbCategory;
        }

        public static async Task<CategoriesEnvelope> ListCategories(SliceFixture fixture, List.Query command)
        {

            // first create the default user
            var user = await UserHelpers.CreateDefaultUser(fixture);

            var dbContext = fixture.GetDbContext();
            var currentAccessor = new StubCurrentUserAccessor(user.Username);

            var createCategoryCommand = new Create.Command()
            {
                Category = new Create.CategoryData()
                {
                    ParentCategory = 0,
                    Name = "TestCategory",
                    Description = "This is a test Category"
                }
            };

            var categoryCreateHandler = new Create.Handler(dbContext, currentAccessor);
            var createdCategory = await categoryCreateHandler.Handle(createCategoryCommand, new System.Threading.CancellationToken());


            var categoryQueryHandler = new List.QueryHandler(dbContext, currentAccessor);
            var queriedCategories = await categoryQueryHandler.Handle(command, new System.Threading.CancellationToken());


            return queriedCategories;
        }
    }
}
