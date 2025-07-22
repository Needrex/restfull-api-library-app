using RestApiApp.Models;

namespace RestApiApp.InterfaceRepositories
{
    public interface IUserOtpsRepository : IGenericRepository<UserOtps, UserOtps>
    {
        Task<UserOtps> AddAsyc(UserOtps userOtps);
        Task<UserOtps> UpdateOTPStatusAsyc(int idOtp, UserOtps userOtps);
    }
}