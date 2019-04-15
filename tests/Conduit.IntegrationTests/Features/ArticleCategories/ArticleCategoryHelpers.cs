using Conduit.Domain;
using Conduit.Features.ArticleCategories;
using Conduit.IntegrationTests.Features.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.IntegrationTests.Features.ArticleCategories
{
    public class ArticleCategoryHelpers
    {
        public static async Task<Domain.ArticleCategory> CreateArticleCategory(SliceFixture fixture, Create.Command command)
        {
            // first create the default user
            var user = await UserHelpers.CreateDefaultUser(fixture);

            var dbContext = fixture.GetDbContext();
            var currentAccessor = new StubCurrentUserAccessor(user.Username);

            var categoryCreateHandler = new Create.Handler(dbContext, currentAccessor);
            var created = await categoryCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbCategory = await fixture.ExecuteDbContextAsync(db => db.ArticleCategories.Where(a => a.ArticleCategoryId == created.ArticleCategoryId)
                .SingleOrDefaultAsync());

            return dbCategory;
        }

        public static async Task<ArticleCategoriesEnvelope> ListCategories(SliceFixture fixture, Conduit.Features.ArticleCategories.List.Query command)
        {

            // first create the default user
            var user = await UserHelpers.CreateDefaultUser(fixture);

            var dbContext = fixture.GetDbContext();
            var currentAccessor = new StubCurrentUserAccessor(user.Username);

            var createCategoryCommand = new Create.Command()
            {
                articleCategory = new ArticleCategory()
                {
                    CategoryId = 1,
                    ArticleId  = 1
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
