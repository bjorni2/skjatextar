using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SkjaTextar.Models;
using SkjaTextar.Tests.Mocks;
using SkjaTextar.Controllers;
using System.Web.Mvc;
using SkjaTextar.ViewModels;
using SkjaTextar.Helpers;


namespace SkjaTextar.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        //Arrange
        [TestMethod]
        public void TestHomeIndexWithMoreThan5()
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
                    Media = new Movie(),
                });

            }

            var controller = new HomeController(mockUnitOfWork);

            
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
        }
        [TestMethod]
        public void TestHomeIndexWithLessThan5()
        {
            //Arrange
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
                    Media = new Movie(),
                });

            }

            var controller = new HomeController(mockUnitOfWork);

            
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
            // Arrange
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
            // Act
            var controller = new HomeController(mockUnitOfWork);

            // Assert
            // this search should return 3 clips 3 shows and no movies
            var resultWithThe_h = controller.Search("the h");
            var viewresult1 = (ViewResult)resultWithThe_h;
            SearchMediaViewModel model1 = viewresult1.Model as SearchMediaViewModel;
            Assert.IsTrue(model1.Clips.Count == 3 && model1.Shows.Count == 3 && model1.Movies.Count == 0);
            for (int i = 0; i < model1.Clips.Count - 1; i++ )
            {
                Assert.IsTrue(string.Compare(model1.Clips[i].Title, model1.Clips[i + 1].Title) < 0);
            }
            for (int i = 0; i < model1.Shows.Count - 1; i++)
            {
                Assert.IsTrue(string.Compare(model1.Shows[i].Title, model1.Shows[i + 1].Title) < 0);
            }
                

            // this search should only return 3 movies
            var resultWithThe_c = controller.Search("the c");
            var viewresult2 = (ViewResult)resultWithThe_c;
            SearchMediaViewModel model2 = viewresult2.Model as SearchMediaViewModel;
            Assert.IsTrue(model2.Movies.Count == 3 && model2.Shows.Count == 0 && model2.Clips.Count == 0);
            for (int i = 0; i < model1.Movies.Count - 1; i++)
            {
                Assert.IsTrue(string.Compare(model1.Movies[i].Title, model1.Movies[i + 1].Title) < 0);
            }
            // this search should return no results
            var resultWithG = controller.Search("G");
            var viewresult3 = (ViewResult)resultWithG;
            SearchMediaViewModel model3 = viewresult3.Model as SearchMediaViewModel;
            Assert.IsTrue(model3.Movies.Count == 0 && model3.Shows.Count == 0 && model3.Clips.Count == 0);
            
            // this should also return no results
            var resultWithNull = controller.Search("");
            var viewresult4 = (ViewResult)resultWithNull;
            SearchMediaViewModel model4 = viewresult4.Model as SearchMediaViewModel;
            Assert.IsTrue(model4.Movies.Count == 0 && model4.Shows.Count == 0 && model4.Clips.Count == 0);


        }
        [TestMethod]
        public void TestAutocomplete()
        {
            //Arrange
            var mockUnitOfWork = new MockUnitOfWork();
            for (int i = 1; i < 4; i++)
                mockUnitOfWork.MediaRepository.Insert(new Movie
                {
                    ID = i,
                    Title = "the cat" + i.ToString(),
                });
            for (int i = 4; i < 7; i++)
                mockUnitOfWork.MediaRepository.Insert(new Show
                {
                    ID = i,
                    Title = "the horse" + i.ToString(),
                });
            for (int i = 7; i < 10; i++)
                mockUnitOfWork.MediaRepository.Insert(new Clip
                {
                    ID = i,
                    Title = "the hero" + i.ToString(),
                });

            var controller = new HomeController(mockUnitOfWork);

            // Act
            var resultWithThe_h = controller.Autocomplete("the h");
            var resultWithThe_c = controller.Autocomplete("the c");
            var resultWithThe = controller.Autocomplete("the");
            var resultWithThe_f = controller.Autocomplete("the f");
            // Asset
            // the Autocomplete method is only used when two or more
            // letters have been typed in the search box so it is
            // need less with one letter or the empty string

            // this search should return 6 results

            var jsonresult1 = (JsonResult)resultWithThe_h;
            List<SearchResult> model1 = jsonresult1.Data as List<SearchResult>;
            Assert.IsTrue(model1.Count == 6);

            // this search should only return 3 results
            var jsonresult2 = (JsonResult)resultWithThe_c;
            List<SearchResult> model2 = jsonresult2.Data as List<SearchResult>;
            Assert.IsTrue(model2.Count == 3);

            // this search should return 9 reults. Since we have
            // that many results it is convienient also to test 
            // the alphabetical order of the Titles.

            var viewresult3 = (JsonResult)resultWithThe;
            List<SearchResult> model3 = viewresult3.Data as List<SearchResult>;
            Assert.IsTrue(model3.Count == 9);

            for (int i = 0; i < model1.Count - 1; i++)
            {
                Assert.IsTrue(string.Compare(model1[i].label, model1[i + 1].label) < 0);
            }

            // this search should return no results

            var viewresult4 = (JsonResult)resultWithThe_f;
            List<SearchResult> model4 = viewresult4.Data as List<SearchResult>;
            Assert.IsTrue(model4.Count == 0);
        }
    }
}
