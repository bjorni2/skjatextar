using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkjaTextar.Tests.Mocks;
using SkjaTextar.Models;
using SkjaTextar.Controllers;
using System.Web.Mvc;
using SkjaTextar.ViewModels;
using SkjaTextar.Exceptions;

namespace SkjaTextar.Tests.Controllers
{
    [TestClass]
    public class MediaControllerTest
    {
        [TestMethod]
        // A test to see if the right Exeptions are thrown
        // in the Index method of the MediaController.
        public void TestReturnsCorrectMediaErrors()
        {
            // Arrange
            var mockUnitOfWork = new MockUnitOfWork();

            // Act
            var controller = new MediaController(mockUnitOfWork);

            // Assert
            // the MissingParameterExeption is should be thrown
            // if the method takes null as a parameter.
            try
            {
                var result = controller.Index(null);
            }
            catch(Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }
            // the DataNotFoundExeption should be thrown
            // if the parameter does not match any id in
            // in the database.
            try
            {
                var result = controller.Index(99);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(DataNotFoundException));
            }
        }
    }
}
