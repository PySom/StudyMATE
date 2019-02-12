using System;
namespace StudyMate.Models
{
	public class CourseTaken
	{
		public int CourseTakenID { get; set; }
		public double Score { get; set; }
		public DateTime DateSubmitted { get; set; }

		public ApplicationUser ApplicationUser { get; set; }
		public string ApplicationUserID { get; set; }

		public Course Course { get; set; }
		public int CourseID { get; set; }
	}
}
