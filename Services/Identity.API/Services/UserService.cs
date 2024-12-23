﻿using AutoMapper;
using Identity.API.Entites;
using Identity.API.Repositories;
using Identity.API.Repositories.Interfaces;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.DTOs;
using Shared.Helper;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ILogger = Serilog.ILogger;

namespace Identity.API.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;
		private readonly IDistributedCache _cache;

		public UserService(
			IUserRepository userRepository,
			IAccountRepository accountRepository,
			ILogger logger,
			IMapper mapper,
			IDistributedCache cache
		)
		{
			_userRepository = userRepository;
			_accountRepository = accountRepository;
			_logger = logger;
			_mapper = mapper;
			_cache = cache;
		}

		public async Task<ApiResponse<string>> ChanageUserPasswordAsync(int userId, ChangeUserPasswordRequestDto dto)
		{
			var user = await _userRepository.GetByIdAsync(userId, c => c.Account);
			if (user == null)
				return new ApiResponse<string>(200, "", $"Không tìm thấy người dùng với id : {userId}");

			if (user.Account!.Password != dto.CurrentPassword)
			{
				return new ApiResponse<string>(400, "", $"Mật khẩu không chính xác");
			}
			user.Account.Password = dto.NewPassword;
			var result = await _accountRepository.UpdateAsync(user.Account);

			if (result > 0)
			{
				// Invalidate cache
				await _cache.RemoveAsync($"User_{userId}");
				await _cache.RemoveAsync("User_All");
				return new ApiResponse<string>(200, "", "Đổi mật khẩu thành công");
			}
			return new ApiResponse<string>(400, "", "Đổi mật khẩu thất bại");
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

			// Invalidate cache
			await _cache.RemoveAsync($"User_{id}");
			await _cache.RemoveAsync("User_All");

			_logger.Information($"End: UserService - DeleteAsync: {id} - Deletion successful");
			return new ApiResponse<int>(200, id, "User and associated account deleted successfully");
		}

		public async Task<ApiResponse<List<UserResponseDTO>>> GetAllAsync()
		{
			_logger.Information($"Begin: UserService - GetAllAsync");

			var cacheKey = "User_All";
			var cachedData = await _cache.GetStringAsync(cacheKey);

			if (!string.IsNullOrEmpty(cachedData))
			{
				var CachedResponse = JsonSerializer.Deserialize<List<UserResponseDTO>>(cachedData);

				_logger.Information($"End: UserService - GetAllAsync");
				return new ApiResponse<List<UserResponseDTO>>(200, CachedResponse, "Data retrieved successfully", true);
			}

			var users = await _userRepository.GetUsersAsync();
			var data = _mapper.Map<List<UserResponseDTO>>(users);

			// Cache the data
			var cacheOptions = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
			};
			await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), cacheOptions);

			_logger.Information($"End: UserService - GetAllAsync");
			return new ApiResponse<List<UserResponseDTO>>(200, data, "Data retrieved successfully");
		}

		public async Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id)
		{
			_logger.Information($"Begin: UserService - GetByIdAsync");

			var cacheKey = $"User_{id}";
			var cachedData = await _cache.GetStringAsync(cacheKey);

			if (!string.IsNullOrEmpty(cachedData))
			{
				var CachedResponse = JsonSerializer.Deserialize<UserResponseDTO>(cachedData);

				_logger.Information($"End: UserService - GetByIdAsync");
				return new ApiResponse<UserResponseDTO>(200, CachedResponse, "User data retrieved successfully");
			}

			var user = await _userRepository.GetUserByIdAsync(id);
			if (user == null)
			{
				return new ApiResponse<UserResponseDTO>(404, null, "User not found");
			}
			var data = _mapper.Map<UserResponseDTO>(user);

			// Cache the data
			var cacheOptions = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
			};
			await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), cacheOptions);

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

			// Invalidate cache
			await _cache.RemoveAsync("User_All");

			_logger.Information($"End: UserService - InsertAsync");
			return new ApiResponse<UserResponseDTO>(200, userDto, "Creation successful");
		}

		public async Task<ApiResponse<UserResponseDTO>> UpdateAsync(int id, UpdateUserRequestDTO item)
		{
			_logger.Information($"Begin: UserService - UpdateAsync");
			var user = await _userRepository.GetUserByIdAsync(id);
			var userId = user.Id;
			var accountId = user.AccountId;
			if (user == null)
			{
				return new ApiResponse<UserResponseDTO>(404, null, "User not found");
			}

			user = _mapper.Map<User>(item);
			user.Id = userId;
			user.AccountId = accountId;

			var result = await _userRepository.UpdateAsync(user);
			if (result > 0)
			{
				// Invalidate cache
				await _cache.RemoveAsync($"User_{id}");
				await _cache.RemoveAsync("User_All");

				return new ApiResponse<UserResponseDTO>(200, _mapper.Map<UserResponseDTO>(user), "Update successful");
			}

			_logger.Information($"End: UserService - UpdateAsync - An error occurred");
			return new ApiResponse<UserResponseDTO>(500, null, "An error occurred during the update");
		}
	}
}
