using MedicalSystemClassLibrary.Factories;
using MedicalSystemClassLibrary.Models;

public class PatientService
{
    private readonly IRepositoryFactory _repositoryFactory;

    public PatientService(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<Patient> GetPatientByIdAsync(int id)
    {
        var repository = _repositoryFactory.CreateRepository<Patient>();
        return await repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        var repository = _repositoryFactory.CreateRepository<Patient>();
        return await repository.GetAllAsync();
    }

    public async Task AddPatientAsync(Patient patient)
    {
        var repository = _repositoryFactory.CreateRepository<Patient>();
        await repository.AddAsync(patient);
    }

    public async Task UpdatePatientAsync(Patient patient)
    {
        var repository = _repositoryFactory.CreateRepository<Patient>();
        await repository.UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        var repository = _repositoryFactory.CreateRepository<Patient>();
        await repository.DeleteAsync(id);
    }
}
