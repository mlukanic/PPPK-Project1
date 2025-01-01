using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemMvc.Models;
using System.Threading.Tasks;
using MedicalSystemClassLibrary.Data;
using System.Linq;
using MedicalSystemClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;

namespace MedicalSystemMvc.Controllers
{
    public class MedicalFileController : Controller
    {
        private readonly MedicalSystemDbContext _context;
        private readonly FileUploadController _fileUploadController;

        public MedicalFileController(MedicalSystemDbContext context, FileUploadController fileUploadController)
        {
            _context = context;
            _fileUploadController = fileUploadController;
        }

        // GET: MedicalFile
        public async Task<IActionResult> Index()
        {
            var medicalFiles = await _context.MedicalFiles
                .Select(m => new MedicalFileViewModel
                {
                    Id = m.Id,
                    FilePath = m.FilePath,
                    ExaminationId = m.ExaminationId
                }).ToListAsync();

            return View(medicalFiles);
        }

        // GET: MedicalFile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalFile = await _context.MedicalFiles
                .Select(m => new MedicalFileViewModel
                {
                    Id = m.Id,
                    FilePath = m.FilePath,
                    ExaminationId = m.ExaminationId
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalFile == null)
            {
                return NotFound();
            }

            return View(medicalFile);
        }

        [HttpPost]
        [Route("MedicalFile/UploadFile")]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            var size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\Files", formFile.FileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Json(new { count = files.Count, size, filePaths });
        }

        public IActionResult Create(string filePath)
        {
            var model = new MedicalFileViewModel();
            if (!string.IsNullOrEmpty(filePath))
            {
                model.FilePath = filePath;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExaminationId,FilePath")] MedicalFileViewModel medicalFile)
        {
            if (ModelState.IsValid)
            {
                var newMedicalFile = new MedicalFile
                {
                    FilePath = medicalFile.FilePath,
                    ExaminationId = medicalFile.ExaminationId
                };

                _context.Add(newMedicalFile);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(medicalFile);
        }

        // GET: MedicalFile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalFile = await _context.MedicalFiles
                .Select(m => new MedicalFileViewModel
                {
                    Id = m.Id,
                    FilePath = m.FilePath,
                    ExaminationId = m.ExaminationId
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalFile == null)
            {
                return NotFound();
            }

            return View(medicalFile);
        }

        // POST: MedicalFile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExaminationId,FilePath")] MedicalFileViewModel medicalFile)
        {
            if (id != medicalFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMedicalFile = await _context.MedicalFiles.FindAsync(id);
                    if (existingMedicalFile == null)
                    {
                        return NotFound();
                    }

                    // Update the file path if a new file was uploaded
                    if (!string.IsNullOrEmpty(medicalFile.FilePath))
                    {
                        existingMedicalFile.FilePath = medicalFile.FilePath;
                    }

                    existingMedicalFile.ExaminationId = medicalFile.ExaminationId;

                    _context.Update(existingMedicalFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalFileExists(medicalFile.Id))
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
            return View(medicalFile);
        }

        // GET: MedicalFile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalFile = await _context.MedicalFiles
                .Select(m => new MedicalFileViewModel
                {
                    Id = m.Id,
                    FilePath = m.FilePath,
                    ExaminationId = m.ExaminationId
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalFile == null)
            {
                return NotFound();
            }

            return View(medicalFile);
        }

        // POST: MedicalFile/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var medicalFile = await _context.MedicalFiles.FindAsync(id);
            if (medicalFile != null)
            {
                _context.MedicalFiles.Remove(medicalFile);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalFileExists(int id)
        {
            return _context.MedicalFiles.Any(e => e.Id == id);
        }
    }
}
