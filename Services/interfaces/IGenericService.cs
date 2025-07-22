using System.Linq.Expressions;
using RestApiApp.Dtos;

namespace RestApiApp.InterfaceServices
{
    public interface IGenericService<T>
    {
        Task<ICollection<T>> GetAsycAllAsync();
        Task<T> GetAsyc(Expression<Func<T, bool>> predicate);
        Task<T> AddAsyc(T entity);
        Task<T> UpdateAsync(int id, T entity);
        Task<T> DeleteAsyc(int id);
    }
}