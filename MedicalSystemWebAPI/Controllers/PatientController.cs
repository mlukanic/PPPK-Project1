using MedicalSystemClassLibrary.Models;
using MedicalSystemClassLibrary.Repositories;
using MedicalSystemClassLibrary.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalSystemWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IRepository<Patient> _patientRepository;

        public PatientController(IRepository<Patient> patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllAsync();
            return Ok(patients);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            if (!OibValidator.IsValidOIB(patient.OIB))
            {
                return BadRequest("Invalid OIB.");
            }

            await _patientRepository.AddAsync(patient);
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdatePatient(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            if (!OibValidator.IsValidOIB(patient.OIB))
            {
                return BadRequest("Invalid OIB.");
            }

            await _patientRepository.UpdateAsync(patient);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            await _patientRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
