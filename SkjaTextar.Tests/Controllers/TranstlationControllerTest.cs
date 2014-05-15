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
using SkjaTextar.Exceptions;

namespace SkjaTextar.Tests.Controllers
{
    [TestClass]
    public class TranstlationControllerTest
    {

        [TestMethod]
        // here we make a dummy translation with 53 
        // segments max 50 segments are displayed at
        // a time on the site( 50 on every page)
        // the second page should include 3 segments
        public void TestTranslationIndex()
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
        [TestMethod]
        // In this simple test we check if the number 
        // of the segments in the database is incremented
        // when we add try to add a line

        public void TestAddLine()
        {
            //Arrange
            var segment = new SegmentViewModel
            {
                Line1 = "Dummy",
                Line2 = "Dummy",
                Original1 = "Dummy",
                Original2 = "Dummy",
                TranslationID = 1,
                TimestampEnd = "00:06:30,000",
                TimestampStart = "00:06:10,000",
            };
            var mockUnitOfWork = new MockUnitOfWork();
            for ( int i = 0; i < 3; i++ )
            {
                var reposegment = new TranslationSegment();
                mockUnitOfWork.TranslationSegmentRepository.Insert(reposegment);
            };

            
            var controller = new TranslationController(mockUnitOfWork);
            int countbefore = mockUnitOfWork.TranslationSegmentRepository.Get().ToList().Count;
            // Act
            var result = controller.AddLine(segment);
            // Assert
            Assert.IsTrue(mockUnitOfWork.TranslationSegmentRepository.Get().ToList().Count == countbefore + 1);
        }
        [TestMethod]
        // A test to see if the right Exeptions are thrown
        // in the CreateTranslation method of the Translation controller.
        public void TestCreateTranslationThrowsErrors()
        {
            //Arrange
            var mockUnitOfWork = new MockUnitOfWork();
            var controller = new TranslationController(mockUnitOfWork);
            // Act
            // The DataNotFoundExeption should be thrown
            // if the first parameter does not match any id 
            // in the database.
            try
            {
                var result = controller.CreateTranslation(1, null);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(DataNotFoundException));
            }
            // Act
            // The MissingParameterExeption should be thrown
            // if the method takes null as the first parameter.
            try
            {
                var result = controller.CreateTranslation(null, null);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }

        }
        [TestMethod]
        // A test to see if the right Exeptions are thrown
        // in the CommentIndex method of the Translation controller.
        public void TestCommentIndexTrowsErrors()
        {
            var mockUnitOfWork = new MockUnitOfWork();
            var controller = new TranslationController(mockUnitOfWork);
            // Act
            // The DataNotFoundExeption should be thrown
            // if the parameter does not match any id 
            // in the database.
            try
            {
                var result = controller.CommentIndex(8);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(DataNotFoundException));
            }
            // Act
            // The MissingParameterExeption should be thrown
            // if the method takes null as a parameter.
            try
            {
                int? variable = null;
                var result = controller.CommentIndex(variable);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }
        }    
    }
}
