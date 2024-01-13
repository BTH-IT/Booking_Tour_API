using BookingApi.DTO;
using BookingApi.Helpers;

namespace BookingApi.Interfaces
{
    public interface IDestinationService
    {
        Task<List<DestinationResponseDTO>> GetAll();
        Task<DestinationResponseDTO> GetById(int id);
        Task<APIResponse<int>> Insert(DestinationRequestDTO item);
        Task<DestinationResponseDTO> Update(DestinationRequestDTO item);
        Task<APIResponse<int>> Delete(int id);
    }
}
