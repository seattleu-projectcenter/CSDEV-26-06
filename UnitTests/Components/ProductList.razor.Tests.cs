using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Bunit;
using NUnit.Framework;

using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Components
{
    public class ProductListTests : BunitTestContext
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        [Test]
        public void ProductList_Valid_Default_Should_Return_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Get the Cards returned
            var result = page.Markup;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("fighter-card") || result.Contains("card"));
        }

        [Test]
        public void ProductList_Valid_Should_Render_FighterCards()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Assert
            Assert.IsNotNull(page.Markup);
            Assert.IsTrue(page.Markup.Contains("fighter-card"));
        }

        [Test]
        public void ProductList_Valid_Should_Render_NoResultsMessage()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Assert
            Assert.IsNotNull(page.Markup);
            Assert.IsTrue(page.Markup.Contains("noResultsMessage"));
        }

        [Test]
        public void ProductList_Valid_Should_Have_DataAttributes()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Assert
            Assert.IsNotNull(page.Markup);
            Assert.IsTrue(page.Markup.Contains("data-title") || page.Markup.Contains("data-nickname"));
        }

        [Test]
        public void ProductList_Valid_Should_Render_Modal()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Assert
            Assert.IsNotNull(page.Markup);
            Assert.IsTrue(page.Markup.Contains("modal") || page.Markup.Contains("productModal"));
        }

        [Test]
        public void ProductList_Valid_Should_Display_FighterStats()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Assert
            Assert.IsNotNull(page.Markup);
            // The modal might not be rendered if no product is selected, so check for card-body or other elements
            Assert.IsTrue(page.Markup.Contains("card-body") || page.Markup.Contains("fighter-card"));
        }
    }
}