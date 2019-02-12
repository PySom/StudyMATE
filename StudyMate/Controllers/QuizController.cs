using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyMate.Data;
using StudyMate.Models;

namespace StudyMate.Controllers
{
	public class QuizController : Controller
	{
		private readonly ApplicationDbContext _context;

		public QuizController(ApplicationDbContext context)
		{
			_context = context;
		}

		//GetQuizByID Quiz/{{id}}
		public IActionResult QuizCourseByID(int id)
		{
			ViewData["ID"] = id;
			return View();
		}

		//GetQuizByID Quiz/{{id}}
		[HttpGet]
		public async Task<IActionResult> ReturnQuizJson(int id)
		{
			Course course = await _context.Course.Include(c => c.QandAs).Select(
				value => new Course {
					CourseID = value.CourseID,
					CourseName = value.CourseName,
					QandAs = value.QandAs.Select(
						qA => new QandA {
							QandAID = qA.QandAID,
							Question = qA.Question,
							CorrectAnswer = qA.CorrectAnswer,
							OptionA = qA.OptionA,
							OptionB = qA.OptionB,
							OptionC = qA.OptionC,
							OptionD = qA.OptionD,
							TopicTitle = qA.TopicTitle,
							ImageUrl = qA.ImageUrl,
							VoiceOver = qA.VoiceOver,
							Explanation = qA.Explanation
						}
					).ToList()
							
			}).SingleOrDefaultAsync(c => c.CourseID == id);
			return Json(course);
		}
	}
}