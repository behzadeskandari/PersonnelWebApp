using Personnel.Infra.Data.Interfaces.Role;
using Personnel.Infra.Data.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Interfaces
{
    public interface IUnitOfWork
    {

        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IUserLocationRepository UserLocationRepository { get; }
        Task CommitAsync();
        ValueTask RollBackAsync();
    }
}
