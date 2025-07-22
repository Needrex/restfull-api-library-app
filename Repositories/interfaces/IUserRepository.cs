using RestApiApp.Models;
using RestApiApp.Dtos;
using System.Linq.Expressions;

namespace RestApiApp.InterfaceRepositories
{
    public interface IUserRepository
    {
        Task<Users> CreateAsyc(Users user);
        Task<Users> GetAsyc(Expression<Func<Users, bool>> predicate);
        Task<Users> UpdateAsync(int id, Users user);
        Task<Users> UpdateLastLoginAsync(Users user);
    }
}