using Microsoft.AspNetCore.Mvc;
using MedicalSystemMvc.Models;
using MedicalSystemClassLibrary.Data;
using MedicalSystemClassLibrary.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedicalSystemClassLibrary.Enums;
using System.Collections.Generic;
using MedicalSystemClassLibrary.Dictionaries;

namespace MedicalSystemMvc.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly MedicalSystemDbContext _context;

        public ExaminationController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: Examination
        public async Task<IActionResult> Index()
        {
            var examinations = await _context.Examinations.ToListAsync();
            return View(examinations);
        }

        // GET: Examination/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examination == null)
            {
                return NotFound();
            }

            return View(examination);
        }

        // GET: Examination/Create
        public IActionResult Create()
        {
            ViewBag.ExaminationTypes = ExaminationTypeDict.Descriptions;
            return View();
        }

        // POST: Examination/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExaminationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Convert Date to UTC
                model.Date = DateTime.SpecifyKind(model.Date, DateTimeKind.Utc);

                var examination = new Examination
                {
                    Date = model.Date,
                    Type = model.Type.ToString(),
                    PatientId = model.PatientId
                };

                _context.Add(examination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ExaminationTypes = ExaminationTypeDict.Descriptions;
            return View(model);
        }

        // GET: Examination/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations.FindAsync(id);
            if (examination == null)
            {
                return NotFound();
            }

            var model = new ExaminationViewModel
            {
                Id = examination.Id,
                Date = examination.Date,
                Type = Enum.Parse<ExaminationType>(examination.Type),
                PatientId = examination.PatientId
            };

            ViewBag.ExaminationTypes = ExaminationTypeDict.Descriptions;
            return View(model);
        }

        // POST: Examination/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExaminationViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var examination = await _context.Examinations.FindAsync(id);
                    if (examination == null)
                    {
                        return NotFound();
                    }

                    // Convert Date to UTC
                    model.Date = DateTime.SpecifyKind(model.Date, DateTimeKind.Utc);

                    examination.Date = model.Date;
                    examination.Type = model.Type.ToString();
                    examination.PatientId = model.PatientId;

                    _context.Update(examination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExaminationExists(model.Id))
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
            ViewBag.ExaminationTypes = ExaminationTypeDict.Descriptions;
            return View(model);
        }

        // GET: Examination/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examination = await _context.Examinations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (examination == null)
            {
                return NotFound();
            }

            return View(examination);
        }

        // POST: Examination/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examination = await _context.Examinations.FindAsync(id);
            _context.Examinations.Remove(examination);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExaminationExists(int id)
        {
            return _context.Examinations.Any(e => e.Id == id);
        }
    }
}
