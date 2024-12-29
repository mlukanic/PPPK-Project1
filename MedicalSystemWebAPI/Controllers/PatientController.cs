using MedicalSystemClassLibrary.Models;
using MedicalSystemClassLibrary.Repositories;
using MedicalSystemClassLibrary.Utilities;
using MedicalSystemWebAPI.Dtos;
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
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllAsync();
            var patientDtos = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                OIB = p.OIB,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender
            }).ToList();

            return Ok(patientDtos);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var patientDto = new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                OIB = patient.OIB,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender
            };

            return Ok(patientDto);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<PatientDto>> CreatePatient(PatientDto patientDto)
        {
            if (!OibValidator.IsValidOIB(patientDto.OIB))
            {
                return BadRequest("Invalid OIB.");
            }

            var patient = new Patient
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                OIB = patientDto.OIB,
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender
            };

            await _patientRepository.AddAsync(patient);

            patientDto.Id = patient.Id;

            return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patientDto);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientDto patientDto)
        {
            if (id != patientDto.Id)
            {
                return BadRequest();
            }

            if (!OibValidator.IsValidOIB(patientDto.OIB))
            {
                return BadRequest("Invalid OIB.");
            }

            var patient = new Patient
            {
                Id = patientDto.Id,
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                OIB = patientDto.OIB,
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender
            };

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
