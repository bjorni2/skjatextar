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
        public void TestCreateTranslationThrowsErrors()
        {
            //Arrange
            var mockUnitOfWork = new MockUnitOfWork();
            var controller = new TranslationController(mockUnitOfWork);
            // Act
            try
            {
                var result = controller.CreateTranslation(1, null);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(DataNotFoundException));
            }
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
        public void TestCommentIndexTrowsErrors()
        {

            var mockUnitOfWork = new MockUnitOfWork();
            var controller = new TranslationController(mockUnitOfWork);
            // Act
            try
            {
                var result = controller.CommentIndex(8);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(DataNotFoundException));
            }
            try
            {
                int? variable = null;
                var result = controller.CommentIndex(variable);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
            }
        }    
    }
}
