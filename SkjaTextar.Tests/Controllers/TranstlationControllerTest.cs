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
    public class TranstlationControllerTest
    {

        [TestMethod]
        public void TestIndexWithMoreThan5()
        {

            //Arrange
            var translation = new Translation
                {   
                    ID = 1,
                    MediaID = 2,  
                    LanguageID = 3,
                };
           
            for (int i = 1; i < 54; i++ )
            {
                var segment = new TranslationSegment
                {
                    SegmentID = i,
                    Translation = translation
                };
            }
            var mockUnitOfWork = new MockUnitOfWork();

            mockUnitOfWork.TranslationRepository.Insert(translation);

            // Act
            var controller = new TranslationController(mockUnitOfWork);




        }
    }
}
