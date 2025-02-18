﻿using Microsoft.AspNetCore.Mvc;
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
                .Select(m => new MedicalRecordViewModel
                {
                    Id = m.Id,
                    DiseaseName = m.DiseaseName,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    PatientId = m.PatientId
                })
                .ToListAsync();
            return View(medicalRecords);
        }

        // GET: MedicalRecord/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords
                .Select(m => new MedicalRecordViewModel
                {
                    Id = m.Id,
                    DiseaseName = m.DiseaseName,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    PatientId = m.PatientId
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            return View(medicalRecord);
        }

        // GET: MedicalRecord/Create
        public IActionResult Create(int patientId)
        {
            var model = new MedicalRecordViewModel
            {
                PatientId = patientId,
                StartDate = DateTime.UtcNow // Set StartDate to DateTime.UtcNow by default
            };
            return View(model);
        }

        // POST: MedicalRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRecordViewModel medicalRecord)
        {
            if (ModelState.IsValid)
            {
                var record = new MedicalRecord
                {
                    PatientId = medicalRecord.PatientId,
                    DiseaseName = medicalRecord.DiseaseName,
                    StartDate = medicalRecord.StartDate.ToUniversalTime(), // Convert to UTC
                    EndDate = medicalRecord.EndDate?.ToUniversalTime() // Convert to UTC if not null
                };
                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicalRecord);
        }

        // GET: MedicalRecord/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                try
                {
                    var medicalRecord = await _context.MedicalRecords.FindAsync(id);
                    if (medicalRecord == null)
                    {
                        return NotFound();
                    }

                    medicalRecord.DiseaseName = model.DiseaseName;
                    medicalRecord.StartDate = model.StartDate.ToUniversalTime(); // Convert to UTC
                    medicalRecord.EndDate = model.EndDate?.ToUniversalTime(); // Convert to UTC if not null
                    medicalRecord.PatientId = model.PatientId;

                    _context.Update(medicalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRecordExists(model.Id))
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
            return View(model);
        }

        // GET: MedicalRecord/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecord = await _context.MedicalRecords
                .Select(m => new MedicalRecordViewModel
                {
                    Id = m.Id,
                    DiseaseName = m.DiseaseName,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    PatientId = m.PatientId
                })
                .FirstOrDefaultAsync(m => m.Id == id);
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

        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.Id == id);
        }
    }
}
