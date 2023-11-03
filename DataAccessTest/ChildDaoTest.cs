using System.Data;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class ChildDaoTest
{
    public void InsertData_ValidData_ReturnsTrue()
    {
        // Arrange
        var mockConnection = new Mock<IDbConnection>();
        var dataAccess = new ChildDao(mockConnection.Object);
        var child = new Child()
        {
            Name = "First",
            Description = "Description"
        };

        // Act
        var result = dataAccess.AddChild(child);

        // Assert
        Assert.IsNotNull(result);
    }

}