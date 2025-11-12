using System.Linq;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;

namespace UnitTests.Pages.Product.AddRating
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
        //[Test]
        //public void AddRating_InValid_....()
        //{
        //    // Arrange

        //    // Act
        //    //var result = TestHelper.ProductService.AddRating(null, 1);

        //    // Assert
        //    //Assert.AreEqual(false, result);
        //}

        // ....

        [Test]
        public void AddRating_InValid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddRating_InValid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddRating_Valid_Product_Rating_5_Should_Return_True()
        {
            // Arrange

            // Get the First data item
            var data = TestHelper.ProductService.GetAllData().First();
            var countOriginal = data.Ratings?.Length ?? 0;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(countOriginal + 1, dataNewList.Ratings.Length);
            Assert.AreEqual(5, dataNewList.Ratings.Last());
        }

        [Test]
        public void AddRating_InValid_Rating_Negative_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, -1);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddRating_InValid_Rating_Above_5_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 6);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddRating_Valid_Rating_0_Should_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var countOriginal = data.Ratings?.Length ?? 0;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 0);
            var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.AreEqual(true, result);
            Assert.AreEqual(countOriginal + 1, dataNewList.Ratings.Length);
            Assert.AreEqual(0, dataNewList.Ratings.Last());
        }

        [Test]
        public void AddRating_InValid_ProductId_Does_Not_Exist_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("invalid-product-id", 3);

            // Assert
            Assert.AreEqual(false, result);
        }
        #endregion AddRating

        #region GetAllData
        [Test]
        public void GetAllData_Valid_Should_Return_All_Products()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void GetAllData_Valid_Should_Return_Correct_Count()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetAllData().ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.Greater(result.Count, 0);
        }
        #endregion GetAllData

        #region GetProductById
        [Test]
        public void GetProductById_Valid_Id_Should_Return_Product()
        {
            // Arrange
            var firstProduct = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.GetProductById(firstProduct.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(firstProduct.Id, result.Id);
        }

        [Test]
        public void GetProductById_Invalid_Id_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetProductById("invalid-id-does-not-exist");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetProductById_Null_Id_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetProductById(null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetProductById_Empty_Id_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetProductById("");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetProductById_Whitespace_Id_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetProductById("   ");

            // Assert
            Assert.IsNull(result);
        }
        #endregion GetProductById

        #region UpdateData
        [Test]
        public void UpdateData_Valid_Product_Should_Return_Updated_Product()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var updatedTitle = "Updated Test Title";
            var updatedDescription = "Updated Test Description";

            var updateModel = new ProductModel
            {
                Id = data.Id,
                Title = updatedTitle,
                Description = updatedDescription,
                Url = data.Url,
                Image = data.Image
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(updateModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedTitle, result.Title);
            Assert.AreEqual(updatedDescription.Trim(), result.Description);
        }

        [Test]
        public void UpdateData_Null_Product_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.UpdateData(null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void UpdateData_Product_With_Null_Id_Should_Return_Null()
        {
            // Arrange
            var updateModel = new ProductModel
            {
                Id = null,
                Title = "Test Title",
                Description = "Test Description",
                Url = "http://test.com",
                Image = "test.jpg"
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(updateModel);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void UpdateData_Product_With_Empty_Id_Should_Return_Null()
        {
            // Arrange
            var updateModel = new ProductModel
            {
                Id = "",
                Title = "Test Title",
                Description = "Test Description",
                Url = "http://test.com",
                Image = "test.jpg"
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(updateModel);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void UpdateData_Product_With_Invalid_Id_Should_Return_Null()
        {
            // Arrange
            var updateModel = new ProductModel
            {
                Id = "invalid-id-that-does-not-exist",
                Title = "Test Title",
                Description = "Test Description",
                Url = "http://test.com",
                Image = "test.jpg"
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(updateModel);

            // Assert
            Assert.IsNull(result);
        }
        #endregion UpdateData

        #region CreateData
        [Test]
        public void CreateData_Valid_Should_Create_New_Product()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.CreateData();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual("Enter Title", result.Title);
            Assert.AreEqual("Enter Description", result.Description);
            Assert.AreEqual("Enter URL", result.Url);
        }

        [Test]
        public void CreateData_Valid_Should_Increase_Product_Count()
        {
            // Arrange
            var initialCount = TestHelper.ProductService.GetAllData().Count();

            // Act
            TestHelper.ProductService.CreateData();
            var finalCount = TestHelper.ProductService.GetAllData().Count();

            // Assert
            Assert.AreEqual(initialCount + 1, finalCount);
        }

        [Test]
        public void CreateData_Valid_Should_Generate_Unique_Id()
        {
            // Arrange

            // Act
            var result1 = TestHelper.ProductService.CreateData();
            var result2 = TestHelper.ProductService.CreateData();

            // Assert
            Assert.AreNotEqual(result1.Id, result2.Id);
        }
        #endregion CreateData

        #region DeleteProduct
        [Test]
        public void DeleteProduct_Valid_Id_Should_Delete_Product()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var productId = data.Id;

            // Act
            var result = TestHelper.ProductService.DeleteProduct(productId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(productId, result.Id);
        }

        [Test]
        public void DeleteProduct_Valid_Id_Should_Reduce_Count()
        {
            // Arrange
            var initialCount = TestHelper.ProductService.GetAllData().Count();
            var data = TestHelper.ProductService.GetAllData().Last();

            // Act
            TestHelper.ProductService.DeleteProduct(data.Id);
            var finalCount = TestHelper.ProductService.GetAllData().Count();

            // Assert
            Assert.AreEqual(initialCount - 1, finalCount);
        }

        [Test]
        public void DeleteProduct_Invalid_Id_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.DeleteProduct("invalid-product-id");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteProduct_Null_Id_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.DeleteProduct(null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteProduct_After_Deletion_Product_Should_Not_Exist()
        {
            // Arrange
            var newProduct = TestHelper.ProductService.CreateData();
            var productId = newProduct.Id;

            // Act
            TestHelper.ProductService.DeleteProduct(productId);
            var result = TestHelper.ProductService.GetProductById(productId);

            // Assert
            Assert.IsNull(result);
        }
        #endregion DeleteProduct

    }
}