using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyMate.Data;
using StudyMate.Models;
using StudyMate.Services;

namespace StudyMate.Controllers
{
    public class QandAsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
		private readonly IImageService _imageService;
		private readonly IFileWriter _writer;

        public QandAsController(ApplicationDbContext context, IImageService imageService, IFileWriter writer, IHostingEnvironment env)
        {
            _context = context;
			_imageService = imageService;
			_writer = writer;
			_env = env;

		}

        // GET: QandAs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QandA.Include(q => q.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QandAs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qandA = await _context.QandA
                .Include(q => q.Course)
                .SingleOrDefaultAsync(m => m.QandAID == id);
            if (qandA == null)
            {
                return NotFound();
            }

            return View(qandA);
        }


		public IActionResult CreateAsSingle()
		{
			ViewData["CourseID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseName");
			return View();
		}

		public IActionResult CreateAsGroup()
		{
			ViewData["CourseID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseName");
			return View();
		}

		// POST: QandAs/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAsSingle([Bind("QandAID,Question,OptionA,OptionB,OptionC,CorrectAnswer,Explanation,OptionD,Voice,Image,CourseID,TopicTitle")] FromFileViewModel file)
		{
			QandA qandA = new QandA();
			if (ModelState.IsValid)
			{
				var voiceOver = _imageService.CreateImage(file.Voice, "VoiceOver", _env);
				if (!string.IsNullOrEmpty(voiceOver))
				{
					qandA.VoiceOver = voiceOver;
				}

				var imageSource = _imageService.CreateImage(file.Image, "images", _env);
				if (!string.IsNullOrEmpty(imageSource))
				{
					qandA.ImageUrl = imageSource;
				}
				qandA.CorrectAnswer = file.CorrectAnswer;
				qandA.CourseID = file.CourseID;
				qandA.Question = file.Question;
				qandA.OptionA = file.OptionA;
				qandA.OptionB = file.OptionB;
				qandA.OptionC = file.OptionC;
				qandA.OptionD = file.OptionD;
				qandA.QandAID = file.QandAID;
				qandA.Explanation = file.Explanation;
				qandA.TopicTitle = file.TopicTitle;
				_context.Add(qandA);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["CoursesID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseName", qandA.CourseID);
			return View(qandA);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAsGroup([Bind("QandAID,CourseID,UploadedFile")] FromFileViewModel file)
		{
			if (ModelState.IsValid)
			{
				var multipleQuestions = _writer.QandAs(file.UploadedFile, file.CourseID);
				if (multipleQuestions != null)
				{
					_context.AddRange(multipleQuestions);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			ViewData["CoursesID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseName", file.CourseID);
			return View(file);
		}

		//GET: QandAs/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var qandA = await _context.QandA.SingleOrDefaultAsync(m => m.QandAID == id);
			if (qandA == null)
			{
				return NotFound();
			}
			FromFileViewModel file = new FromFileViewModel
			{
				CorrectAnswer = qandA.CorrectAnswer,
				CourseID = qandA.CourseID,
				ImageUrl = qandA.ImageUrl,
				OptionA = qandA.OptionA,
				OptionB = qandA.OptionB,
				OptionC = qandA.OptionC,
				OptionD = qandA.OptionD,
				QandAID = qandA.QandAID,
				VoiceOver = qandA.VoiceOver,
				Question = qandA.Question,
				Explanation = qandA.Explanation,
				TopicTitle = qandA.TopicTitle,
			};

			ViewData["CourseID"] = new SelectList(_context.Course, "CourseID", "CourseName", qandA.CourseID);
			return View(file);
			
		}

		// POST: QandAs/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("QandAID,Question,OptionA,OptionB,OptionC,Explanation,CorrectAnswer,OptionD,ImageUrl,VoiceOver,TopicTitle,Voice,Image,CourseID")] FromFileViewModel file)
		{
			QandA qandA = new QandA();
			if (id != file.QandAID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					if (file.Voice != null)
					{
						string voiceOver = _imageService.EditImage(file.Voice, file.VoiceOver, "VoiceOver", _env);
						if (!string.IsNullOrEmpty(voiceOver))
						{
							qandA.VoiceOver = voiceOver;
						}
					}
					else {
						qandA.VoiceOver = file.VoiceOver;
					}
					if (file.Image != null)
					{
						string img = _imageService.EditImage(file.Image, file.ImageUrl, "images", _env);
						if (!string.IsNullOrEmpty(img))
						{
							qandA.ImageUrl = img;
						}
					}
					else
					{
						qandA.ImageUrl = file.ImageUrl;
					}
					qandA.CorrectAnswer = file.CorrectAnswer;
					qandA.CourseID = file.CourseID;
					qandA.Question = file.Question;
					qandA.OptionA = file.OptionA;
					qandA.OptionB = file.OptionB;
					qandA.OptionC = file.OptionC;
					qandA.OptionD = file.OptionD;
					qandA.QandAID = file.QandAID;
					qandA.TopicTitle = file.TopicTitle;
					qandA.Explanation = file.Explanation;
					_context.Update(qandA);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!QandAExists(qandA.QandAID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["CoursesID"] = new SelectList(_context.Course, "CourseID", "CourseName", qandA.CourseID);
			return View(qandA);
		}

		// GET: QandAs/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qandA = await _context.QandA
                .Include(q => q.Course)
                .SingleOrDefaultAsync(m => m.QandAID == id);
            if (qandA == null)
            {
                return NotFound();
            }

            return View(qandA);
        }

        // POST: QandAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qandA = await _context.QandA.SingleOrDefaultAsync(m => m.QandAID == id);
			if (qandA != null)
			{
				if (qandA.VoiceOver != null && qandA.ImageUrl != null)
				{
					Parallel.Invoke(() => _imageService.DeleteImage(qandA.VoiceOver, _env),
									() => _imageService.DeleteImage(qandA.ImageUrl, _env));
				}
				else if (qandA.VoiceOver != null)
				{
					_imageService.DeleteImage(qandA.VoiceOver, _env);
				}
				else if (qandA.ImageUrl != null)
				{
					_imageService.DeleteImage(qandA.ImageUrl, _env);
				}
			}
            _context.QandA.Remove(qandA);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QandAExists(int id)
        {
            return _context.QandA.Any(e => e.QandAID == id);
        }
    }
	
}
