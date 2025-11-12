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
        public void Comment_Valid_And_UniqueID_On_Construction()
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
