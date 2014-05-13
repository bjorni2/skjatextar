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
        public void ReturnsCorrectMedia()
        {
            // Arrange
            var mockUnitOfWork = new MockUnitOfWork();

            // Act
            var controller = new MediaController(mockUnitOfWork);

            // Assert
            try
            {
                var result = controller.Index(null);
            }
            catch(Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }

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
