using AutoMapper;
using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Models;
using BookingApi.Services.Interfaces;

namespace BookingApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse<int>> Delete(int id)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                bool isSuccess = await _accountRepository.Delete(id);
                if (isSuccess)
                {
                    response.StatusCode = 201;
                    response.Result = id;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Not Found";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<List<AccountResponseDTO>> GetAll()
        {
            var result = await _accountRepository.GetAll();

            if (result == null) return null;

            List<AccountResponseDTO> account = _mapper.Map<List<Account>, List<AccountResponseDTO>>(result);

            return account;
        }

        public async Task<AccountResponseDTO> GetByEmail(string email)
        {
            var result = await _accountRepository.GetByEmail(email);

            if (result == null) return null;

            AccountResponseDTO account = _mapper.Map<Account, AccountResponseDTO>(result);

            return account;
        }

        public async Task<AccountResponseDTO> GetById(int id)
        {
            var result = await _accountRepository.GetById(id);

            if (result == null) return null;

            AccountResponseDTO account = _mapper.Map<Account, AccountResponseDTO>(result);

            return account;
        }

        public async Task<APIResponse<int>> Insert(AccountRequestDTO item)
        {
            APIResponse<int> response = new APIResponse<int>();

            try
            {
                (bool isSuccess, int insertedItemId) = await _accountRepository.Insert(item);
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

        public async Task<AccountResponseDTO> Update(AccountRequestDTO item)
        {
            Account data = await _accountRepository.Update(item);

            if (data == null) return null;

            AccountResponseDTO account = _mapper.Map<Account, AccountResponseDTO>(data);

            return account;
        }
    }
}
