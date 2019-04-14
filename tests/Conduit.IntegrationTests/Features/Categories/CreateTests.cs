using Conduit.Features.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Conduit.IntegrationTests.Features.Categories
{
    public class CreateTests:SliceFixture
    {
        [Fact]
        public async Task CreateCategoryTest_ValidData_ExpectSuccess()
        {
            var command = new Create.Command()
            {
                Category = new Create.CategoryData()
                {
                   ParentCategory = 0,
                   Name = "TestCategory",
                   Description = "This is a test Category"
                }
            };

            var createdCategory = await CategoryHelpers.CreateCategory(this, command);

            Assert.NotNull(createdCategory);
            Assert.NotEqual(0, createdCategory.CategoryId);
        }
    }
}
