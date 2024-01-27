using Personnel.Domain.Core.Contracts;
using Personnel.Domain.Core.Interfaces;
using Personnel.Domain.Dtos;
using Personnel.Domain.Dtos.Roles;
using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Application.Interfaces
{
    public interface IRoleService
    {
        Task DeleteAsync(int id);
        Roles GetById(int id);
        void UpdateRole(Roles role);
        Task CreateAsync(RoleDetailDto dto);

        Task<RoleDetailDto> FindAsync(int id);

        void Insert(Roles role);

        List<Roles> GetAll();


        Roles GetBySystemName(string systemName);

        IList<Roles> GetUserRolesofRoleGroup(RoleGroup roleGroup);

        IList<Roles> GetUserRolesofRoleGroupWithUser(RoleGroup roleGroup);
        string UserRole(List<int> roleIds);

        Roles GetByIdAndRoleGroup(int id, RoleGroup roleGroup);
        PagedList<RoleDto> GetRolesPagination(PageFilterDto command);
    }
}
