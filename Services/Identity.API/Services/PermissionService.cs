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
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public PermissionService(IPermissionRepository permissionRepository,
            IMapper mapper,
            ILogger logger) 
        {
            this._permissionRepository = permissionRepository;
            this._mapper = mapper;  
            this._logger = logger;
        }
        public async Task<ApiResponse<int>> DeleteAsync(int id)
        {
            _logger.Information($"Begin : PermissionService - DeleteAsync : {id}");
            var permission = await _permissionRepository.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                return new ApiResponse<int>(404, 0, "Không tìm thấy quyền cần xóa");
            }
            _permissionRepository.Delete(permission);
            var result = await _permissionRepository.SaveChangesAsync();
            if (result > 0)
            {
                _logger.Information($"End : PermissionService - DeleteAsync : {id} - Xóa thất bại");
                return new ApiResponse<int>(200, result, "Xóa quyền thành công");
            }
            else
            {
                _logger.Information($"End : PermissionService - DeleteAsync : {id} - Xóa thất bại");
                return new ApiResponse<int>(400, result, "Xóa quyền thất bại");

            }
        }

        public async Task<ApiResponse<List<PermissionResponseDTO>>> GetAllAsync()
        {
            _logger.Information($"Begin : PermissionService - GetAllAsync");
            var permissions = await _permissionRepository.FindAll(false).ToListAsync();
            _logger.Information($"Mapping list of permission to dto");
            var data = _mapper.Map<List<PermissionResponseDTO>>(permissions);
            _logger.Information($"End : PermissionService - GetAllAsync");
            return new ApiResponse<List<PermissionResponseDTO>>(200, data, "Lấy dữ liệu thành công");
        }

        public async Task<ApiResponse<PermissionResponseDTO>> GetByIdAsync(int id)
        {
            _logger.Information($"Begin : PermissionService - GetByIdAsync");

            var permission = await _permissionRepository.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                return new ApiResponse<PermissionResponseDTO>(404, null, "Không tìm thấy quyền");
            }
            var data = _mapper.Map<PermissionResponseDTO>(permission);
            _logger.Information($"End : PermissionService - GetByIdAsync");
            return new ApiResponse<PermissionResponseDTO>(200, data, "Lấy dữ liệu quyền thành công");
        }

        public async Task<ApiResponse<int>> InsertAsync(PermissionRequestDTO item)
        {
            _logger.Information($"Begin : PermissionService - InsertAsync");
            var checkPermissionName = await _permissionRepository.GetPermissionByNameAsync(item.Name);
            if (checkPermissionName != null)
            {
                return new ApiResponse<int>(400, -1, "Tên quyền đã tồn tại");
            }
            var permissionEntity = _mapper.Map<Permission>(item);
            var newId = await _permissionRepository.CreateAsync(permissionEntity);
            _logger.Information($"End : PermissionService - CreateAsync");
            return new ApiResponse<int>(200, newId, "Tạo thành công");
        }

        public async Task<ApiResponse<PermissionResponseDTO>> UpdateAsync(PermissionRequestDTO item)
        {
            _logger.Information($"Begin : PermissionService - UpdateAsync");
            var permission = await _permissionRepository.FindByCondition(c => c.Id.Equals(item.Id)).FirstOrDefaultAsync();
            if (permission == null)
            {
                return new ApiResponse<PermissionResponseDTO>(404, null, "Không tìm thấy quyền");
            }
            if (await _permissionRepository.FindByCondition(c => c.Name.Equals(item.Name) && !c.Id.Equals(item.Id)).FirstOrDefaultAsync() != null)
            {
                return new ApiResponse<PermissionResponseDTO>(400, null, "Tên quyền đã tồn tại");
            }
            permission = _mapper.Map<Permission>(item);
            var result = await _permissionRepository.UpdateAsync(permission);
            if (result > 0)
            {
                return new ApiResponse<PermissionResponseDTO>(200, _mapper.Map<PermissionResponseDTO>(permission), "Cập nhật thành công");
            }
            _logger.Information($"End : PermissionService - UpdateAsync");
            return new ApiResponse<PermissionResponseDTO>(200, null, "Có lỗi xảy ra");
        }
    }
}
