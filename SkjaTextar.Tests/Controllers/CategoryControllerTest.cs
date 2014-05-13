using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkjaTextar.Tests.Mocks;
using SkjaTextar.Models;
using SkjaTextar.Controllers;
using System.Web.Mvc;
using SkjaTextar.ViewModels;
using System.Collections.Generic;
using SkjaTextar.Exceptions;

namespace SkjaTextar.Tests.Controllers
{
    [TestClass]
    public class CategoryControllerTest
    {
        [TestMethod]
        public void CorrectModel()
        {
            // Arrange
            var mockUnitOfWork = new MockUnitOfWork();
            for (int i = 1; i < 4; i++)
                mockUnitOfWork.CategoryRepository.Insert(new Category
                {
                    ID = i,
                    Name = "Gaman" + i,
                });
            mockUnitOfWork.MovieRepository.Insert(new Movie
            {
                ID = 1,
                Title = "Test",
                CategoryID = 1,
            });

            // Act
            var controller = new CategoryController(mockUnitOfWork);

            // Assert
            var result1 = controller.MediaByCategory(1, "Movie");
            var viewresult1 = (ViewResult)result1;
            Assert.IsInstanceOfType(viewresult1.Model, typeof(IEnumerable<Movie>));
            
        }

        [TestMethod]
        public void ThrowsErrors()
        {
            // Arrange
            var mockUnitOfWork = new MockUnitOfWork();
            for (int i = 1; i < 4; i++)
            mockUnitOfWork.CategoryRepository.Insert(new Category
            {
                ID = i,
                Name = "Gaman" + i,
            });
            mockUnitOfWork.MovieRepository.Insert(new Movie
            {
                ID = 1,
                Title = "Test",
                CategoryID = 1,
            });

            // Act
            var controller = new CategoryController(mockUnitOfWork);

            // Assert
            try
            {
                var result = controller.MediaByCategory(null, "Movie");
            }
            catch(Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }

            try
            {
                var result = controller.MediaByCategory(1, null);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }
        }
    }
}
