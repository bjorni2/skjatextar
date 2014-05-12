using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SkjaTextar.Models;
using SkjaTextar.Tests.Mocks;
using SkjaTextar.Controllers;
using System.Web.Mvc;
using SkjaTextar.ViewModels;


namespace SkjaTextar.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestIndexWithMoreThan5()
        {


            var mockUnitOfWork = new MockUnitOfWork();
            for (int i = 1; i < 8; i++)
            {
                mockUnitOfWork.TranslationRepository.Insert(new Translation
                {
                    ID = i,
                    MediaID = i,
                    Score = i,
                    NumberOfDownloads = i,
                    Locked = false,
                    LanguageID = 2,
                });

            }

            var controller = new HomeController(mockUnitOfWork);

            //Arrange
            //Act
            var result = controller.Index();
            //Assert
            var viewresult = (ViewResult)result;
            HomeViewModel model = viewresult.Model as HomeViewModel;
            Assert.IsTrue(model.TopTranslations.Count == 5);
            for (int i = 0; i < model.TopTranslations.Count - 1; i++)
            {
                Assert.IsTrue(model.TopTranslations[i].NumberOfDownloads >= model.TopTranslations[i + 1].NumberOfDownloads);
            }

            /*for ( int i = 0; i < model.TopTranslations.Count - 1; i++)
            {
                Assert.IsTrue(model.)
            }*/
        }
        [TestMethod]
        public void TestIndexWithLessThan5()
        {


            var mockUnitOfWork = new MockUnitOfWork();
            for (int i = 1; i < 3; i++)
            {
                mockUnitOfWork.TranslationRepository.Insert(new Translation
                {
                    ID = i,
                    MediaID = i,
                    Score = i,
                    NumberOfDownloads = i,
                    Locked = false,
                    LanguageID = 2,
                });

            }

            var controller = new HomeController(mockUnitOfWork);

            //Arrange
            //Act
            var result = controller.Index();
            //Assert
            var viewresult = (ViewResult)result;
            HomeViewModel model = viewresult.Model as HomeViewModel;
            Assert.IsTrue(model.TopTranslations.Count == 2);
            for (int i = 0; i < model.TopTranslations.Count - 1; i++)
            {
                Assert.IsTrue(model.TopTranslations[i].NumberOfDownloads >= model.TopTranslations[i + 1].NumberOfDownloads);
            }

        }
    }
}
