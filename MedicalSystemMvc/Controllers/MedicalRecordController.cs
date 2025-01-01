using Microsoft.AspNetCore.Mvc;
using MedicalSystemMvc.Models;
using MedicalSystemClassLibrary.Data;
using MedicalSystemClassLibrary.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemMvc.Controllers
{
    public class MedicalRecordController : Controller
    {
        private readonly MedicalSystemDbContext _context;

        public MedicalRecordController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: MedicalRecord
        public async Task<IActionResult> Index()
        {
            var medicalRecords = await _context.MedicalRecords
                .Select(mr => new MedicalRecordViewModel
                {
                    Id = mr.Id,
                    DiseaseName = mr.DiseaseName,
                    StartDate = mr.StartDate,
                    EndDate = mr.EndDate,
                    PatientId = mr.PatientId
                })
                .ToListAsync();

            return View(medicalRecords);
        }

        // GET: MedicalRecord/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var medicalRecord = await _context.MedicalRecords
                .Where(mr => mr.Id == id)
                .Select(mr => new MedicalRecordViewModel
                {
                    Id = mr.Id,
                    DiseaseName = mr.DiseaseName,
                    StartDate = mr.StartDate,
                    EndDate = mr.EndDate,
                    PatientId = mr.PatientId
                })
                .FirstOrDefaultAsync();

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // GET: MedicalRecord/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MedicalRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var medicalRecord = new MedicalRecord
                {
                    DiseaseName = model.DiseaseName,
                    StartDate = DateTime.SpecifyKind(model.StartDate, DateTimeKind.Utc),
                    EndDate = model.EndDate.HasValue ? DateTime.SpecifyKind(model.EndDate.Value, DateTimeKind.Utc) : (DateTime?)null,
                    PatientId = model.PatientId
                };

                _context.Add(medicalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: MedicalRecord/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            var model = new MedicalRecordViewModel
            {
                Id = medicalRecord.Id,
                DiseaseName = medicalRecord.DiseaseName,
                StartDate = medicalRecord.StartDate,
                EndDate = medicalRecord.EndDate,
                PatientId = medicalRecord.PatientId
            };

            return View(model);
        }

        // POST: MedicalRecord/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicalRecordViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var medicalRecord = await _context.MedicalRecords.FindAsync(id);
                if (medicalRecord == null)
                {
                    return NotFound();
                }

                medicalRecord.DiseaseName = model.DiseaseName;
                medicalRecord.StartDate = DateTime.SpecifyKind(model.StartDate, DateTimeKind.Utc);
                medicalRecord.EndDate = model.EndDate.HasValue ? DateTime.SpecifyKind(model.EndDate.Value, DateTimeKind.Utc) : (DateTime?)null;
                medicalRecord.PatientId = model.PatientId;

                _context.Update(medicalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: MedicalRecord/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var medicalRecord = await _context.MedicalRecords
                .Where(mr => mr.Id == id)
                .Select(mr => new MedicalRecordViewModel
                {
                    Id = mr.Id,
                    DiseaseName = mr.DiseaseName,
                    StartDate = mr.StartDate,
                    EndDate = mr.EndDate,
                    PatientId = mr.PatientId
                })
                .FirstOrDefaultAsync();

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // POST: MedicalRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
