using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(AuthRegisterDTO register);
        Task<object> Login(AuthLoginDTO login);
        Task<string> RefreshToken(string refreshToken);
        Task<bool> GetProfile(AuthLoginDTO item);
    }
}