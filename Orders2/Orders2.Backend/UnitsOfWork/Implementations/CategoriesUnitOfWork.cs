using Orders2.Backend.Repositories.Interfaces;
using Orders2.Backend.UnitsOfWork.Implementations;
using Orders2.Backend.UnitsOfWork.Interfaces;
using Orders2.Shared.DTOs;
using Orders2.Shared.Entities;
using Orders2.Shared.Responses;

namespace Orders2.Backend.UnitsOfWork.Implementations;

public class CategoriesUnitOfWork : GenericUnitOfWork<Category>, ICategoriesUnitOfWork
{
    private readonly ICategoriesRepository _categoriesRepository;

    public CategoriesUnitOfWork(IGenericRepository<Category> repository, ICategoriesRepository categoriesRepository)
: base(repository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination) =>
await _categoriesRepository.GetAsync(pagination);

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await
_categoriesRepository.GetTotalRecordsAsync(pagination);
}
