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
        [TestMethod]
        public void TestSearch()
        {
            var mockUnitOfWork = new MockUnitOfWork();
            for (int i = 1; i < 4; i++)
            mockUnitOfWork.MovieRepository.Insert(new Movie
            {
                ID = i,
                Title = "the cat" + i.ToString(),
            });
            for (int i = 4; i < 7; i++)
                mockUnitOfWork.ShowRepository.Insert(new Show
                {
                    ID = i,
                    Title = "the horse" + i.ToString(),
                });
            for (int i = 7; i < 10; i++)
                mockUnitOfWork.ClipRepository.Insert(new Clip
                {
                    ID = i,
                    Title = "the hero" + i.ToString(),
                });
            
            var controller = new HomeController(mockUnitOfWork);

            // this search should return 3 clips 3 shows and no movies
            var resultWithThe_h = controller.Search("the h");
            var viewresult1 = (ViewResult)resultWithThe_h;
            SearchMediaViewModel model1 = viewresult1.Model as SearchMediaViewModel;
            Assert.IsTrue(model1.Clips.Count == 3 && model1.Shows.Count == 3 && model1.Movies.Count == 0);

            // this search should only return 3 movies
            var resultWithThe_c = controller.Search("the c");
            var viewresult2 = (ViewResult)resultWithThe_c;
            SearchMediaViewModel model2 = viewresult2.Model as SearchMediaViewModel;
            Assert.IsTrue(model2.Movies.Count == 3 && model2.Shows.Count == 0 && model2.Clips.Count == 0);

            // this search should return no results
            var resultWithG = controller.Search("G");
            var viewresult3 = (ViewResult)resultWithG;
            SearchMediaViewModel model3 = viewresult1.Model as SearchMediaViewModel;
            Assert.IsTrue(model2.Movies.Count == 0 && model2.Shows.Count == 0 && model2.Clips.Count == 0);
            
            // this should also return no results
            var resultWithNull = controller.Search("");
            var viewresult4 = (ViewResult)resultWithNull;
            SearchMediaViewModel model4 = viewresult1.Model as SearchMediaViewModel;
            Assert.IsTrue(model2.Movies.Count == 0 && model2.Shows.Count == 0 && model2.Clips.Count == 0);

        }
    }
}
