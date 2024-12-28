using MedicalSystemClassLibrary;
using MedicalSystemClassLibrary.Data;
using MedicalSystemClassLibrary.Models;
using MedicalSystemClassLibrary.Repositories;
using Microsoft.EntityFrameworkCore;

public class PatientRepository : IRepository<Patient>
{
    private readonly MedicalSystemDbContext _context;

    public PatientRepository(MedicalSystemDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.MedicalRecords)
            .Include(p => p.Examinations)
            .Include(p => p.Prescriptions)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task AddAsync(Patient entity)
    {
        await _context.Patients.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Patient entity)
    {
        _context.Patients.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Patients.FindAsync(id);
        if (entity != null)
        {
            _context.Patients.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
