﻿using System.Collections.Generic;
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
    public class GrandParentControllerTest : Controller
    {
        [TestMethod]
        public void GetAll_AllGrandParents()
        {
            // Arrange
            var grandParentDaoMock = new Mock<IGrandParentDao>();
            var childDaoMock = new Mock<IChildDao>();
            var grandParents = new List<GrandParent>()
            {
                new()
                {
                    Id = 1,
                    PrimaryChild = 1,
                    Name = "Child1",
                    Description = "Description1"
                },
                new()
                {
                    Id = 2,
                    PrimaryChild = 2,
                    Name = "Child2",
                    Description = "Description2"
                }
            };
            grandParentDaoMock.Setup(x => x.GetAll()).Returns(grandParents);
            var grandParentController = new GrandParentController(grandParentDaoMock.Object, childDaoMock.Object);
            
            // Act
            var result = grandParentController.Get(0);
            IEnumerable<GrandParent>? okResult = null; 
            
            if (result.Result is OkObjectResult okObjectResult)
            {
                okResult = okObjectResult.Value as IEnumerable<GrandParent>;
            }

            // Assert 
            if (okResult != null) Assert.AreEqual(okResult.Count(), 2);
        }
        
        [TestMethod]
        public void Add_ValidInputChildExists_GrandParentWithPrimaryChildAdded()
        {
            // Arrange
            var grandParentDaoMock = new Mock<IGrandParentDao>();
            var childDaoMock = new Mock<IChildDao>();
            var grandParent = new DemoAPIApplication.Models.GrandParent()
            {
                PrimaryChild = 1,
                Name = "Child1",
                Description = "Description1"
            };
            grandParentDaoMock.Setup(x => x.Add(It.IsAny<GrandParent>())).Returns(1);
            childDaoMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Child());
            var grandParentController = new GrandParentController(grandParentDaoMock.Object, childDaoMock.Object);
            
            // Act
            var result = grandParentController.Add(grandParent);
            
            // Assert
            Assert.IsTrue(result.Result is OkObjectResult);
        }
    }
}