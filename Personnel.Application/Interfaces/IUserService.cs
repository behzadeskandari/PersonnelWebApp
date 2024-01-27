using Microsoft.AspNetCore.Identity;
using Personnel.Application.Shared;
using Personnel.Domain.Core.Interfaces;
using Personnel.Domain.Dtos;
using Personnel.Domain.Dtos.Users;
using Personnel.Domain.Entities.Identity;
using Personnel.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personnel.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> NormalizeAll();
        Task<bool> UpdateRecentlyAddedUsers();
        Task<User> GetUserForSignIn(string userName);
        Task<OperationResult<bool>> CreateOrUpdatePersonel(UserNewDto dto);
        Task<OperationResult<UserNewDto>> GetUserWithFullInfo(int userId);
        Task DeleteAsync(int id);
        Task SignIn(User user, bool isPersist);
        List<User> GetAll();
        User GetById(int id);
        void Update(User user);
        public Task<List<Roles>> GetUserRoles(int userId);
        public Task<List<PermissionRecord>> GetUserPermissions(int userId);
        User GetUserByEmail(string email);
        Task ResetUserLockoutAsync(User user);
        Task<IdentityResult> IncrementAccessFailedCountAsync(User user);
        Task SignOut();



        User GetUserByGuid(Guid guid);

        User GetUserByUsername(string username);

        User GetUserByNationalCode(string nationalCode);
        Task<bool> IsUserLockedOutAsync(User user);


        bool ExistUsername(string username, int id = 0);

        bool ExistMobileNumber(string mobileNumber);




        IList<string> MobileOfUserRole(int roleId);

        IList<string> MobileOfPersonel(bool AllPersonel = true);

        List<int> RoleOfUser(int userId);

        bool IsPersonel(int userId);

        IPagedList<UserListDto> UserList(PageFilterDto command, UserSearchDto search);


    }
}
