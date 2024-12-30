using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemMvc.Models;
using System.Threading.Tasks;
using MedicalSystemClassLibrary.Data;
using System.Linq;
using MedicalSystemClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MedicalSystemMvc.Controllers
{
    public class MedicalFileController : Controller
    {
        private readonly MedicalSystemDbContext _context;

        public MedicalFileController(MedicalSystemDbContext context)
        {
            _context = context;
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

        // GET: MedicalFile/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MedicalFile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExaminationId")] MedicalFileViewModel medicalFile, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var resourcesFolder = Path.Combine(Directory.GetCurrentDirectory(), "resources");
                    if (!Directory.Exists(resourcesFolder))
                    {
                        Directory.CreateDirectory(resourcesFolder);
                    }

                    var filePath = Path.Combine(resourcesFolder, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var newMedicalFile = new MedicalFile
                    {
                        FilePath = filePath,
                        ExaminationId = medicalFile.ExaminationId
                    };

                    _context.Add(newMedicalFile);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please upload a file.");
                }
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExaminationId")] MedicalFileViewModel medicalFile, IFormFile file)
        {
            if (id != medicalFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedMedicalFile = await _context.MedicalFiles.FindAsync(id);
                    if (file != null && file.Length > 0)
                    {
                        var filePath = Path.Combine("wwwroot/uploads", file.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        updatedMedicalFile.FilePath = filePath;
                    }

                    updatedMedicalFile.ExaminationId = medicalFile.ExaminationId;

                    _context.Update(updatedMedicalFile);
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        public IActionResult Download(string filePath)
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}
