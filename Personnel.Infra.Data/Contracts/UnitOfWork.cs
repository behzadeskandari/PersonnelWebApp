using Personnel.Infra.Data.Contracts.RoleContract;
using Personnel.Infra.Data.Contracts.UserContract;
using Personnel.Infra.Data.Interfaces.Role;
using Personnel.Infra.Data.Interfaces.Users;
using Personnel.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personnel.Infra.Data.Context;

namespace Personnel.Infra.Data.Contracts
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly PersonnelDbContext _db;
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IUserLocationRepository UserLocationRepository { get; }
        
        public UnitOfWork(PersonnelDbContext db)
        {
            _db = db;
            UserRepository = new UserRepository(_db);
            RoleRepository = new RoleRepository(_db);
            UserLocationRepository = new UserLocationRepository(_db);
        
        }



        public Task CommitAsync()
        {
            return _db.SaveChangesAsync();
        }

        public ValueTask RollBackAsync()
        {
            return _db.DisposeAsync();
        }
    }
}
