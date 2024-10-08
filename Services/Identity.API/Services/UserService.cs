using AutoMapper;
using Identity.API.Entites;
using Identity.API.Repositories;
using Identity.API.Repositories.Interfaces;
using Identity.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

namespace Identity.API.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public UserService(
			IUserRepository userRepository,
			IAccountRepository accountRepository,
			ILogger logger,
			IMapper mapper
		)
		{
			_userRepository = userRepository;
			_accountRepository = accountRepository;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin: UserService - DeleteAsync: {id}");
			var user = await _userRepository.GetUserByIdAsync(id);
			if (user == null)
			{
				return new ApiResponse<int>(404, 0, "User not found");
			}

			var account = await _accountRepository.GetAccountByIdAsync(user.AccountId); 
			if (account != null)
			{
				await _accountRepository.DeleteAccountAsync(account.Id);
			}

			await _userRepository.DeleteUserAsync(user.Id);
			_logger.Information($"End: UserService - DeleteAsync: {id} - Deletion successful");
			return new ApiResponse<int>(200, id, "User and associated account deleted successfully");
		}


		public async Task<ApiResponse<List<UserResponseDTO>>> GetAllAsync()
		{
			_logger.Information($"Begin: UserService - GetAllAsync");
			var users = await _userRepository.GetUsersAsync();
			var data = _mapper.Map<List<UserResponseDTO>>(users);
			_logger.Information($"End: UserService - GetAllAsync");
			return new ApiResponse<List<UserResponseDTO>>(200, data, "Data retrieved successfully");
		}

		public async Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id)
		{
			_logger.Information($"Begin: UserService - GetByIdAsync");
			var user = await _userRepository.GetUserByIdAsync(id);
			if (user == null)
			{
				return new ApiResponse<UserResponseDTO>(404, null, "User not found");
			}
			var data = _mapper.Map<UserResponseDTO>(user);
			_logger.Information($"End: UserService - GetByIdAsync");
			return new ApiResponse<UserResponseDTO>(200, data, "User data retrieved successfully");
		}

		public async Task<ApiResponse<UserResponseDTO>> InsertAsync(UserRequestDTO item)
		{
			_logger.Information($"Begin: UserService - InsertAsync");
			var existingUser = await _userRepository.FindByCondition(c => c.AccountId == item.AccountId).FirstOrDefaultAsync();
			if (existingUser != null)
			{
				return new ApiResponse<UserResponseDTO>(400, null, "Account is already assigned to another user");
			}

			var userEntity = _mapper.Map<User>(item);
			var newId = await _userRepository.CreateAsync(userEntity);

			var createdUser = await _userRepository.GetUserByIdAsync(newId);
			var userDto = _mapper.Map<UserResponseDTO>(createdUser);
			_logger.Information($"End: UserService - InsertAsync");
			return new ApiResponse<UserResponseDTO>(200, userDto, "Creation successful");
		}

		public async Task<ApiResponse<UserResponseDTO>> UpdateAsync(UserRequestDTO item)
		{
			_logger.Information($"Begin: UserService - UpdateAsync");
			var user = await _userRepository.GetUserByIdAsync(item.Id.Value);
			if (user == null)
			{
				return new ApiResponse<UserResponseDTO>(404, null, "User not found");
			}

			if (await _userRepository.FindByCondition(c => c.AccountId.Equals(item.AccountId) && !c.Id.Equals(item.Id)).FirstOrDefaultAsync() != null)
			{
				return new ApiResponse<UserResponseDTO>(400, null, "Account is already assigned to another user");
			}

			user = _mapper.Map<User>(item);
			var result = await _userRepository.UpdateAsync(user);
			if (result > 0)
			{
				return new ApiResponse<UserResponseDTO>(200, _mapper.Map<UserResponseDTO>(user), "Update successful");
			}

			_logger.Information($"End: UserService - UpdateAsync - An error occurred");
			return new ApiResponse<UserResponseDTO>(500, null, "An error occurred during the update");
		}
	}
}
