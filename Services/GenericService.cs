using System.Linq.Expressions;
using RestApiApp.Dtos;
using RestApiApp.InterfaceRepositories;
using RestApiApp.InterfaceServices;

namespace RestApiApp.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericService<T> _genericRepository;

        public GenericService(IGenericService<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        async public Task<ICollection<T>> GetAsycAllAsync()
        {
            var data = await _genericRepository.GetAsycAllAsync();
            return data;
        }

        async public Task<T> GetAsyc(Expression<Func<T, bool>> predicate)
        {
            var data = await _genericRepository.GetAsyc(predicate);
            return data;
        }

        async virtual public Task<T> AddAsyc(T entity)
        {
            var data = await _genericRepository.AddAsyc(entity);
            return data;
        }

        async public Task<T> UpdateAsync(int id, T entity)
        {
            var data = await _genericRepository.UpdateAsync(id, entity);
            return data;
        }

        async public Task<T> DeleteAsyc(int id)
        {
            var data = await _genericRepository.DeleteAsyc(id);
            return data;
        }
    }
}