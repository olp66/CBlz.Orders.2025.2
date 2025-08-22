using Orders2.Backend.Repositories.Interfaces;
using Orders2.Backend.UnitsOfWork.Implementations;
using Orders2.Backend.UnitsOfWork.Interfaces;
using Orders2.Shared.Entities;
using Orders2.Shared.Responses;

namespace Orders2.Backend.UnitsOfWork.Implementations;

public class CountriesUnitOfWork : GenericUnitOfWork<Country>, ICountriesUnitOfWork
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesUnitOfWork(IGenericRepository<Country> repository, ICountriesRepository countriesRepository) :
base(repository)
    {
        _countriesRepository = countriesRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync() => await
_countriesRepository.GetAsync();

    public override async Task<ActionResponse<Country>> GetAsync(int id) => await _countriesRepository.GetAsync(id);
}
