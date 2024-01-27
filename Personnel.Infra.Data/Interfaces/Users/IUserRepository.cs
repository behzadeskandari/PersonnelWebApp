using Personnel.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Infra.Data.Interfaces.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> IsExistUserAsync(int userId);
        Task<bool> IsExistParentUserAsync(int personelCode);
        Task<User> GetByPersonelCodeAsync(int personelCode);
        Task<User> GetUserWithRoles(int userId);
        Task AddUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        User GetUserById(int id);
        void UpdateUser(User user);
    }
}
