using AutoMapper;
using Identity.API.Entites;
using Identity.API.Repositories.Interfaces;
using Identity.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.DTOs;
using Shared.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ILogger = Serilog.ILogger;
using System.Security.Claims;
using Shared.Enums;

namespace Identity.API.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public AuthService(IUserRepository userRepository,
			IAccountRepository accountRepository,
			ILogger logger,
			IMapper mapper,
			IConfiguration configuration,
			IRoleRepository roleRepository)
		{
			this._roleRepository = roleRepository;
			this._userRepository = userRepository;
			this._accountRepository = accountRepository;
			this._configuration = configuration;
			this._logger = logger;
			this._mapper = mapper;
		}

		public async Task<ApiResponse<AuthResponseDTO>> LoginAsync(AuthLoginDTO loginDTO)
		{
			var account = await _accountRepository.FindByCondition(c => c.Email.Equals(loginDTO.Email), false, c => c.Role, c => c.Role.RoleDetails).FirstOrDefaultAsync();

			if (account == null || !account.Password.Equals(loginDTO.Password))
			{
				return new ApiResponse<AuthResponseDTO>(400, null, "Invalid login credentials");
			}

			var user = await _userRepository.FindByCondition(c => c.AccountId.Equals(account.Id), false).FirstOrDefaultAsync();

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]!);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
					new Claim(ClaimTypes.Email, account.Email),
					new Claim(ClaimTypes.Role, account.Role!.Id.ToString()),
					new Claim(ClaimTypes.PrimarySid,user!.Id.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(20),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var accessToken = tokenHandler.WriteToken(token);

			tokenDescriptor.Expires = DateTime.UtcNow.AddMonths(2);
			token = tokenHandler.CreateToken(tokenDescriptor);
			var refreshToken = tokenHandler.WriteToken(token);

			return new ApiResponse<AuthResponseDTO>(200, new AuthResponseDTO()
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken,
				Account = _mapper.Map<AccountResponseDTO>(account),
				User = _mapper.Map<UserResponseDTO>(user),
			}, "Login successful");
		}

		public async Task<ApiResponse<string>> RefreshToken(string refreshToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]!);

			try
			{
				var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out _);

				var accountId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var email = principal.FindFirst(ClaimTypes.Email)?.Value;
				var roleId = principal.FindFirst(ClaimTypes.Role)?.Value;

				var account = await _accountRepository.FindByCondition(c => c.Id.Equals(int.Parse(accountId!)), false, c => c.Role, c => c.Role.RoleDetails).FirstOrDefaultAsync();

				if (account == null)
				{
					return new ApiResponse<string>(401, "", "Account not found");
				}

				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
						new Claim(ClaimTypes.Email, account.Email),
						new Claim(ClaimTypes.Role, account.Role.Id.ToString()),
					}),
					Expires = DateTime.UtcNow.AddDays(29),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};

				var newToken = tokenHandler.CreateToken(tokenDescriptor);
				var newAccessToken = tokenHandler.WriteToken(newToken);

				return new ApiResponse<string>(200, newAccessToken, "Token refreshed successfully");
			}
			catch (Exception ex)
			{
				return new ApiResponse<string>(401, ex.Message, "Token refresh failed");
			}
		}
		public async Task<ApiResponse<UserResponseDTO>> RegisterAsync(AuthRegisterDTO registerDTO)
		{
			var emailCheckResponse = await CheckEmailExists(registerDTO.Email);
			if (emailCheckResponse != null) return emailCheckResponse;

			var userRole = await GetUserRoleAsync();
			if (userRole == null) return new ApiResponse<UserResponseDTO>(400, null, "User role not found");

			var newAccountId = await CreateAccountAsync(registerDTO, userRole.Id);
			if (newAccountId <= 0) return new ApiResponse<UserResponseDTO>(400, null, "Registration failed");

			var newUserId = await CreateUserAsync(registerDTO, newAccountId);
			if (newUserId <= 0) return new ApiResponse<UserResponseDTO>(400, null, "Registration failed");

			var userDTO = await MapUserToDTOAsync(newUserId, newAccountId, registerDTO, userRole.Id);
			return new ApiResponse<UserResponseDTO>(200, userDTO, "Registration successful");
		}

		private async Task<ApiResponse<UserResponseDTO>> CheckEmailExists(string email)
		{
			var checkEmailAccount = await _accountRepository.GetAccountByEmailAsync(email);
			if (checkEmailAccount != null)
			{
				return new ApiResponse<UserResponseDTO>(400, null, "Email already exists");
			}
			return null;
		}

		private async Task<Role> GetUserRoleAsync()
		{
			return await _roleRepository.FindByCondition(c => c.RoleName.Equals(Constants.Role.User)).FirstOrDefaultAsync();
		}

		private async Task<int> CreateAccountAsync(AuthRegisterDTO registerDTO, int roleId)
		{
			var account = new Account()
			{
				Email = registerDTO.Email,
				Password = registerDTO.Password,
				RoleId = roleId
			};
			return await _accountRepository.CreateAsync(account);
		}

		private async Task<int> CreateUserAsync(AuthRegisterDTO registerDTO, int accountId)
		{
			var user = new User()
			{
				Fullname = registerDTO.Fullname,
				BirthDate = registerDTO.BirthDate,
				Country = registerDTO.Country,
				Phone = registerDTO.Phone,
				Gender = registerDTO.Gender,
				AccountId = accountId,
				CreatedAt = DateTime.Now,
			};
			return await _userRepository.CreateAsync(user);
		}

		private async Task<UserResponseDTO> MapUserToDTOAsync(int userId, int accountId, AuthRegisterDTO registerDTO, int roleId)
		{
			var user = await _userRepository.GetByIdAsync(userId); 
			var userDTO = _mapper.Map<UserResponseDTO>(user);

			var role = await _roleRepository.GetRoleByIdAsync(roleId);
			var roleDetails = await _roleRepository.GetRoleDetailsByRoleIdAsync(role.Id); 

			userDTO.Account = new AccountResponseDTO
			{
				Id = accountId,
				Email = registerDTO.Email,
				Role = new RoleResponseDTO
				{
					Id = role.Id,
					RoleName = role.RoleName,
					Status = role.Status,
					RoleDetails = roleDetails.Select(rd => new RoleDetailDTO
					{
						RoleId = rd.RoleId,
						PermissionId = rd.PermissionId,
						ActionName = (ActionType)rd.ActionName, 
						Status = rd.Status
					}).ToList()
				}
			};

			return userDTO;
		}
	}
}
