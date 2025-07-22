using RestApiApp.InterfaceRepositories;
using RestApiApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using RestApiApp.Utils;
using RestApiApp.Dtos;
using RestApiApp.Exceptions;

namespace RestApiApp.Repositories
{
    public class GenericRepository<T, TDto> : IGenericRepository<T, TDto>
    where T : class
    where TDto : class
    {

        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }


        public virtual async Task<T> AddAsyc(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> DeleteAsyc(int id)
        {
            var data = await _context.Set<T>().FindAsync(id);
            if (data == null) return data;

            _context.Remove<T>(data);
            await _context.SaveChangesAsync();

            return data;
        }

        public virtual async Task<T> GetAsyc(Expression<Func<T, bool>> predicate)
        {
            var data = await _context.Set<T>()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(predicate);
            return data;
        }

        public virtual async Task<ICollection<T>> GetAsycAllAsync()
        {
            var data = await _context.Set<T>().AsNoTracking().ToListAsync<T>();
            return data;
        }

        public virtual async Task<T> UpdateAsync(int id, T entity)
        {
            var result = await _context.FindAsync<T>(id);
            if (result == null) return result;

            ObjectUpdateAsyncr.UpdateAsyncNonNullProperties(entity, result);
            await _context.SaveChangesAsync();

            return result;
        }


        public async Task<GetPagedResponeDto<TDto>> GetAsycPaged(
        Expression<Func<T, TDto>> selector,
        int page, int pageSize)
        {
            var query = _context.Set<T>().Select(selector).AsNoTracking();
            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new GetPagedResponeDto<TDto>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}