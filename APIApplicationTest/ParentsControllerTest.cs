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
    public class ParentsControllerTest
    {
        [TestMethod]
        public void GetAll_AllParents()
        {
            // Arrange
            var parentDaoMock = new Mock<IParentsRepository>();
            var grandParentDaoMock = new Mock<IGrandParentsRepository>();
            var parents = new List<Parent>()
            {
                new()
                {
                    Id = 1,
                    Name = "Parent1",
                    Description = "Description1"
                },
                new()
                {
                    Id = 2,
                    Name = "Parent2",
                    Description = "Description2"
                }
            };
            parentDaoMock.Setup(x => x.GetAll()).Returns(parents);
            var parentController = new ParentsController(parentDaoMock.Object, grandParentDaoMock.Object);

            // Act
            var result = parentController.Get(0);
            IEnumerable<Child>? okResult = null;

            if (result.Result is OkObjectResult okObjectResult)
            {
                okResult = okObjectResult.Value as IEnumerable<Child>;
            }

            // Assert 
            if (okResult != null) Assert.AreEqual(okResult.Count(), 2);
        }

        [TestMethod]
        public void Update_ValidGrandParentId_ParentUpdatedWithGrandParent()
        {
            // Arrange
            var parentDaoMock = new Mock<IParentsRepository>();
            var grandParentDaoMock = new Mock<IGrandParentsRepository>();
            var parent = new DemoAPIApplication.Models.Parent()
            {
                Name = "Parent1",
                Description = "Description1"
            };
            var granParentID = 1;

            parentDaoMock.Setup(x => x.Update(It.IsAny<Parent>())).Returns(1);

            // Act
            var parentController = new ParentsController(parentDaoMock.Object, grandParentDaoMock.Object);
            var result = parentController.Update(parent, granParentID);

            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
        }
    }
}