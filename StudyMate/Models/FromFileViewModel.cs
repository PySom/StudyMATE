using Microsoft.AspNetCore.Http;


namespace StudyMate.Models
{
	public class FromFileViewModel : QandA
	{
		public IFormFile UploadedFile { get; set; }
		public IFormFile Image { get; set; }
		public IFormFile Voice { get; set; }
	}
}
