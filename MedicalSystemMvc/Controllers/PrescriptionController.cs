using Microsoft.AspNetCore.Mvc;
using MedicalSystemMvc.Models;
using MedicalSystemClassLibrary.Data;
using MedicalSystemClassLibrary.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemMvc.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly MedicalSystemDbContext _context;

        public PrescriptionController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: Prescription
        public async Task<IActionResult> Index()
        {
            var prescriptions = await _context.Prescriptions
                .Select(p => new PrescriptionViewModel
                {
                    Id = p.Id,
                    Medication = p.Medication,
                    Dosage = p.Dosage,
                    PatientId = p.PatientId
                })
                .ToListAsync();

            return View(prescriptions);
        }

        // GET: Prescription/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var prescription = await _context.Prescriptions
                .Where(p => p.Id == id)
                .Select(p => new PrescriptionViewModel
                {
                    Id = p.Id,
                    Medication = p.Medication,
                    Dosage = p.Dosage,
                    PatientId = p.PatientId
                })
                .FirstOrDefaultAsync();

            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // GET: Prescription/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var prescription = new Prescription
                {
                    Medication = model.Medication,
                    Dosage = model.Dosage,
                    PatientId = model.PatientId
                };

                _context.Add(prescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Prescription/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            var model = new PrescriptionViewModel
            {
                Id = prescription.Id,
                Medication = prescription.Medication,
                Dosage = prescription.Dosage,
                PatientId = prescription.PatientId
            };

            return View(model);
        }

        // POST: Prescription/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrescriptionViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var prescription = await _context.Prescriptions.FindAsync(id);
                if (prescription == null)
                {
                    return NotFound();
                }

                prescription.Medication = model.Medication;
                prescription.Dosage = model.Dosage;
                prescription.PatientId = model.PatientId;

                _context.Update(prescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Prescription/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var prescription = await _context.Prescriptions
                .Where(p => p.Id == id)
                .Select(p => new PrescriptionViewModel
                {
                    Id = p.Id,
                    Medication = p.Medication,
                    Dosage = p.Dosage,
                    PatientId = p.PatientId
                })
                .FirstOrDefaultAsync();

            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // POST: Prescription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
