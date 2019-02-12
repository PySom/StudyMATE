using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyMate.Data;
using StudyMate.Models;

namespace StudyMate.Controllers
{
    public class CoursesTakenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesTakenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseTakens
        public async Task<IActionResult> Index()
        {
            return View(await _context.CourseTaken.ToListAsync());
        }

        // GET: CourseTakens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseTaken = await _context.CourseTaken
                .SingleOrDefaultAsync(m => m.CourseTakenID == id);
            if (courseTaken == null)
            {
                return NotFound();
            }

            return View(courseTaken);
        }

        // GET: CourseTakens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CourseTakens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseTakenID,Score,DateSubmitted,ApplicationUserID,CoursesID")] CourseTaken courseTaken)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseTaken);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courseTaken);
        }

        // GET: CourseTakens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseTaken = await _context.CourseTaken.SingleOrDefaultAsync(m => m.CourseTakenID == id);
            if (courseTaken == null)
            {
                return NotFound();
            }
            return View(courseTaken);
        }

        // POST: CourseTakens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseTakenID,Score,DateSubmitted,ApplicationUserID,CoursesID")] CourseTaken courseTaken)
        {
            if (id != courseTaken.CourseTakenID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseTaken);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseTakenExists(courseTaken.CourseTakenID))
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
            return View(courseTaken);
        }

        // GET: CourseTakens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseTaken = await _context.CourseTaken
                .SingleOrDefaultAsync(m => m.CourseTakenID == id);
            if (courseTaken == null)
            {
                return NotFound();
            }

            return View(courseTaken);
        }

        // POST: CourseTakens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseTaken = await _context.CourseTaken.SingleOrDefaultAsync(m => m.CourseTakenID == id);
            _context.CourseTaken.Remove(courseTaken);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseTakenExists(int id)
        {
            return _context.CourseTaken.Any(e => e.CourseTakenID == id);
        }
    }
}
