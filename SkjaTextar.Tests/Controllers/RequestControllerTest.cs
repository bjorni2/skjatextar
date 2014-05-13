using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkjaTextar.Controllers;
using SkjaTextar.Models;
using SkjaTextar.Tests.Mocks;
using SkjaTextar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SkjaTextar.Tests.Controllers
{
	[TestClass]
	public class RequestControllerTest
	{
		[TestMethod]
		public void TestIndexScore()
		{
			//Arrange
			var mockUnitOfWork = new MockUnitOfWork();
			mockUnitOfWork.MediaRepository.Insert(new Movie
				{
					ID = 1,
					Title = "Test",
					CategoryID = 1,
					ReleaseYear = 1998,
				});
			
			var movie = new Movie
				{
					ID = 1,
					Title = "Test",
					CategoryID = 1,
					ReleaseYear = 1998,
				};

			for (int i = 1; i < 8; i++)
			{
				mockUnitOfWork.RequestRepository.Insert(new Request
				{
					ID = i,
					MediaID = 1,
					LanguageID = i,
					Score = i,
					Media = movie,
				});
			}

			var controller = new RequestController(mockUnitOfWork);

			//Act
			var result = controller.Index("Score");
			//Assert
			var viewresult = (ViewResult)result;
			List<RequestVoteViewModel> model = viewresult.Model as List<RequestVoteViewModel>;
			Assert.IsTrue(model.Count() == 7);
			for (int i = 0; i < model.Count() - 1; i++)
			{
				Assert.IsTrue(model[i].Request.Score <= model[i + 1].Request.Score);
			}
		}

		[TestMethod]
		public void TestCreateForMedia()
		{

		}

		/*[TestMethod]
		public void TestDetailsMovie()
		{
			//Arrange
			var mockUnitOfWork = new MockUnitOfWork();
			mockUnitOfWork.MediaRepository.Insert(new Movie
			{
				ID = 1,
				Title = "Test",
				CategoryID = 1,
				ReleaseYear = 1998,
			});

			var movie = new Movie
			{
				ID = 1,
				Title = "Test",
				CategoryID = 1,
				ReleaseYear = 1998,
			};

			mockUnitOfWork.RequestRepository.Insert(new Request
			{
				ID = 1,
				MediaID = 1,
				LanguageID = 1,
				Score = 1,
				Media = movie,
			});

			var controller = new RequestController(mockUnitOfWork);

			//Act
			var result = controller.Details(1);
			//Assert
			var viewresult = (ViewResult)result;
			Request model = viewresult.Model as Request;
			Assert.IsTrue(model.MediaID == 1);
		}*/
	}
}
