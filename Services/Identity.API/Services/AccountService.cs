using AutoMapper;
using Identity.API.Entites;
using Identity.API.Repositories.Interfaces;
using Identity.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Identity.API.Services
{
	public class AccountService : IAccountService
	{
		private readonly IAccountRepository _accountRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		public AccountService(IAccountRepository accountRepository, IMapper mapper, ILogger logger)
		{
			_accountRepository = accountRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<ApiResponse<int>> CreateAsync(AccountRequestDTO item)
		{
			_logger.Information("Begin: AccountService - CreateAsync");

			if (await _accountRepository.GetAccountByEmailAsync(item.Email) != null)
			{
				return new ApiResponse<int>(400, -1, "Email already exists");
			}

			var accountEntity = _mapper.Map<Account>(item);
			var newId = await _accountRepository.CreateAsync(accountEntity);

			_logger.Information("End: AccountService - CreateAsync");
			return new ApiResponse<int>(200, newId, "Account created successfully");
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: AccountService - DeleteAsync: {id}");
			var account = await _accountRepository.GetAccountByIdAsync(id);
			if (account == null)
			{
				return new ApiResponse<int>(404, 0, "Account not found");
			}

			await _accountRepository.DeleteAccountAsync(id); 
			_logger.Information($"End: AccountService - DeleteAsync: {id} - Deletion succeeded");
			return new ApiResponse<int>(200, id, "Account deleted successfully");
		}

		public async Task<ApiResponse<List<AccountResponseDTO>>> GetAllAsync()
		{
			_logger.Information("Begin: AccountService - GetAllAsync");
			var accounts = await _accountRepository.GetAccountsAsync();
			var data = _mapper.Map<List<AccountResponseDTO>>(accounts);
			_logger.Information("End: AccountService - GetAllAsync");
			return new ApiResponse<List<AccountResponseDTO>>(200, data, "Data retrieved successfully");

		}

		public async Task<ApiResponse<AccountResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information("Begin: AccountService - GetByIdAsync");
			var account = await _accountRepository.GetAccountByIdAsync(id);
			if (account == null)
			{
				return new ApiResponse<AccountResponseDTO>(404, null, "Account not found");
			}
			var data = _mapper.Map<AccountResponseDTO>(account);
			_logger.Information("End: AccountService - GetByIdAsync");
			return new ApiResponse<AccountResponseDTO>(200, data, "Account data retrieved successfully");
		}

		public async Task<ApiResponse<AccountResponseDTO>> UpdateAsync(AccountRequestDTO item)
		{
			_logger.Information("Begin: AccountService - UpdateAsync");
			var account = await _accountRepository.GetAccountByIdAsync(item.Id);
			if (account == null)
			{
				return new ApiResponse<AccountResponseDTO>(404, null, "Account not found");
			}

			if (await _accountRepository.GetAccountByEmailAsync(item.Email) != null)
			{
				return new ApiResponse<AccountResponseDTO>(400, null, "Email already exists");
			}

			_mapper.Map(item, account); 
			await _accountRepository.UpdateAccountAsync(account);

			_logger.Information("End: AccountService - UpdateAsync");
			return new ApiResponse<AccountResponseDTO>(200, _mapper.Map<AccountResponseDTO>(account), "Account updated successfully");
		}
	}
}
