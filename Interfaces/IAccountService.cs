
using BookingApi.DTO;
using BookingApi.Helpers;

namespace BookingApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountResponseDTO>> GetAll();
        Task<AccountResponseDTO> GetById(int id);
        Task<AccountResponseDTO> GetByEmail(string email);
        Task<APIResponse> Insert(AccountRequestDTO item);
        Task<AccountResponseDTO> Update(AccountRequestDTO item);
        Task<APIResponse> Delete(int id);
    }
}
