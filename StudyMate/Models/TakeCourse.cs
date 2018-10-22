using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMate.Models
{
	public class TakeCourse
	{
		public ApplicationUser ApplicationUser { get; set; }
		public int ApplicationUserID { get; set; }

		public Course Courses { get; set; }
		public int CoursesID { get; set; }
	}
}
