using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkjaTextar.Controllers;
using SkjaTextar.Exceptions;
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
		public void TestHomeCreate()
		{
			//Arrange
			var mockUnitOfWork = new MockUnitOfWork();
			var controller = new RequestController(mockUnitOfWork);

			//Act and Assert
			var result = controller.Create("Movie");
			var viewresult = (ViewResult)result;
			var model = viewresult.Model;
			Assert.IsInstanceOfType(model, typeof(MovieRequestViewModel));

			var result2 = controller.Create("Show");
			var viewresult2 = (ViewResult)result2;
			var model2 = viewresult2.Model;
			Assert.IsInstanceOfType(model2, typeof(ShowRequestViewModel));

			var result3 = controller.Create("Clip");
			var viewresult3 = (ViewResult)result3;
			var model3 = viewresult3.Model;
			Assert.IsInstanceOfType(model3, typeof(ClipRequestViewModel));
		}
		
		[TestMethod]
		public void HomeCreate()
		{
			var mockUnitOfWork = new MockUnitOfWork();
			var media = new Media
			{
				ID = 1,
			};
			var controller = new RequestController(mockUnitOfWork);

			//Act
			try
			{
					var result = controller.Details(null);
			}
			catch(Exception ex)
			{
				Assert.IsInstanceOfType(ex, typeof(MissingParameterException));
			}

			try
			{
					var result2 = controller.Details(0);
			}
			catch(Exception ex)
			{
				Assert.IsInstanceOfType(ex, typeof(DataNotFoundException));
			}

			try
			{
				var result3 = controller.Details(1);
			}
			catch(Exception ex)
			{
				Assert.IsInstanceOfType(ex, typeof(ApplicationException));
			}
		}
	}
}
