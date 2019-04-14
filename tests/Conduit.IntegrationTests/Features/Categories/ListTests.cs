using Conduit.Features.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Conduit.IntegrationTests.Features.Categories
{
    public class ListTests : SliceFixture
    {
        [Fact]
        public async Task ListArticleTest_ValidData_ExpectSuccess()
        {
            var command = new List.Query(0, "test");
            var categories = await CategoryHelpers.ListCategories(this, command);

            Assert.NotNull(categories);
            Assert.True(categories.Categories.Count > 0);
        }
    }
}
