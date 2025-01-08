using MedicalSystemClassLibrary.Data;
using MedicalSystemClassLibrary.Models;
using MedicalSystemClassLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class MedicalFileController : Controller
{
    private readonly MedicalSystemDbContext _context;
    private readonly IMinioService _minioService;

    public MedicalFileController(MedicalSystemDbContext context, IMinioService minioService)
    {
        _context = context;
        _minioService = minioService;
    }

    // GET: MedicalFile
    public async Task<IActionResult> Index()
    {
        var medicalFiles = await _context.MedicalFiles
            .Select(m => new MedicalFileViewModel
            {
                Id = m.Id,
                ObjectId = m.ObjectId,
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
                ObjectId = m.ObjectId,
                ExaminationId = m.ExaminationId
            })
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medicalFile == null)
        {
            return NotFound();
        }

        return View(medicalFile);
    }

    // POST: MedicalFile/UploadFile
    [HttpPost]
    [Route("MedicalFile/UploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png"};
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest("Invalid file type. Only image files are allowed.");
        }

        string objectId;
        using (var stream = file.OpenReadStream())
        {
            objectId = await _minioService.PutObject(stream, file.FileName, file.ContentType, file.Length);
        }

        return Json(new { objectId, fileName = file.FileName, size = file.Length });
    }

     // GET: MedicalFile/DownloadFile/5
    [HttpGet]
    [Route("MedicalFile/DownloadFile/{id}")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var medicalFile = await _context.MedicalFiles.FindAsync(id);
        if (medicalFile == null)
        {
            return NotFound();
        }

        var minioObjectResponse = await _minioService.GetObject(medicalFile.ObjectId);
        if (minioObjectResponse == null)
        {
            return NotFound();
        }

        return File(minioObjectResponse.Data, minioObjectResponse.ContentType, Path.GetFileName(medicalFile.ObjectId));
    }

    // GET: MedicalFile/Create
    public IActionResult Create(string objectId)
    {
        var model = new MedicalFileViewModel();
        if (!string.IsNullOrEmpty(objectId))
        {
            model.ObjectId = objectId;
        }
        return View(model);
    }

    // POST: MedicalFile/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ExaminationId,ObjectId")] MedicalFileViewModel medicalFile)
    {
        if (ModelState.IsValid)
        {
            var newMedicalFile = new MedicalFile
            {
                ObjectId = medicalFile.ObjectId,
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
                ObjectId = m.ObjectId,
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ExaminationId,ObjectId")] MedicalFileViewModel medicalFile, IFormFile newFile)
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

                if (newFile != null && newFile.Length > 0)
                {
                    await _minioService.DeleteObject(existingMedicalFile.ObjectId);

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png"};
                    var extension = Path.GetExtension(newFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        return BadRequest("Invalid file type.");
                    }

                    string newObjectId;
                    using (var stream = newFile.OpenReadStream())
                    {
                        newObjectId = await _minioService.PutObject(stream, newFile.FileName, newFile.ContentType, newFile.Length);
                    }

                    existingMedicalFile.ObjectId = newObjectId;
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
                ObjectId = m.ObjectId,
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
            await _minioService.DeleteObject(medicalFile.ObjectId);

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
