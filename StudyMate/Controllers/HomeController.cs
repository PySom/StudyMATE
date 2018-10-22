using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudyMate.Data;
using StudyMate.Models;

namespace StudyMate.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;
		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var context = _context.Course.ToList();
			return View(context);
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
