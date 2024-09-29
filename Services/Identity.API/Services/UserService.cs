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
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public UserService(
			IUserRepository userRepository,
			ILogger logger,
			IMapper mapper
		)
		{
			this._userRepository = userRepository;
			this._logger = logger;
			this._mapper = mapper;
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin : UserService - DeleteAsync : {id}");
			var user = await _userRepository.GetUserByIdAsync(id);
			if (user == null)
			{
				return new ApiResponse<int>(404, 0, "User not found");
			}
			_userRepository.Delete(user);
			var result = await _userRepository.SaveChangesAsync();
			if (result > 0)
			{
				_logger.Information($"End : UserService - DeleteAsync : {id} - Deletion successful");
				return new ApiResponse<int>(200, result, "User deleted successfully");
			}
			else
			{
				_logger.Information($"End : UserService - DeleteAsync : {id} - Deletion failed");
				return new ApiResponse<int>(400, result, "User deletion failed");
			}
		}

		public async Task<ApiResponse<List<UserResponseDTO>>> GetAllAsync()
		{
			_logger.Information($"Begin : UserService - GetAllAsync");
			var users = await _userRepository.FindAll(false, c => c.Account, c => c.Account.Role, c => c.Account.Role.RoleDetails).ToListAsync();
			_logger.Information($"Mapping list of users to DTO");
			var data = _mapper.Map<List<UserResponseDTO>>(users);
			_logger.Information($"End : UserService - GetAllAsync");
			return new ApiResponse<List<UserResponseDTO>>(200, data, "Data retrieved successfully");
		}

		public async Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id)
		{
			_logger.Information($"Begin : UserService - GetByIdAsync");

			var user = await _userRepository.GetByIdAsync(id, c => c.Account, c => c.Account.Role, c => c.Account.Role.RoleDetails);
			if (user == null)
			{
				return new ApiResponse<UserResponseDTO>(404, null, "User not found");
			}
			var data = _mapper.Map<UserResponseDTO>(user);
			_logger.Information($"End : UserService - GetByIdAsync");
			return new ApiResponse<UserResponseDTO>(200, data, "User data retrieved successfully");
		}

		public async Task<ApiResponse<UserResponseDTO>> InsertAsync(UserRequestDTO item)
		{
			_logger.Information($"Begin : UserService - InsertAsync");
			var checkAccount = await _userRepository.FindByCondition(c => c.AccountId == item.AccountId).FirstOrDefaultAsync();
			if (checkAccount != null)
			{
				return new ApiResponse<UserResponseDTO>(400, null, "Account is already assigned to another user");
			}
			var userEntity = _mapper.Map<User>(item);
			var newId = await _userRepository.CreateAsync(userEntity);

			var createdUser = await _userRepository.GetByIdAsync(newId, c => c.Account, c => c.Account.Role, c => c.Account.Role.RoleDetails);
			var userDto = _mapper.Map<UserResponseDTO>(createdUser);

			_logger.Information($"End : UserService - InsertAsync");
			return new ApiResponse<UserResponseDTO>(200, userDto, "Creation successful");
		}

		public async Task<ApiResponse<UserResponseDTO>> UpdateAsync(UserRequestDTO item)
		{
			_logger.Information($"Begin : UserService - UpdateAsync");
			var user = await _userRepository.FindByCondition(c => c.Id.Equals(item.Id)).FirstOrDefaultAsync();
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
			_logger.Information($"End : UserService - UpdateAsync");
			return new ApiResponse<UserResponseDTO>(200, null, "An error occurred");
		}
	}
}
