using System;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;

namespace UnitTests.Models
{
    /// <summary>
    /// Unit tests for CommentModel
    /// </summary>
    public class CommentModelTests
    {
        [Test]
        public void DefaultConstructor_Should_Assign_Unique_Valid_Id()
        {
            // Arrange
            CommentModel comment1 = null;
            CommentModel comment2 = null;
            
            //Act
            comment1 = new CommentModel();
            comment2 = new CommentModel();

            // Assert
            Assert.IsNotNull(comment1.Id);
            Assert.AreNotEqual(comment1.Id, comment2.Id);
        }
    }
}
