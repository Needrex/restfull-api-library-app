using System.Linq.Expressions;
using RestApiApp.Dtos;

namespace RestApiApp.InterfaceRepositories
{
    public interface IGenericRepository<T, TDto>
    where TDto : class
    {
        Task<ICollection<T>> GetAsycAllAsync();
        Task<T> GetAsyc(Expression<Func<T, bool>> predicate);
        Task<T> AddAsyc(T entity);
        Task<T> UpdateAsync(int id, T entity);
        Task<T> DeleteAsyc(int id);
        Task<GetPagedResponeDto<TDto>> GetAsycPaged(
        Expression<Func<T, TDto>> selector,
        int page, int pageSize);
    }
}