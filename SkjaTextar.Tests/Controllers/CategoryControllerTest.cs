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
        // Here we test whether the Media by category
        // returns IEnumerable of the right type 
        // of media.

        public void TestCorrectModel()
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
        // Here we check whether or not the Media by
        // category method throws the right errors.
        public void TestMediaByCategoryThrowsErrors()
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

            var controller = new CategoryController(mockUnitOfWork);
            // Act
            try
            {
                var result = controller.MediaByCategory(null, "Movie");
            }
            // Assert
            catch(Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }

            // Act
            try
            {
                var result = controller.MediaByCategory(1, null);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }
        }
    }
}
