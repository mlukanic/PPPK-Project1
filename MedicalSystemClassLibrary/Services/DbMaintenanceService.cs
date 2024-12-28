using MedicalSystemClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystemClassLibrary.Services
{
    public class DbMaintenanceService
    {
        private readonly MedicalSystemDbContext _context;

        public DbMaintenanceService(MedicalSystemDbContext context)
        {
            _context = context;
        }

        public void ClearPatientsAndResetId()
        {
            // Delete all patients
            _context.Patients.RemoveRange(_context.Patients);
            _context.SaveChanges();

            // Reset the sequence
            _context.ResetPatientIdSequence();
        }
    }
}
