using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Objects;
using DemoAPIApplication.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace APIApplicationTest
{
    [TestClass]
    public class ChildControllerTest
    {
        [TestMethod]
        public void GetAll_AllChildren()
        {
            // Arrange
            var childDaoMock = new Mock<IChildDao>();
            var children = new List<Child>()
            {
                new()
                {
                    Id = 1,
                    Name = "Child1",
                    Description = "Description1"
                },
                new()
                {
                    Id = 2,
                    Name = "Child2",
                    Description = "Description2"
                }
            };
            childDaoMock.Setup(x => x.GetAll()).Returns(children);
            var childController = new ChildController(childDaoMock.Object);
            
            // Act
            var result = childController.Get(0);
            IEnumerable<Child>? okResult = null; 
            
            if (result.Result is OkObjectResult okObjectResult)
            {
                okResult = okObjectResult.Value as IEnumerable<Child>;
            }

            // Assert 
            if (okResult != null) Assert.AreEqual(okResult.Count(), 2);
        }
        
        [TestMethod]
        public void Get_ValidId_Child()
        {
            // Arrange
            var itemId = 1;
            var childDaoMock = new Mock<IChildDao>();
            var child = new Child()
            {
                Id = itemId,
                Name = "Child1",
                Description = "Description1"
            };
            childDaoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(child);
            var childController = new ChildController(childDaoMock.Object);
            
            // Act
            var result = childController.Get(itemId);
            
            // Assert
            Assert.IsTrue(result.Value?.All(x=>x.Name == "Child1"));
        }
        
        [TestMethod]
        public void Get_InvalidId_ResultNotFound()
        {
            // Arrange
            var invalidItemId = 1;
            var childDaoMock = new Mock<IChildDao>();
            var childController = new ChildController(childDaoMock.Object);
            
            // Act
            var result = childController.Get(invalidItemId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
        
        [TestMethod]
        public void Add_ValidInput_Success()
        {
            // Arrange
            var childDaoMock = new Mock<IChildDao>();
            var child = new DemoAPIApplication.Models.Child()
            {
                Name = "Child1",
                Description = "Description1"
            };
            childDaoMock.Setup(x => x.Add(It.IsAny<Child>())).Returns(1);
            var childController = new ChildController(childDaoMock.Object);
            
            // Act
            var result = childController.Add(child);
            
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
        }
        
        [TestMethod]
        public void Update_ValidInput_Success()
        {
            // Arrange
            var exsistingItemId = 1;
            var childDaoMock = new Mock<IChildDao>();
            var child = new DemoAPIApplication.Models.Child()
            {
                Name = "Child1",
                Description = "Description1"
            };
            childDaoMock.Setup(x => x.Update(It.IsAny<Child>())).Returns(1);
            var childController = new ChildController(childDaoMock.Object);
            
            // Act
            var result = childController.Update(child, exsistingItemId);
            
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
        }
        
        [TestMethod]
        public void Delete_ValidId_Success()
        {
            // Arrange
            var existingItemId = 1;
            var childDaoMock = new Mock<IChildDao>();
            childDaoMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
            var childController = new ChildController(childDaoMock.Object);
            
            // Act
            var result = childController.Delete(existingItemId);
            
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
        }
    }
}