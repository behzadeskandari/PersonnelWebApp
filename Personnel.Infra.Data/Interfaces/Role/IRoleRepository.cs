using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Interfaces.Role
{
    public interface IRoleRepository : IBaseRepository<Roles>
    {
        void RemoveRole(int id);
        void AddRole(Roles role);
        void UpdateRole(Roles role);
        Task AddRoleAsync(Roles role);
    }
}
