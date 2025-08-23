using Orders2.Shared.DTOs;
using Orders2.Shared.Entities;
using Orders2.Shared.Responses;

namespace Orders2.Backend.Repositories.Interfaces;

public interface ICountriesRepository
{
    Task<ActionResponse<Country>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<IEnumerable<Country>>> GetAsync();
}
