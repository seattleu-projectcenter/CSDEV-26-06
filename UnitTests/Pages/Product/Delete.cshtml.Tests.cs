using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            _pageModel.OnGet("ufc-amanda-nunes");

            // Assert
            Assert.AreEqual(true, _pageModel.ModelState.IsValid);
            Assert.AreEqual("Amanda Nunes", _pageModel.Product.Title);
        }

        [Test]
        public void OnGet_InvalidId_Should_Return_NotFound()
        {
            // Arrange
            var invalidId = "invalid-id-that-does-not-exist";

            // Act
            var result = _pageModel.OnGet(invalidId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void OnGet_NullId_Should_Return_NotFound()
        {
            // Arrange

            // Act
            var result = _pageModel.OnGet(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void OnGet_ValidId_Should_Return_PageResult()
        {
            // Arrange

            // Act
            var result = _pageModel.OnGet("ufc-amanda-nunes");

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
        }

        [Test]
        public void OnGet_ValidId_Should_Set_Product_Property()
        {
            // Arrange
            var validId = "ufc-amanda-nunes";

            // Act
            _pageModel.OnGet(validId);

            // Assert
            Assert.IsNotNull(_pageModel.Product);
            Assert.AreEqual(validId, _pageModel.Product.Id);
        }
        #endregion OnGet

        #region OnPost
        [Test]
        public void OnPost_Valid_Should_Return_RedirectToPageResult()
        {
            // Arrange
            _pageModel.Product = new ProductModel
            {
                Id = "ufc-jon-jones",
                Title = "Jon Jones",
                Description = "All-time great with mix of reach, creativity, and wrestling dominance.",
                Url = "https://www.ufc.com/athlete/jon-jones",
                Image = "image",
            };

            // Act
            var result = _pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        [Test]
        public void OnPost_Valid_Should_Redirect_To_Product_Index()
        {
            // Arrange
            _pageModel.Product = new ProductModel
            {
                Id = "ufc-zhang-weili",
                Title = "Zhang Weili",
                Description = "Strawweight champion blending power, speed, and elite conditioning.",
                Url = "https://www.ufc.com/athlete/zhang-weili",
                Image = "image",
            };

            // Act
            var result = _pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("/Product/Index", result.PageName);
        }

        [Test]
        public void OnPost_Valid_Should_Delete_Product()
        {
            // Arrange
            var productId = "ufc-alexandre-pantoja";
            _pageModel.Product = new ProductModel
            {
                Id = productId,
                Title = "Alexandre Pantoja",
                Description = "Flyweight champion known for his relentless pace, slick grappling, and fearless exchanges.",
                Url = "https://www.ufc.com/athlete/alexandre-pantoja",
                Image = "image",
            };

            // Verify product exists before deletion
            var productBeforeDelete = TestHelper.ProductService.GetProductById(productId);
            Assert.IsNotNull(productBeforeDelete, "Product should exist before deletion");

            // Act
            _pageModel.OnPost();

            // Assert - Product should be deleted
            var productAfterDelete = TestHelper.ProductService.GetProductById(productId);
            Assert.IsNull(productAfterDelete, "Product should be deleted after OnPost");
        }

        [Test]
        public void OnPost_Valid_Should_Remove_Product_From_Collection()
        {
            // Arrange
            var productId = "ufc-alex-pereira";
            _pageModel.Product = new ProductModel
            {
                Id = productId,
                Title = "Alex Pereira",
                Description = "Two-division champion with elite kickboxing roots, devastating power, and ice-cold composure.",
                Url = "https://www.ufc.com/athlete/alex-pereira",
                Image = "image",
            };

            // Get initial count
            var initialCount = TestHelper.ProductService.GetAllData().Count();

            // Act
            _pageModel.OnPost();

            // Assert - Count should decrease by 1
            var finalCount = TestHelper.ProductService.GetAllData().Count();
            Assert.AreEqual(initialCount - 1, finalCount, "Product count should decrease by 1 after deletion");
        }

        [Test]
        public void OnPost_AfterDeletion_Product_Should_Not_Be_Retrievable()
        {
            // Arrange
            var productId = "ufc-kimbo-slice";
            _pageModel.Product = new ProductModel
            {
                Id = productId,
                Title = "Kevin Ferguson",
                Description = "Street-fighting legend turned MMA sensation, known for raw power, charisma, and explosive knockouts.",
                Url = "https://www.ufc.com/athlete/kimbo-slice",
                Image = "image",
            };

            // Act
            _pageModel.OnPost();

            // Assert - Try to retrieve the deleted product
            var deletedProduct = TestHelper.ProductService.GetProductById(productId);
            Assert.IsNull(deletedProduct, "Deleted product should not be retrievable");
        }
        #endregion OnPost
    }

    public class TestBUnitHelper
    {
    }

}
