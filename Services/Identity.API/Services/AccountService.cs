using AutoMapper;
using Identity.API.Entites;
using Identity.API.Repositories.Interfaces;
using Identity.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Services
{
    public class AccountService : IAccountSerivce
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public AccountService(IAccountRepository accountRepository,
            IMapper mapper,
            ILogger logger)
        {
            this._accountRepository = accountRepository;    
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<ApiResponse<int>> CreateAsync(AccountRequestDTO item)
        {
            _logger.LogInformation($"Begin : AccountService - CreateAsync");
            var accountEntity = _mapper.Map<Account>(item);
            var newId = await _accountRepository.CreateAsync(accountEntity);
            _logger.LogInformation($"End : AccountService - CreateAsync");
            return new ApiResponse<int>(200, newId,"Tạo thành công");
        }

        public async Task<ApiResponse<int>> DeleteAsync(int id)
        {
            _logger.LogInformation($"Begin : AccountService - DeleteAsync : {id}");
            var account =  await _accountRepository.GetAccountByIdAsync(id);
            if(account == null)
            {
                return new ApiResponse<int>(404,0,"Không tìm thấy tài khoản cần xóa");
            }
            _accountRepository.Delete(account);
            var result  = await _accountRepository.SaveChangesAsync();
            if (result > 0) {
                _logger.LogInformation($"End : AccountService - DeleteAsync : {id} - Xóa thất bại");
                return new ApiResponse<int>(200,result, "Xóa tài khoản thành công");
            }
            else
            {
                _logger.LogInformation($"End : AccountService - DeleteAsync : {id} - Xóa thất bại");
                return new ApiResponse<int>(400, result, "Xóa tài khoản thất bại");

            }
        }

        public async Task<ApiResponse<List<AccountResponseDTO>>> GetAllAsync()
        {
            _logger.LogInformation($"Begin : AccountService - GetAllAsync");
            var accounts = await _accountRepository.FindAll(false, account => account.Role, account => account.Role.RoleDetails).ToListAsync();
            _logger.LogInformation($"Mapping list of account to dto");
            var data = _mapper.Map<List<AccountResponseDTO>>(accounts);  
            _logger.LogInformation($"End : AccountService - GetAllAsync");
            return new ApiResponse<List<AccountResponseDTO>>(200, data, "Lấy dữ liệu thành công");
        }

        public async Task<ApiResponse<AccountResponseDTO>> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Begin : AccountService - GetByIdAsync");
            
            var account = await _accountRepository.GetAccountByIdAsync(id);
            if (account == null) 
            {
                return new ApiResponse<AccountResponseDTO>(404, null, "Không tìm thấy tài khoản");
            }
            var data = _mapper.Map<AccountResponseDTO>(account);
            _logger.LogInformation($"End : AccountService - GetByIdAsync");
            return new ApiResponse<AccountResponseDTO>(200, data, "Lấy dữ liệu tài khoản thành công");
        }

        public async Task<ApiResponse<AccountResponseDTO>> UpdateAsync(AccountRequestDTO item)
        {
            _logger.LogInformation($"Begin : AccountService - UpdateAsync");
            var account = await _accountRepository.FindByCondition(c => c.Id.Equals(item.Id)).FirstOrDefaultAsync();
            if (account == null) 
            {
                return new ApiResponse<AccountResponseDTO>(404, null, "Không tìm thấy tài khoản");
            }
            account = _mapper.Map<Account>(item);
            var result = await _accountRepository.UpdateAsync(account);
            if(result > 0)
            {
                return new ApiResponse<AccountResponseDTO>(200, _mapper.Map<AccountResponseDTO>(account), "Cập nhật thành công");
            }
            _logger.LogInformation($"End : AccountService - UpdateAsync");
            return new ApiResponse<AccountResponseDTO>(200, null, "Có lỗi xảy ra");
        }
    }
}
