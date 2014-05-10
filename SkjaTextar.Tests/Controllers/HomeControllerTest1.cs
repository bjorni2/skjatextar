using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SkjaTextar.Models;
using SkjaTextar.Tests.Mocks;
using SkjaTextar.Controllers;
using System.Web.Mvc;

namespace SkjaTextar.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest1
    {
        [TestMethod]
        public void TestIndexWithMoreThan5Translations()
        {

            List<Translation> translations = new List<Translation>();
            for (int i = 0; i < 8; i++)
            {
                translations.Add(new Translation
                {
                    ID = i,
                    MediaID = i,
                    Score = i,
                    NumberOfDownloads = i,
                    Locked = false,
                    LanguageID = 2,
                    /*Media = 
                    Language 
                     TranslationSegments 
                     Comments 
                    Reports 
                    TranslationVotes */
                });
            }
            Mocks.MockTranslationRepository mockrepo = new Mocks.MockTranslationRepository(translations);
            var controller = new HomeController(mockrepo);
            
            //Arrange
            //Act
            var result = controller.Index();
            //Assert
            var viewresult = (ViewResult)result;
            var model = viewresult.Model;
            
        }
    }
}
