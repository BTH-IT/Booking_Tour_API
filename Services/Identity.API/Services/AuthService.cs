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
            IRoleRepository roleRepository) {
            this._roleRepository = roleRepository;
            this._userRepository = userRepository;
            this._accountRepository = accountRepository;
            this._configuration = configuration;
            this._logger = logger;
            this._mapper = mapper;
        }
        public async Task<ApiResponse<AuthResponseDTO>> LoginAsync(AuthLoginDTO loginDTO)
        {
            var account = await _accountRepository.FindByCondition(c=>c.Email.Equals(loginDTO.Email),false,c=>c.Role, c => c.Role.RoleDetails).FirstOrDefaultAsync();

            if (account == null || !account.Password.Equals(loginDTO.Password))
            {
                return new ApiResponse<AuthResponseDTO>(400, null, "Thông tin đăng nhập không đúng");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Role.Id.ToString()),
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
                RefreshToken = refreshToken
            },"Đăng nhập thành công");
        }

        public async Task<ApiResponse<string>> RefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]!);

            try
            {
                // Validate and parse the refresh token
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                // Extract account information from the principal
                var accountId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                var roleId = principal.FindFirst(ClaimTypes.Role)?.Value;

                // Retrieve the account from the database using the extracted information
                var account = await _accountRepository.FindByCondition(c=>c.Id.Equals(int.Parse(accountId!)),false,c=>c.Role,c =>c.Role.RoleDetails).FirstOrDefaultAsync();

                if (account == null)
                {
                    // Account not found, return null or handle as needed
                    return new ApiResponse<string>(401,"","Không tìm thấy tài khoản");
                }

                // Generate a new access token
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

                return new ApiResponse<string>(200,newAccessToken,"Tạo mới token thành công");
            }
            catch (Exception ex)
            {
                // Token validation failed, return null or handle as needed
                    return new ApiResponse<string>(401,ex.Message,"Tạo mới token không thành công");
            }
        }

        public async Task<ApiResponse<bool>> RegisterAsync(AuthRegisterDTO registerDTO)
        {
            var checkEmailAccount = await _accountRepository.GetAccountByEmailAsync(registerDTO.Email);
            if(checkEmailAccount != null)
            {
                return new ApiResponse<bool>(400,false,"Email đã tồn tại");
            }
            var userRole = await _roleRepository.FindByCondition(c => c.RoleName.Equals(Constants.Role.User)).FirstOrDefaultAsync();
            var account = new Account()
            {
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                RoleId = userRole.Id
            };
            var newAccountId =  await _accountRepository.CreateAsync(account);
            if (newAccountId <= 0)
            {
                return new ApiResponse<bool>(400, false, "Có lỗi xảy ra trong quá trình đăng kí");
            }
            var user = new User()
            {
                Fullname = registerDTO.Fullname,
                BirthDate = registerDTO.BirthDate,
                Country = registerDTO.Country,
                Phone = registerDTO.Phone,
                Gender = registerDTO.Gender,
                AccountId = newAccountId,
                CreatedAt = DateTime.Now,
            };
            var newUserId = await _userRepository.CreateAsync(user);
            if (newAccountId <= 0)
            {
                return new ApiResponse<bool>(400, false, "Có lỗi xảy ra trong quá trình đăng kí");
            }
            return new ApiResponse<bool>(200, true, "Đăng kí thành công");
        }
    }
}
