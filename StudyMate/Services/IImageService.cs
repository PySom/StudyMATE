using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace StudyMate.Services
{
    public interface IImageService
    {
        
    	string CreateImage(IFormFile file, string fileDefault, IHostingEnvironment _env);
		string EditImage(IFormFile file, string imageUrl, string fileDefault, IHostingEnvironment env);
		bool DeleteImage(string ImageUrl, IHostingEnvironment _env);
		
	}
}