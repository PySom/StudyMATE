using Microsoft.AspNetCore.Http;
using StudyMate.Models;
using System.Collections.Generic;

namespace StudyMate.Services
{
	public interface IFileWriter
	{
		ICollection<QandA> QandAs(IFormFile file, int id);
	}
}
