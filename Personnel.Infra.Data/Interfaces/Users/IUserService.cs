using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Personnel.Infra.Data.Shared;
using Personnel.Domain.Core.Interfaces;
using Personnel.Domain.Dtos;
using Personnel.Domain.Dtos.Personnel;
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

        IList<SelectListItem> GetAvailableUser(string systemName);

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


        int NumberOfAvailableUser(int roleId);




        IList<User> GetAllActiveUsersWithUserLocationId(int userLocationId);
        User GetUserWithRole(int id);

        List<User> GetAllUserHavePermission(string name);

        User GetUserByPhoneNumber(string phoneNumber);

        IList<int> UserofLocationWithSystemRoleName(int userLocationId, string systemRoleName);
        Task<bool> ChangePersonelEmail();

        IList<PersonelListDto> GetAllPersonelUser();



        #region UserLocation
        void DeleteUserLocation(UserLocation userLocation);

        UserLocation GetUserLocationById(int userLocationId);

        UserLocation GetUserLocationByName(string userLocation);
        IPagedList<UserLocationDto> SearchAllUserLocation(PageFilterDto command, UserLocationDto search);
        IList<UserLocation> GetAllUserLocations();
        IList<UserLocation> GetAllUserLocationsByIsActiveInRecruit(bool isActiveInRecruit);
        IPagedList<UserLocationDto> GetAllUserLocationsSearch(PageFilterDto command, UserLocationDto model);
        IList<UserLocation> AllAssitance();


        void InsertUserLocation(UserLocation userLocation);

        void UpdateUserLocation(UserLocation userLocation);



        IList<UserLocation> GetAllActiveUserLocationByIsOprational();

        IList<string> MobileOfUserLocation(int userLocationId);

        List<int> UserRoleIds(int userId);




        //void GenerateUserForArang();


        UserLocation GetUserLocationByCode(int code);

        //UserLocation GetUserLocationByCodeAndIsOprational(int code);

        //IList<UserLocationDamageDto> GetUserLocationDamage();

        Task<SignInResult> AdminLogin(User user, string password);
        #endregion

    }
}
