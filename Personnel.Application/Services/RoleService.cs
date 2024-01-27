using AutoMapper;
using Personnel.Application.Interfaces;
using Personnel.Domain.Core.Contracts;
using Personnel.Domain.Dtos.Roles;
using Personnel.Domain.Dtos;
using Personnel.Domain.Entities.Identity;
using Personnel.Domain.Security;
using Personnel.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Personnel.Domain.Core.Exceptions;
using AutoMapper.QueryableExtensions;
using Personnel.Application.Helpers;
namespace Personnel.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task DeleteAsync(int id)
        {

            _unitOfWork.RoleRepository.RemoveRole(id);
            await _unitOfWork.CommitAsync();
        }

        public Roles GetById(int id)
        {
            return _unitOfWork.RoleRepository.TableNoTracking.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateRole(Roles role)
        {
            _unitOfWork.RoleRepository.UpdateRole(role);
            _unitOfWork.CommitAsync();
        }

        public async Task CreateAsync(RoleDetailDto dto)
        {
            var role = _mapper.Map<Roles>(dto);

            role.PermissionInRoles = dto.PermisonRecordIds.Select(permissionRecordId => new PermissionInRole { PermissionRecordId = permissionRecordId }).ToList();

            await _unitOfWork.RoleRepository.AddRoleAsync(role);

            await _unitOfWork.CommitAsync();

            dto.Id = role.Id;
        }

        public async Task<RoleDetailDto> FindAsync(int id)
        {
            var role = await _unitOfWork.RoleRepository.TableNoTracking.ProjectTo<RoleDetailDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
                throw new NotFoundException();

            return role;
        }

        public void Insert(Roles role)
        {
            _unitOfWork.RoleRepository.AddRole(role);
            _unitOfWork.CommitAsync();
        }

        public List<Roles> GetAll()
        {
            return _unitOfWork.RoleRepository.TableNoTracking.ToList();
        }


        public Roles GetBySystemName(string systemName)
        {
            var role = _unitOfWork.RoleRepository.TableNoTracking.FirstOrDefault(x => x.SystemName == systemName);

            if (role == null)
                throw new NotFoundException();

            return role;
        }

        public IList<Roles> GetUserRolesofRoleGroup(RoleGroup roleGroup)
        {
            var query = _unitOfWork.RoleRepository.TableNoTracking.Where(x => x.RoleGroup == roleGroup);

            return query.ToList();

        }

        public IList<Roles> GetUserRolesofRoleGroupWithUser(RoleGroup roleGroup)
        {
            var query = _unitOfWork.RoleRepository.TableNoTracking.Include(x => x.UserInRoles).Where(x => x.RoleGroup == roleGroup);

            return query.ToList();

        }

        public string UserRole(List<int> roleIds)
        {
            var roles = _unitOfWork.RoleRepository.TableNoTracking.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name).ToList();

            return string.Join(" | ", roles);

        }

        public Roles GetByIdAndRoleGroup(int id, RoleGroup roleGroup)
        {
            var query = _unitOfWork.RoleRepository.TableNoTracking.FirstOrDefault(p => p.Id == id && p.RoleGroup == roleGroup);
            return query;
        }
        public PagedList<RoleDto> GetRolesPagination(PageFilterDto command)
        {
            var query = _unitOfWork.RoleRepository.TableNoTracking.Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name,
                SystemName = x.SystemName,
                Active = x.Active,
                IsSystemRole = x.IsSystemRole,

            });
            return new PagedList<RoleDto>(query.DtoOrderByCommand(command), command.PageIndex, command.PageSize);

        }

    }

}
