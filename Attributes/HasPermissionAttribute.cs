using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingApi.Services.Interfaces;
using BookingApi.DTO;
using BookingApi.Models;

namespace BookingApi.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HasPermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public int PermissionId { get; }
        public ActionType Action { get; }
        public IConfiguration _config;
        public IRoleService _roleService;

        public HasPermissionAttribute(int permissionId, ActionType action, IConfiguration config, IRoleService roleService)
        {
            PermissionId = permissionId;
            Action = action;
            _config = config;
            _roleService = roleService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var bearerToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(bearerToken))
            {
                var decodedToken = ValidateAndDecodeJwt(bearerToken);

                if (decodedToken != null)
                {
                    // Kiểm tra quyền người dùng từ token
                    var roleId = decodedToken.Claims.Where(c => c.Type == "Role").Select(c => c.Value);

                    List<RoleDetailDTO> roleDetails = await _roleService.GetRoleDetailAllByRoleId(Convert.ToInt32(roleId));

                    foreach (RoleDetailDTO roleDetail in roleDetails)
                    {
                        if (roleDetail.PermissionId == PermissionId && roleDetail.Status && roleDetail.ActionName == Action)
                        {
                            return;
                        }
                    }

                    context.Result = new ForbidResult();
                    return;
                }
                else
                {
                    context.Result = new UnauthorizedResult(); // Xác thực token thất bại
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedResult(); // Không tìm thấy bearer token
                return;
            }
        }

        private JwtSecurityToken ValidateAndDecodeJwt(string token)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]!);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return (JwtSecurityToken)validatedToken;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu xác thực thất bại
                // Ví dụ: log lỗi, trả về null, hoặc ném ngoại lệ tùy thuộc vào yêu cầu của bạn
                return null;
            }
        }
    }
}
