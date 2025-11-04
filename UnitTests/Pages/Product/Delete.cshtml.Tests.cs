using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Pages.Product.Delete
{
    public class DeleteTests : TestBUnitHelper
    {
        #region TestSetup
        private DeleteModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new DeleteModel(TestHelper.ProductService);
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Product()
        {
            // Arrange

            // Act
            _pageModel.OnGet("bwm-2021-t8-s");

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("The Ultimate Fighter", _pageModel.Product.Title);
        }
        #endregion OnGet

        #region OnPost
        [Test]
        public void OnPost_Valid_Should_Return_RedirectToPageResult()
        {
            // Arrange
            _pageModel.Product = new ProductModel
            {
                Id = "bwm-2021-t8-s",
                Title = "The Ultimate Fighter",
                Description = "The Ultimate Fighter is an American reality television series.",
                Url = "https://www.ufc.com/tuf",
                Image = "image",
            };

            // Act
            var result = _pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }
        #endregion OnPost
    }

    public class TestBUnitHelper
    {
    }

}
