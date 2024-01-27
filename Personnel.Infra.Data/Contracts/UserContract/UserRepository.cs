using Microsoft.EntityFrameworkCore;
using Personnel.Domain.Entities.Identity;
using Personnel.Infra.Data.Context;
using Personnel.Infra.Data.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Contracts.UserContract
{

    public class UserRepository : BaseAsyncRepository<User>, IUserRepository
    {
        public UserRepository(PersonnelDbContext dbContext) : base(dbContext)
        {
        }

        public Task<bool> IsExistUserAsync(int userId)
        {
            return Task.FromResult(TableNoTracking.Any(x => x.Id == userId));
        }

        public Task<bool> IsExistParentUserAsync(int personelCode)
        {
            return TableNoTracking.AnyAsync(x => x.OperationUnitCode == personelCode.ToString());
        }

        public Task<User> GetByPersonelCodeAsync(int personelCode)
        {
            return TableNoTracking.FirstOrDefaultAsync(x => x.OperationUnitCode == personelCode.ToString());
        }

        public Task<User> GetUserWithRoles(int userId)
        {
            return Entities.Include(x => x.UserInRoles).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task AddUserAsync(User user)
        {
            Entities.AddAsync(user);
            return Task.CompletedTask;
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return Entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public User GetUserById(int id)
        {
            return Entities.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateUser(User user)
        {
            Entities.Update(user);
        }
    }
}
