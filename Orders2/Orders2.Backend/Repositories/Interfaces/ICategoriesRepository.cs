using Orders2.Shared.DTOs;
using Orders2.Shared.Entities;
using Orders2.Shared.Responses;

namespace Orders2.Backend.Repositories.Interfaces;

public interface ICategoriesRepository
{
    Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}
