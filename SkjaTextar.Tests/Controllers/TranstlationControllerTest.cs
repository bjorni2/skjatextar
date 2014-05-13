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
using PagedList;

namespace SkjaTextar.Tests.Controllers
{
    [TestClass]
    public class TranstlationControllerTest
    {

        [TestMethod]
        public void TestIndexForTranslation()
        {

            //Arrange
            var translation = new Translation
                {   
                    ID = 1,
                    MediaID = 2,  
                    LanguageID = 3,
                    Media = new Media{ Title = "Dummy"},
                    Language = new Language{ Name = "Dummy2"},
                    TranslationSegments = new List<TranslationSegment>()
                };
           
            for (int i = 1; i < 54; i++ )
            {
                var segment = new TranslationSegment
                {
                    SegmentID = i,
                    Translation = translation
                };
                translation.TranslationSegments.Add(segment);
            }
            var mockUnitOfWork = new MockUnitOfWork();
            mockUnitOfWork.TranslationRepository.Insert(translation);
            var controller = new TranslationController(mockUnitOfWork);

            //Act
            var result = controller.Index(1, 2);

            // Assert
            var viewResult = (ViewResult)result;
            PagedList<TranslationSegment> model = viewResult.Model as PagedList<TranslationSegment>;
            Assert.IsTrue(model.Count == 3);

        }
    }
}
