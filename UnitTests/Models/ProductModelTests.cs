using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using System.Text.Json;

namespace UnitTests.Models
{
    public class ProductModelTests
    {
        [Test]
        public void ToString_Valide_Return_JSON_With_Expected_Values()
        {
            // Arrange
            var product = new ProductModel
            {
                Title = "Max Holloway",
                Maker = "UFC",
            };

            // Act
            var json = product.ToString();

            // Assert
            Assert.That(json, Does.Contain("\"Title\":\"Max Holloway\""));
            Assert.That(json, Does.Contain("\"Maker\":\"UFC\""));
        }
    }
}