using MedicalSystemClassLibrary.Factories;
using MedicalSystemClassLibrary.Repositories;
using Microsoft.Extensions.DependencyInjection;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    public RepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IRepository<T> CreateRepository<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<IRepository<T>>();
    }
}
