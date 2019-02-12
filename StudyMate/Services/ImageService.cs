using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace StudyMate.Services{
    public class ImageService : IImageService
	{
		public static Random random = new Random(55);
		public ImageService(){}
		public string CreateImage(IFormFile file, string fileDefault, IHostingEnvironment _env)
		{
			string relativePath = "";
			if (file != null)
			{
				
					int randomId = ImageService.random.Next(56, 1000);
					var fileName = $"{randomId}{Path.GetFileName(file.FileName)}";
					relativePath = Path.Combine(fileDefault, fileName);
					var absolutePath = Path.Combine(_env.WebRootPath, relativePath);

					using (FileStream stream = new FileStream(absolutePath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
	
				return relativePath;
			}
			return null;
		}
		

		public string EditImage(IFormFile file, string imageUrl, string fileDefault, IHostingEnvironment env)
		{
			if(file!=null)
			{
				string relativePath = "";
				if (!string.IsNullOrEmpty(imageUrl))
				{
					var oldPath = Path.Combine(env.WebRootPath, imageUrl);
					if (System.IO.File.Exists(oldPath))
					{
						System.IO.File.Delete(oldPath);
					}
				}
				int randomId = random.Next(56, 1000);
				var fileName = $"{randomId}{Path.GetFileName(file.FileName)}";
				relativePath = Path.Combine(fileDefault, fileName);
				var absolutePath = Path.Combine(env.WebRootPath, relativePath);

				using (FileStream stream = new FileStream(absolutePath, FileMode.Create))
				{
					file.CopyTo(stream);
				}

				return relativePath;

			}
			return null;
		}

		
		//Delete a single image
		public bool DeleteImage(string ImageUrl, IHostingEnvironment _env)
		{
			if (string.IsNullOrEmpty(ImageUrl))
			{
				ImageUrl = "";
			}
			var oldPath = Path.Combine(_env.WebRootPath, ImageUrl);
			if (System.IO.File.Exists(oldPath))
			{
				System.IO.File.Delete(oldPath);
				return true;
			}
			return false;
		}       
    }
}