using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkjaTextar.Tests.Mocks;
using SkjaTextar.Models;
using SkjaTextar.Controllers;
using System.Web.Mvc;
using SkjaTextar.ViewModels;

namespace SkjaTextar.Tests.Controllers
{
    [TestClass]
    public class MediaControllerTest
    {
        [TestMethod]
        public void ReturnsCorrectMedia()
        {
            //Arrange
            var mockUnitOfWork = new MockUnitOfWork();
            for (int i = 1; i < 3; i++)
            {
                mockUnitOfWork.MediaRepository.Insert(new Movie
                {
                    ID = i,
                });
            }
            for (int i = 3; i < 5; i++)
            {
                mockUnitOfWork.ShowRepository.Insert(new Show
                {
                    ID = i,
                });
            }
            for (int i = 5; i < 7; i++)
            {
                mockUnitOfWork.MediaRepository.Insert(new Clip
                {
                    ID = i,
                });
            }

            var controller = new MediaController(mockUnitOfWork);


            //Act
            var result = controller.Index(null);

            //Assert
            var viewresult = (ViewResult)result;
            var test = viewresult.GetType();
            var blabla = "blabla";
        }
    }
}
