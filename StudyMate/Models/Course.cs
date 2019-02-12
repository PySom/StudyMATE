using System.Collections.Generic;

namespace StudyMate.Models
{
	public class Course
	{
		public int CourseID { get; set; }
		public string CourseName { get; set; }
		public ICollection<QandA> QandAs { get; set; }

	}
}
