using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;

namespace UnitTests.Pages.Product.Update
{
    public class UpdateTests
    {
        private UpdateModel _pageModel;

        [SetUp]
        public void TestInitialize()
        {
            _pageModel = new UpdateModel(TestHelper.ProductService);
        }

        #region OnGet

        [Test]
        public void OnGet_NullId_Should_Redirect_To_Index()
        {
            // Act
            var result = _pageModel.OnGet(null);

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("./Index", ((RedirectToPageResult)result).PageName);
        }

        [Test]
        public void OnGet_UnknownId_Should_Redirect_To_Index()
        {
            // Act
            var result = _pageModel.OnGet("does-not-exist");

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("./Index", ((RedirectToPageResult)result).PageName);
        }

        [Test]
        public void OnGet_ValidId_Should_Load_Product_And_Return_Page()
        {
            // Act
            var result = _pageModel.OnGet("ufc-amanda-nunes");

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsNotNull(_pageModel.Product);
            Assert.AreEqual("Amanda Nunes", _pageModel.Product.Title);
        }

        #endregion OnGet

        #region OnPost

        [Test]
        public void OnPost_Invalid_Model_Should_Return_Page()
        {
            // Arrange
            _pageModel.ModelState.AddModelError("Title", "required");

            // Act
            var result = _pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
        }

        [Test]
        public void OnPost_Update_Fails_Should_Return_Page_With_Error()
        {
            // Arrange
            _pageModel.Product = new ProductModel
            {
                Id = "missing-id",
                Title = "Test",
                Description = "Desc",
                Image = "img",
                Url = "url"
            };

            // Act
            var result = _pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsFalse(_pageModel.ModelState.IsValid);
        }

        [Test]
        public void OnPost_Valid_Update_Should_Redirect_To_Index()
        {
            // Arrange
            _pageModel.Product = new ProductModel
            {
                Id = "ufc-amanda-nunes",
                Title = "Updated Title",
                Description = "Updated description",
                Url = "https://www.ufc.com/tuf",
                Image = "image",
                Nickname = "Nickname",
                Record = "10-0",
                Height = "6'0\"",
                Weight = "200 lbs",
                Reach = "75 in",
                Stance = "Orthodox",
                DateOfBirth = "Jan 1 1990",
                Status = "Active",
                Hometown = "Anywhere",
            };

            // Act
            var result = _pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("./Index", ((RedirectToPageResult)result).PageName);
            Assert.AreEqual("ufc-amanda-nunes", ((RedirectToPageResult)result).RouteValues["id"]);
        }

        #endregion OnPost
    }
}
