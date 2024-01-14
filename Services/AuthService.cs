using BookingApi.DTO;
using BookingApi.Services.Interfaces;
using BookingApi.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IAccountRepository accountRepository, IUserRepository userRepository, IConfiguration config)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _config = config;
        }

        public Task<bool> GetProfile(AuthLoginDTO item)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Login(AuthLoginDTO login)
        {
            var account = await _accountRepository.GetByEmail(login.Email);

            if (account == null || !account.Password.Equals(login.Password))
            {
                return null; // Invalid login credentials
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Role.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(token);

            tokenDescriptor.Expires = DateTime.UtcNow.AddDays(1);
            token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = tokenHandler.WriteToken(token);

            return new { accessToken, refreshToken };
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]!);

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
                var account = await _accountRepository.GetById(int.Parse(accountId!));

                if (account == null)
                {
                    // Account not found, return null or handle as needed
                    return null;
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
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var newToken = tokenHandler.CreateToken(tokenDescriptor);
                var newAccessToken = tokenHandler.WriteToken(newToken);

                return newAccessToken;
            }
            catch (Exception)
            {
                // Token validation failed, return null or handle as needed
                return null;
            }
        }

        public async Task<bool> Register(AuthRegisterDTO item)
        {
            // Kiểm tra xem người dùng đã tồn tại hay chưa
            var existingAccount = await _accountRepository.GetByEmail(item.Email);

            if (existingAccount != null)
            {
                // Người dùng đã tồn tại, trả về false hoặc xử lý tùy thuộc vào yêu cầu của bạn
                return false;
            }

            // Tạo một tài khoản mới từ thông tin đăng ký
            var newAccount = new AccountRequestDTO
            {
                Email = item.Email,
                Password = HashPassword(item.Password), // Hàm HashPassword cần được triển khai để bảo mật mật khẩu
            };

            // Lưu tài khoản mới vào cơ sở dữ liệu
            (var isSuccess, var id) = await _accountRepository.Insert(newAccount);

            if (isSuccess && id >= 0)
            {
                var newUser = new UserRequestDTO
                {
                    Fullname = item.Fullname,
                    BirthDate = item.BirthDate,
                    Country = item.Country,
                    Phone = item.Phone,
                    Gender = item.Gender,
                    AccountId = id
                };

                (isSuccess, id) = await _userRepository.Insert(newUser);
            }

            // Trả về true nếu đăng ký thành công, ngược lại trả về false
            return isSuccess;
        }

        // Hàm để băm mật khẩu (hash password) - Bạn cần triển khai hàm này để bảo mật mật khẩu
        private string HashPassword(string password)
        {
            // Thực hiện băm mật khẩu ở đây, có thể sử dụng các thư viện như BCrypt.NET, Argon2, ...
            // Đây chỉ là một ví dụ đơn giản, không nên sử dụng trong môi trường thực tế
            return password;
        }
    }
}