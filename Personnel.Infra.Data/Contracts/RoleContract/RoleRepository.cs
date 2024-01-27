using Personnel.Domain.Entities.Identity;
using Personnel.Infra.Data.Context;
using Personnel.Infra.Data.Interfaces.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Contracts.RoleContract
{
    public class RoleRepository : BaseAsyncRepository<Roles>, IRoleRepository
    {
        public RoleRepository(PersonnelDbContext dbContext) : base(dbContext)
        {
        }

        public void RemoveRole(int id)
        {
            var role = Entities.Find(id);
            if (role != null)
                Entities.Remove(role);
        }

        public void AddRole(Roles role)
        {
            Entities.Add(role);
        }

        public void UpdateRole(Roles role)
        {
            Entities.Update(role);
        }

        public Task AddRoleAsync(Roles role)
        {
            Entities.AddAsync(role);
            return Task.CompletedTask;
        }
    }
}
