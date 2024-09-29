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
                return new ApiResponse<int>(404, 0, "Không tìm thấy người dùng cần xóa");
            }
            _userRepository.Delete(user);
            var result = await _userRepository.SaveChangesAsync();
            if (result > 0)
            {
                _logger.Information($"End : UserService - DeleteAsync : {id} - Xóa thất bại");
                return new ApiResponse<int>(200, result, "Xóa người dùng thành công");
            }
            else
            {
                _logger.Information($"End : UserService - DeleteAsync : {id} - Xóa thất bại");
                return new ApiResponse<int>(400, result, "Xóa người dùng thất bại");
            }
        }

        public async Task<ApiResponse<List<UserResponseDTO>>> GetAllAsync()
        {
            _logger.Information($"Begin : UserService - GetAllAsync");
            var users = await _userRepository.FindAll(false,c=>c.Account,c =>c.Account.Role,c =>c.Account.Role.RoleDetails).ToListAsync();
            _logger.Information($"Mapping list of user to dto");
            var data = _mapper.Map<List<UserResponseDTO>>(users);
            _logger.Information($"End : UserService - GetAllAsync");
            return new ApiResponse<List<UserResponseDTO>>(200, data, "Lấy dữ liệu thành công");
        }

        public async Task<ApiResponse<UserResponseDTO>> GetUserByIdAsync(int id)
        {
            _logger.Information($"Begin : UserService - GetByIdAsync");

            var user = await _userRepository.GetByIdAsync(id,c=>c.Account,c =>c.Account.Role,c=>c.Account.Role.RoleDetails);
            if (user == null)
            {
                return new ApiResponse<UserResponseDTO>(404, null, "Không tìm thấy người dùng");
            }
            var data = _mapper.Map<UserResponseDTO>(user);
            _logger.Information($"End : UserService - GetByIdAsync");
            return new ApiResponse<UserResponseDTO>(200, data, "Lấy dữ liệu người dùng thành công");
        }

        public async Task<ApiResponse<int>> InsertAsync(UserRequestDTO item)
        {
            _logger.Information($"Begin : UserService - InsertAsync");
            var checkAccount = await _userRepository.FindByCondition(c => c.AccountId == item.AccountId).FirstOrDefaultAsync();
            if (checkAccount != null)
            {
                return new ApiResponse<int>(400, -1, "Tài khoản đã được gán cho người dùng khác");
            }
            var userEntity = _mapper.Map<User>(item);
            var newId = await _userRepository.CreateAsync(userEntity);
            _logger.Information($"End : UserService - CreateAsync");
            return new ApiResponse<int>(200, newId, "Tạo thành công");
        }

        public async Task<ApiResponse<UserResponseDTO>> UpdateAsync(UserRequestDTO item)
        {
            _logger.Information($"Begin : UserService - UpdateAsync");
            var role = await _userRepository.FindByCondition(c => c.Id.Equals(item.Id)).FirstOrDefaultAsync();
            if (role == null)
            {
                return new ApiResponse<UserResponseDTO>(404, null, "Không tìm thấy vai trò");
            }
            if (await _userRepository.FindByCondition(c => c.AccountId.Equals(item.AccountId) && !c.Id.Equals(item.Id)).FirstOrDefaultAsync() != null)
            {
                return new ApiResponse<UserResponseDTO>(400, null, "Tài khoản đã được gán cho người dùng khác");
            }
            role = _mapper.Map<User>(item);
            var result = await _userRepository.UpdateAsync(role);
            if (result > 0)
            {
                return new ApiResponse<UserResponseDTO>(200, _mapper.Map<UserResponseDTO>(role), "Cập nhật thành công");
            }
            _logger.Information($"End : UserService - UpdateAsync");
            return new ApiResponse<UserResponseDTO>(200, null, "Có lỗi xảy ra");
        }
    }
}
