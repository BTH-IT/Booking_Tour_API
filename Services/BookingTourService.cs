using AutoMapper;
using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Interfaces;
using BookingApi.Models;
using BookingApi.Services.Interfaces;

namespace BookingApi.Services
{
    public class BookingTourService : IBookingTourService
    {
        private readonly IBookingTourRepository _bookingTourRepository;
        private readonly IMapper _mapper;

        public BookingTourService(IBookingTourRepository bookingTourRepository, IMapper mapper)
        {
            _bookingTourRepository = bookingTourRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<int>> Delete(int id)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                bool isSuccess = await _bookingTourRepository.Delete(id);
                if (isSuccess)
                {
                    response.StatusCode = 201;
                    response.Result = id;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Something went wrong";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<List<BookingTourResponseDTO>> GetAll()
        {
            try
            {
                var bookingTours = await _bookingTourRepository.GetAll();
                var responseDTOs = _mapper.Map<List<BookingTour>, List<BookingTourResponseDTO>>(bookingTours);

                return responseDTOs;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }

        public async Task<BookingTourResponseDTO> GetById(int id)
        {
            try
            {
                var bookingTour = await _bookingTourRepository.GetById(id);
                var responseDTO = _mapper.Map<BookingTour, BookingTourResponseDTO>(bookingTour);

                return responseDTO;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }

        public async Task<APIResponse<int>> Insert(BookingTourRequestDTO item)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                (bool isSuccess, int insertedItemId) = await _bookingTourRepository.Insert(item);
                if (isSuccess)
                {
                    response.StatusCode = 201;
                    response.Result = insertedItemId;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Something went wrong";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<BookingTourResponseDTO> Update(BookingTourRequestDTO item)
        {
            try
            {
                var updatedBookingTour = await _bookingTourRepository.Update(item);
                var responseDTO = _mapper.Map<BookingTour, BookingTourResponseDTO>(updatedBookingTour);

                return responseDTO;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                return null;
            }
        }
    }
}
