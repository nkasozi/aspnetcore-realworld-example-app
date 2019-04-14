using Conduit.Features.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Conduit.IntegrationTests.Features.Categories
{
    public class EditTests:SliceFixture
    {
        [Fact]
        public async Task EditTest_ValidData_ExpectSuccess()
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


            var createdArticle = await CategoryHelpers.CreateCategory(this, createCmd);

            var command = new Edit.Command()
            {
                Category = new Domain.Category()
                {
                    ParentCategoryId = 1,
                    CategoryName = "Updated " + createdArticle.CategoryName,
                    CategoryDescription = "Updated " + createdArticle.CategoryDescription,
                    CategoryId = createdArticle.CategoryId
                },
            };
            
            var dbContext = GetDbContext();

            var articleEditHandler = new Edit.Handler(dbContext);
            var edited = await articleEditHandler.Handle(command, new System.Threading.CancellationToken());

            Assert.NotNull(edited);
            Assert.Equal(edited.Category.ParentCategoryId, command.Category.ParentCategoryId);
            Assert.Contains(edited.Category.CategoryName, command.Category.CategoryName);
            // use assert Contains because we do not know the order in which the tags are saved/retrieved
            Assert.Contains(edited.Category.CategoryDescription, command.Category.CategoryDescription);
            Assert.Equal(edited.Category.CategoryId, command.Category.CategoryId);
        }
    }
}
