using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyMate.Data;
using StudyMate.Models;

namespace StudyMate.Controllers
{
    public class QandAsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;

        public QandAsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
			_env = environment;
        }

        // GET: QandAs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QandA.Include(q => q.Courses);
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
                .Include(q => q.Courses)
                .SingleOrDefaultAsync(m => m.QandAID == id);
            if (qandA == null)
            {
                return NotFound();
            }

            return View(qandA);
        }

		// GET: QandAs/Create
		public IActionResult CreateAsSingle()
		{
			ViewData["CoursesID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseName");
			return View();
		}

		public IActionResult CreateAsGroup()
		{
			ViewData["CoursesID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseName");
			return View();
		}

		// POST: QandAs/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAsSingle([Bind("QandAID,Question,OptionA,OptionB,OptionC,CorrectAnswer,OptionD,Voice,Image,CoursesID")] FromFileViewModel file)
		{
			QandA qandA = new QandA();
			if (ModelState.IsValid)
			{
				var voiceOver = UploadSound(file.Voice);
				if (!string.IsNullOrEmpty(voiceOver))
				{
					qandA.VoiceOver = voiceOver;
				}

				var imageSource = UploadImage(file.Image);
				if (!string.IsNullOrEmpty(imageSource))
				{
					qandA.ImageUrl = imageSource;
				}
				qandA.CorrectAnswer = file.CorrectAnswer;
				qandA.CoursesID = file.CoursesID;
				qandA.Question = file.Question;
				qandA.OptionA = file.OptionA;
				qandA.OptionB = file.OptionB;
				qandA.OptionC = file.OptionC;
				qandA.OptionD = file.OptionD;
				qandA.QandAID = file.QandAID;
				_context.Add(qandA);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["CoursesID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseID", qandA.CoursesID);
			return View(qandA);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAsGroup([Bind("QandAID,CoursesID,UploadedFile,Voice,Image")] FromFileViewModel file)
		{
			if (ModelState.IsValid)
			{
				var multipleQuestions = WriteTextFile(file);
						
				

				if (multipleQuestions != null)
				{
					_context.AddRange(multipleQuestions);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			ViewData["CoursesID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseID", file.CoursesID);
			return View(file);
		}

		// GET: QandAs/Edit/5
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
			FromFileViewModel file = new FromFileViewModel { CorrectAnswer = qandA.CorrectAnswer,
															 CoursesID = qandA.CoursesID,
															 ImageUrl = qandA.ImageUrl,
															 OptionA = qandA.OptionA,
															 OptionB = qandA.OptionB,
															 OptionC = qandA.OptionC,
															 OptionD = qandA.OptionD,
															 QandAID = qandA.QandAID,
															 VoiceOver = qandA.VoiceOver,
															 Question = qandA.Question};

            ViewData["CoursesID"] = new SelectList(_context.Course, "CourseID", "CourseID", qandA.CoursesID);
            return View(file);
        }

        // POST: QandAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QandAID,Question,OptionA,OptionB,OptionC,CorrectAnswer,OptionD,Voice,Image,CoursesID")] FromFileViewModel file)
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
					var voiceOver = UploadSound(file.Voice);
					if (!string.IsNullOrEmpty(voiceOver))
					{
						qandA.VoiceOver = voiceOver;
					}

					var imageSource = UploadImage(file.Image);
					if (!string.IsNullOrEmpty(imageSource))
					{
						qandA.ImageUrl = imageSource;
					}
					qandA.CorrectAnswer = file.CorrectAnswer;
					qandA.CoursesID = file.CoursesID;
					qandA.Question = file.Question;
					qandA.OptionA = file.OptionA;
					qandA.OptionB = file.OptionB;
					qandA.OptionC = file.OptionC;
					qandA.OptionD = file.OptionD;
					qandA.QandAID = file.QandAID;
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
            ViewData["CoursesID"] = new SelectList(_context.Course, "CourseID", "CourseID", qandA.CoursesID);
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
                .Include(q => q.Courses)
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
            _context.QandA.Remove(qandA);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QandAExists(int id)
        {
            return _context.QandA.Any(e => e.QandAID == id);
        }

		private List<QandA> WriteTextFile(FromFileViewModel file)
		{
			string message = "";
			string correctAnswer = "";
			string quest = "";
			string opt1 = "";
			string opt2 = "";
			string opt3 = "";
			string opt4 = "";
			int question = 0;
			int option1 = 0;
			int option2 = 0;
			int option3 = 0;
			int option4 = 0;

			List<QandA> multipleQuestions = new List<QandA>();
			Stream reader = file.UploadedFile.OpenReadStream();
			if (reader != null)
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(reader))
					{

						foreach (var item in System.IO.File.ReadLines(file.UploadedFile.FileName))
						{
							if (item.EndsWith('#') && question == 0)
							{

								quest += item.Remove(item.Length - 1, 1);
								question = 1;
							}



							else if (item.EndsWith(',') && option1 == 0)
							{

								opt1 += item.Remove(item.Length - 1, 1);
								if (item.StartsWith('/'))
								{
									opt1 = opt1.Remove(0, 1);
									correctAnswer = opt1;
								}
								option1 = 1;
							}


							else if (item.EndsWith(',') && option2 == 0)
							{

								opt2 += item.Remove(item.Length - 1, 1);
								if (item.StartsWith('/'))
								{
									opt2 = opt2.Remove(0, 1);
									correctAnswer = opt2;
								}
								option2 = 1;

							}

							else if (item.EndsWith(',') && option3 == 0)
							{

								opt3 += item.Remove(item.Length - 1, 1);
								if (item.StartsWith('/'))
								{
									opt3 = opt3.Remove(0, 1);
									correctAnswer = opt3;
								}
								option3 = 1;

							}


							else if (item.EndsWith('$') && option4 == 0)
							{

								opt4 += item.Remove(item.Length - 1, 1);
								if (item.StartsWith('/'))
								{
									opt4 = opt4.Remove(0, 1);
									correctAnswer = opt4;
								}
								QandA qa = new QandA() { Question = quest, OptionA = opt1, OptionB = opt2, OptionC = opt3, OptionD = opt4, CoursesID = file.CoursesID, CorrectAnswer = correctAnswer };
								quest = "";
								opt1 = "";
								opt2 = "";
								opt3 = "";
								opt4 = "";
								question = 0;
								option1 = 0;
								option2 = 0;
								option3 = 0;
								multipleQuestions.Add(qa);
							}
							

						}
					}

				}
				catch (Exception e)
				{
					message = e.Message;
					
				}
				finally
				{
					reader.Close();

				}
			}
			if (string.IsNullOrEmpty(message))
			{
				return multipleQuestions;
			}
			else return null;
		}

		private string UploadSound(IFormFile file)
		{
			var soundObject = file;
			if (soundObject != null)
			{
				var soundObjectName = $"{Path.GetFileName(soundObject.FileName)}";
				string soundSrc = Path.Combine("VoiceOver", soundObjectName);
				string abs = Path.Combine(_env.WebRootPath, soundSrc);
				using (FileStream stream = new FileStream(abs, FileMode.Create))
				{
					soundObject.CopyTo(stream);
				}
				return soundSrc;
			}
			return null;
		}

		private string UploadImage(IFormFile file)
		{
			var fileObject = file;
			if (fileObject != null)
			{
				var fileName = $"{Path.GetFileName(fileObject.FileName)}";
				string src = Path.Combine("images", fileName);
				string abs = Path.Combine(_env.WebRootPath, src);
				using (FileStream stream = new FileStream(abs, FileMode.Create))
				{
					fileObject.CopyTo(stream);
				}
				return src;
			}
			else return null;
		}
			
    }
}
