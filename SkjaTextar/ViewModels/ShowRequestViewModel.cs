using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkjaTextar.Models;

namespace SkjaTextar.ViewModels
{
	public class ShowRequestViewModel
	{
		public Show Show { get; set; }

		public Request Request { get; set; }
	}
}