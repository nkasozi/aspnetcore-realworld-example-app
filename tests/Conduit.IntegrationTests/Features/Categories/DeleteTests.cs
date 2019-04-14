using Conduit.Features.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Conduit.IntegrationTests.Features.Categories
{
    public class DeleteTests:SliceFixture
    {
        [Fact]
        public async Task DeleteTest_ValidData_ExpectSuccess()
        {
            var createCmd = new Create.Command()
            {
                Category = new Create.CategoryData()
                {
                    ParentCategory = 0,
                    Name = "TestCategory",
                    Description = "This is a test Category"
                }
            };

            var article = await CategoryHelpers.CreateCategory(this, createCmd);
            var CategoryId = article.CategoryId;

            var deleteCmd = new Delete.Command(CategoryId);

            var dbContext = GetDbContext();

            var categoryDeleteHandler = new Delete.QueryHandler(dbContext);
            await categoryDeleteHandler.Handle(deleteCmd, new System.Threading.CancellationToken());

            var dbArticle = await ExecuteDbContextAsync(db => db.Categories.Where(d => d.CategoryId == deleteCmd.CategoryId).SingleOrDefaultAsync());

            Assert.Null(dbArticle);
        }
    }
}
