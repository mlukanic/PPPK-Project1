using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSystemMvc.Models;
using MedicalSystemClassLibrary.Data;
using MedicalSystemClassLibrary.Models;
using System.Threading.Tasks;
using System.Linq;
using MedicalSystemClassLibrary.Enums;
using MedicalSystemClassLibrary.Dictionaries;

namespace MedicalSystemMvc.Controllers
{
    public class PatientController : Controller
    {
        private readonly MedicalSystemDbContext _context;
        private readonly CsvExporter _csvExporter;

        public PatientController(MedicalSystemDbContext context, CsvExporter csvExporter)
        {
            _context = context;
            _csvExporter = csvExporter;
        }

        // GET: Patient
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            IQueryable<Patient> patients = _context.Patients;

            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.LastName.Contains(searchString) || p.OIB.Contains(searchString));
            }

            var patientList = await patients.ToListAsync();

            var patientViewModels = patientList
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    OIB = p.OIB,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender
                }).ToList();

            return View(patientViewModels);
        }

        public IActionResult Details(int id)
        {
            var patient = _context.Patients
                .Where(p => p.Id == id)
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    OIB = p.OIB,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    MedicalRecords = p.MedicalRecords.Select(mr => new MedicalRecordViewModel
                    {
                        DiseaseName = mr.DiseaseName,
                        StartDate = mr.StartDate,
                        EndDate = mr.EndDate
                    }).ToList(),
                    Prescriptions = p.Prescriptions.Select(pr => new PrescriptionViewModel
                    {
                        Medication = pr.Medication,
                        Dosage = pr.Dosage
                    }).ToList(),
                    Examinations = p.Examinations.Select(ex => new ExaminationViewModel
                    {
                        Date = ex.Date,
                        Type = Enum.Parse<ExaminationType>(ex.Type)
                    }).ToList()
                })
                .FirstOrDefault();

            if (patient == null)
            {
                return NotFound();
            }

            // Pass the examination types to the view using ViewBag
            ViewBag.ExaminationTypes = ExaminationTypeDict.Descriptions;

            return View(patient);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientViewModel patient)
        {
            if (ModelState.IsValid)
            {
                var newPatient = new Patient
                {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    OIB = patient.OIB,
                    DateOfBirth = DateTime.SpecifyKind(patient.DateOfBirth, DateTimeKind.Utc),
                    Gender = patient.Gender
                };

                _context.Add(newPatient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    OIB = p.OIB,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientViewModel patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPatient = await _context.Patients.FindAsync(id);
                    if (existingPatient == null)
                    {
                        return NotFound();
                    }

                    existingPatient.FirstName = patient.FirstName;
                    existingPatient.LastName = patient.LastName;
                    existingPatient.OIB = patient.OIB;
                    existingPatient.DateOfBirth = DateTime.SpecifyKind(patient.DateOfBirth, DateTimeKind.Utc);
                    existingPatient.Gender = patient.Gender;

                    _context.Update(existingPatient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    OIB = p.OIB,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ExportToCsv(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.MedicalRecords)
                .Include(p => p.Examinations)
                .Include(p => p.Prescriptions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            var csv = _csvExporter.ExportPatientsToCsv(new List<Patient> { patient });
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", $"PatientDetails-{patient.OIB}.csv");
        }
    }
}
