using Conduit.Domain;
using Conduit.Features.ArticleCategories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Conduit.IntegrationTests.Features.ArticleCategories
{
    public class DeleteTests : SliceFixture
    {
        [Fact]
        public async Task DeleteTest_ValidData_ExpectSuccess()
        {
            //create a test Article
            var articleCmd = new Conduit.Features.Articles.Create.Command()
            {
                Article = new Conduit.Features.Articles.Create.ArticleData()
                {
                    Title = "Test article dsergiu77",
                    Description = "Description of the test article",
                    Body = "Body of the test article",
                    TagList = new string[] { "tag1", "tag2" }
                }
            };


            //save it
            var article = await Articles.ArticleHelpers.CreateArticle(this, articleCmd);

            //create a test category
            var categoryCmd = new Conduit.Features.Categories.Create.Command()
            {
                Category = new Conduit.Features.Categories.Create.CategoryData()
                {
                    ParentCategory = 0,
                    Name = "TestCategory",
                    Description = "This is a test Category"
                }
            };

            //save it
            var createdCategory = await Categories.CategoryHelpers.CreateCategory(this, categoryCmd);


            //assign the article to the category
            var createCmd = new Create.Command()
            {
                articleCategory = new Domain.ArticleCategory()
                {
                    ArticleCategoryId = 0,
                    ArticleId = 1,
                    CategoryId = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            //save the assignment
            var createdArticleCategory = await ArticleCategoryHelpers.CreateArticleCategory(this, createCmd);

            var articleCategoryId = createdArticleCategory.ArticleCategoryId;

            //delete created article
            var deleteCmd = new Delete.Command(articleCategoryId);

            var dbContext = GetDbContext();

            var categoryDeleteHandler = new Delete.QueryHandler(dbContext);
            await categoryDeleteHandler.Handle(deleteCmd, new System.Threading.CancellationToken());

            var dbArticle = await ExecuteDbContextAsync(db => db.ArticleCategories.Where(d => d.ArticleCategoryId == deleteCmd.ArticleCategoryId).SingleOrDefaultAsync());

            Assert.Null(dbArticle);
        }
    }
}
