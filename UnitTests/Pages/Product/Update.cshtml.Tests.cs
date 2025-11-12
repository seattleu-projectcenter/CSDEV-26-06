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
        public void OnGet_EmptyId_Should_Redirect_To_Index()
        {
            // Act
            var result = _pageModel.OnGet("");

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("./Index", ((RedirectToPageResult)result).PageName);
        }

        [Test]
        public void OnGet_WhitespaceId_Should_Redirect_To_Index()
        {
            // Act
            var result = _pageModel.OnGet("   ");

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("./Index", ((RedirectToPageResult)result).PageName);
        }

        [Test]
        public void OnGet_ValidId_Should_Load_Product_And_Return_Page()
        {
            // Arrange
            var validId = "ufc-alexander-volkanovski";

            // Act
            var result = _pageModel.OnGet(validId);

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
            Assert.IsNotNull(_pageModel.Product);
            Assert.AreEqual("Alexander Volkanovski", _pageModel.Product.Title);
        }

        [Test]
        public void OnGet_ValidId_Should_Set_Product_Property()
        {
            // Arrange
            var validId = "ufc-alexander-volkanovski";

            // Act
            _pageModel.OnGet(validId);

            // Assert
            Assert.IsNotNull(_pageModel.Product);
            Assert.AreEqual(validId, _pageModel.Product.Id);
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
                Id = "ufc-alexander-volkanovski",
                Title = "Updated Title",
                Description = "Updated description",
                Url = "https://www.ufc.com/athlete/alexander-volkanovski",
                Image = "https://dmxg5wxfqgb4u.cloudfront.net/styles/athlete_bio_full_body/s3/2024-02/VOLKANOVSKI_ALEXANDER_L_BELT_02-17.png?itok=92BzRoRw",
                Nickname = "The Great",
                Record = "27-4-0",
                Height = "5'6\"",
                Weight = "145 lbs",
                Reach = "71.5 in",
                Stance = "Orthodox",
                DateOfBirth = "September 29, 1988",
                Status = "Active",
                Hometown = "Australia",
            };

            // Act
            var result = _pageModel.OnPost();

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual("./Index", ((RedirectToPageResult)result).PageName);
            Assert.AreEqual("ufc-alexander-volkanovski", ((RedirectToPageResult)result).RouteValues["id"]);
        }

        #endregion OnPost
    }
}
