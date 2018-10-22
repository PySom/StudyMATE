using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMate.Models
{
	public class QandA
	{
		public int QandAID { get; set; }
		public string Question { get; set; }
		public string OptionA { get; set; }
		public string OptionB { get; set; }
		public string OptionC { get; set; }
		public string OptionD { get; set; }
		public string ImageUrl { get; set; }
		public string CorrectAnswer { get; set; }

		public string VoiceOver { get; set; }

		public Course Courses { get; set; }
		public int CoursesID { get; set; }
	}
}
