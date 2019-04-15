using Conduit.Features.ArticleCategories;
using Conduit.IntegrationTests.Features.ArticleCategories;
using Conduit.IntegrationTests.Features.Articles;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Conduit.IntegrationTests.Features.ArtcileCategories
{
    public class CreateTests : SliceFixture
    {
        [Fact]
        public async Task CreateArticleCategoryTest_ValidData_ExpectSuccess()
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
            var article = await ArticleHelpers.CreateArticle(this, articleCmd);

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

            //check thats its not null obj
            Assert.NotNull(createdArticleCategory);

            //check if it has an ID
            Assert.NotEqual(0, createdArticleCategory.ArticleCategoryId);
        }
    }
}
