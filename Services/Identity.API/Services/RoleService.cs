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
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _roleRepository;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public RoleService(
			IRoleRepository roleRepository,
			ILogger logger,
			IMapper mapper
		)
		{
			this._roleRepository = roleRepository;
			this._logger = logger;
			this._mapper = mapper;
		}

		public async Task<ApiResponse<int>> DeleteAsync(int id)
		{
			_logger.Information($"Begin : RoleService - DeleteAsync : {id}");
			var role = await _roleRepository.GetRoleByIdAsync(id);
			if (role == null)
			{
				return new ApiResponse<int>(404, 0, "Role not found");
			}
			if (role.RoleName == "Admin" || role.RoleName == "User")
			{
				return new ApiResponse<int>(400, 0, "Cannot delete Admin or User role");
			}
			await _roleRepository.DeleteRoleDetailByRoleIdAsync(role.Id);
			_roleRepository.Delete(role);

			var result = await _roleRepository.SaveChangesAsync();
			if (result > 0)
			{
				_logger.Information($"End : RoleService - DeleteAsync : {id} - Deletion successful");
				return new ApiResponse<int>(200, result, "Role deleted successfully");
			}
			else
			{
				_logger.Information($"End : RoleService - DeleteAsync : {id} - Deletion failed");
				return new ApiResponse<int>(400, result, "Role deletion failed");
			}
		}

		public async Task<ApiResponse<List<RoleResponseDTO>>> GetAllAsync()
		{
			_logger.Information($"Begin : RoleService - GetAllAsync");
			var roles = await _roleRepository.FindAll(false, c => c.RoleDetails).ToListAsync();
			_logger.Information($"Mapping list of roles to DTO");
			var data = _mapper.Map<List<RoleResponseDTO>>(roles);
			_logger.Information($"End : RoleService - GetAllAsync");
			return new ApiResponse<List<RoleResponseDTO>>(200, data, "Data retrieved successfully");
		}

		public async Task<ApiResponse<RoleResponseDTO>> GetByIdAsync(int id)
		{
			_logger.Information($"Begin : RoleService - GetByIdAsync");

			var role = await _roleRepository.GetByIdAsync(id, c => c.RoleDetails);
			if (role == null)
			{
				return new ApiResponse<RoleResponseDTO>(404, null, "Role not found");
			}
			var data = _mapper.Map<RoleResponseDTO>(role);
			_logger.Information($"End : RoleService - GetByIdAsync");
			return new ApiResponse<RoleResponseDTO>(200, data, "Role data retrieved successfully");
		}

		public async Task<ApiResponse<RoleResponseDTO>> InsertAsync(RoleRequestDTO item)
		{
			_logger.Information($"Begin : RoleService - InsertAsync");
			var checkRoleName = await _roleRepository.FindByCondition(c => c.RoleName == item.RoleName).FirstOrDefaultAsync();
			if (checkRoleName != null)
			{
				return new ApiResponse<RoleResponseDTO>(400, null, "Role name already exists");
			}
			var roleEntity = _mapper.Map<Role>(item);
			var newId = await _roleRepository.CreateAsync(roleEntity);

			var createdRole = await _roleRepository.GetByIdAsync(newId);
			var roleDto = _mapper.Map<RoleResponseDTO>(createdRole);

			_logger.Information($"End : RoleService - InsertAsync");
			return new ApiResponse<RoleResponseDTO>(200, roleDto, "Creation successful");
		}

		public async Task<ApiResponse<RoleResponseDTO>> UpdateAsync(RoleRequestDTO item)
		{
			_logger.Information($"Begin : RoleService - UpdateAsync");
			var role = await _roleRepository.FindByCondition(c => c.Id.Equals(item.Id)).FirstOrDefaultAsync();
			if (role == null)
			{
				return new ApiResponse<RoleResponseDTO>(404, null, "Role not found");
			}
			if (await _roleRepository.FindByCondition(c => c.RoleName.Equals(item.RoleName) && !c.Id.Equals(item.Id)).FirstOrDefaultAsync() != null)
			{
				return new ApiResponse<RoleResponseDTO>(400, null, "Role name already exists");
			}
			role = _mapper.Map<Role>(item);
			var result = await _roleRepository.UpdateAsync(role);
			if (result > 0)
			{
				return new ApiResponse<RoleResponseDTO>(200, _mapper.Map<RoleResponseDTO>(role), "Update successful");
			}
			_logger.Information($"End : RoleService - UpdateAsync");
			return new ApiResponse<RoleResponseDTO>(200, null, "An error occurred");
		}

		public async Task<ApiResponse<RoleDetailDTO>> UpdateRoleDetailByRoleIdAsync(string roleId, RoleDetailDTO item)
		{
			_logger.Information($"Begin : RoleService - UpdateRoleDetailByRoleIdAsync");
			var roleDetail = _mapper.Map<RoleDetail>(item);
			var data = await _roleRepository.UpdateRoleDetailByRoleIdAsync(item.RoleId, roleDetail);
			_logger.Information($"End : RoleService - UpdateRoleDetailByRoleIdAsync");

			return new ApiResponse<RoleDetailDTO>(200, _mapper.Map<RoleDetailDTO>(data), "Update successful");
		}
	}
}
