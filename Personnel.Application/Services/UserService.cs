using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Personnel.Application.Interfaces;
using Personnel.Application.Shared;
using Personnel.Domain.Core.Contracts;
using Personnel.Domain.Core.Interfaces;
using Personnel.Domain.Dtos.Users;
using Personnel.Domain.Dtos;
using Personnel.Domain.Entities.Identity;
using Personnel.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Personnel.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Personnel.Application.Mangers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Personnel.Application.Helpers;
using Personnel.Domain.Dtos.Personnel;

namespace Personnel.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BaseUserManager _baseUserManager;
        private readonly BaseSignInManager _baseSignInManager;
        
        #region Field


        #endregion

        #region Ctor

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, BaseUserManager baseUserManager, BaseSignInManager baseSignInManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _baseUserManager = baseUserManager;
            _baseSignInManager = baseSignInManager;
        }
        #endregion

        #region Method

        public IList<User> GetAllActiveUsersWithUserLocationId(int userLocationId)
        {
            var query = _unitOfWork.UserRepository.TableNoTracking.Where(p => p.UserLocationId != null && p.UserLocationId == userLocationId && p.Active);
            return query.ToList();
        }

        public User GetUserWithRole(int id)
        {
            if (id < 1)
                return null;
            return _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles).FirstOrDefault(x => x.Id == id);
        }

        public List<User> GetAllUserHavePermission(string name)
        {
            var query = _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles).ThenInclude(x => x.Role).Where(x => x.UserInRoles.Any
                (c => c.Role.SystemName == name)).Where(p => p.Active);


            return query.ToList();
        }
        public async Task<bool> IsUserLockedOutAsync(User user)
        {
            var lockoutEndDate = await _baseUserManager.GetLockoutEndDateAsync(user);

            return (lockoutEndDate.HasValue && lockoutEndDate.Value > DateTimeOffset.Now);
        }
        public async Task ResetUserLockoutAsync(User user)
        {
            await _baseUserManager.SetLockoutEndDateAsync(user, null);
            await _baseUserManager.ResetAccessFailedCountAsync(user);
        }
        public async Task<IdentityResult> IncrementAccessFailedCountAsync(User user)
        {


            return await _baseUserManager.AccessFailedAsync(user);
        }

        public async Task SignOut()
        {
            await _baseSignInManager.SignOutAsync();
        }

        public async Task<bool> NormalizeAll()
        {
            var users = await _baseUserManager.Users.Where(x => x.NormalizedUserName == null).ToListAsync();
            foreach (var user in users)
            {
                await _baseUserManager.UpdateSecurityStampAsync(user);
            }
            return true;

        }

        public async Task<bool> UpdateRecentlyAddedUsers()
        {
            var users = await _unitOfWork.UserRepository.TableNoTracking.Where(x => x.CreatedOn > DateTime.Now.AddDays(-10) && string.IsNullOrEmpty(x.NormalizedUserName)).ToListAsync();
            foreach (var user in users)
            {
                await _baseUserManager.UpdateSecurityStampAsync(user);
            }

            return true;
        }

        public async Task<User> GetUserForSignIn(string userName)
        {
            var user = await _baseUserManager.FindByNameAsync(userName);

            return user;
        }

        public async Task<OperationResult<bool>> CreateOrUpdatePersonel(UserNewDto dto)
        {

            if (dto.Id == 0)
                return await CreateUser(dto);
            return await UpdateUser(dto);
        }

        private async Task<OperationResult<bool>> UpdateUser(UserNewDto dto)
        {

            var existingUser = await _unitOfWork.UserRepository.GetUserWithRoles(dto.Id);
            if (existingUser == null)
                return OperationResult<bool>.FailureResult("کاربر وجود ندارد");
            var personelRole = await _unitOfWork.RoleRepository.TableNoTracking.FirstOrDefaultAsync(x => x.SystemName == SystemUserRoleNames.Personel);
            if (!dto.UserInRoles.Any(x => x.RoleId == personelRole.Id))
                return OperationResult<bool>.FailureResult("انتخاب نقش \"پرسنل\" برای پرسنل الزامی است");
            //foreach (var item in existingUser.UserInRoles)
            //{
            //    existingUser.UserInRoles.Remove(item);
            //}
            var ceDate = existingUser.CreatedOn;
            _mapper.Map(dto, existingUser);
            existingUser.CreatedOn = ceDate;
            var checkUniquenessResult = CheckUniqueness(existingUser);
            if (!checkUniquenessResult.Result)
                return OperationResult<bool>.FailureResult(checkUniquenessResult.ErrorMessage);
            var password = Guid.NewGuid();
            existingUser.UserGuid = Guid.NewGuid();
            existingUser.Password = $"{"safgasdfgsadfg43563456"}{"365474dfgdfg"}".HashSha256();
            //existingUser.PasswordSalt = new PaswordSaltGenerator().Generate(20);
            existingUser.Email = dto.Username + "@bimehalborz.ir";

            existingUser.UserInRoles = (dto.UserInRoles.Select(x => new UserInRole { RoleId = x.RoleId, UserId = dto.Id })).ToList();
            var updateUserResult = await _baseUserManager.UpdateAsync(existingUser);
            return OperationResult<bool>.SuccessResult(true);
        }

        private async Task<OperationResult<bool>> CreateUser(UserNewDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var checkUniquenessResult = CheckUniqueness(user);
            if (!checkUniquenessResult.Result)
                return OperationResult<bool>.FailureResult(checkUniquenessResult.ErrorMessage);
            var password = Guid.NewGuid();
            user.Password = $"{"safgasdfgsadfg43563456"}{"365474dfgdfg"}".HashSha256();
            //user.PasswordSalt = new PaswordSaltGenerator().Generate(20);
            user.Email = dto.Username + "@bimehalborz.ir";
            user.UserInRoles = (dto.UserInRoles.Select(x => new UserInRole { RoleId = x.RoleId })).ToList();
            user.CreatedOn = DateTime.Now;
            var addUserResult = await _baseUserManager.CreateAsync(user);
            //await _unitOfWork.UserRepository.AddUserAsync(user);
            //await _unitOfWork.CommitAsync();
            return OperationResult<bool>.SuccessResult(true);
        }
        private OperationResult<bool> CheckUniqueness(User user)
        {
            if (_unitOfWork.UserRepository.TableNoTracking.Any(x => x.Mobile == user.Mobile && x.Id != user.Id))
                return OperationResult<bool>.FailureResult("شماره تلفن همراه تکراری است");
            if (_unitOfWork.UserRepository.TableNoTracking.Any(x => x.UserName == user.UserName && x.Id != user.Id))
                return OperationResult<bool>.FailureResult("نام کاربری همراه تکراری است");
            if (_unitOfWork.UserRepository.TableNoTracking.Any(x => x.Email == user.Email && x.Id != user.Id))
                return OperationResult<bool>.FailureResult("ایمیل تکراری است");
            if (_unitOfWork.UserRepository.TableNoTracking.Any(x => x.NationalCode == user.NationalCode && x.Id != user.Id))
                return OperationResult<bool>.FailureResult("کد یا شناسه ملی تکراری است");
            return OperationResult<bool>.SuccessResult(true);
        }
        public async Task<OperationResult<UserNewDto>> GetUserWithFullInfo(int userId)
        {
            var user = await _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserLocation).Include(x => x.UserInRoles)
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return OperationResult<UserNewDto>.NotFoundResult("کاربر مورد نظر یافت نشد");
            var result = _mapper.Map<UserNewDto>(user);
            return OperationResult<UserNewDto>.SuccessResult(result);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task SignIn(User user, bool isPersist)
        {
            await _baseSignInManager.SignInAsync(user, isPersist);
        }

        public List<User> GetAll()
        {
            return _unitOfWork.UserRepository.TableNoTracking.ToList();
        }

        public User GetById(int id)
        {
            return _unitOfWork.UserRepository.GetUserById(id);
        }

        public void Update(User user)
        {
            _unitOfWork.UserRepository.UpdateUser(user);
            _unitOfWork.CommitAsync();

        }

        public async Task<List<Roles>> GetUserRoles(int userId)
        {
            var user = await _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles).ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return new List<Roles>();
            var result = user.UserInRoles.Select(x => x.Role).ToList();
            return result;
        }

        public async Task<List<PermissionRecord>> GetUserPermissions(int userId)
        {
            var user = await _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles).ThenInclude(x => x.Role).ThenInclude(x => x.PermissionInRoles).ThenInclude(x => x.PermissionRecord)
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return new List<PermissionRecord>();
            var result = user.UserInRoles.SelectMany(x => x.Role.PermissionInRoles).Select(x => x.PermissionRecord).ToList();
            return result;
        }


        public virtual User GetUserByGuid(Guid guid)
        {
            if (Guid.Empty == guid)
                return null;


            var query = from c in _unitOfWork.UserRepository.TableNoTracking
                        orderby c.Id
                        where c.UserGuid == guid
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get User by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public virtual User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles).ThenInclude(x => x.Role)
                        orderby c.Id
                        where c.UserName == username
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        public virtual User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            // var q=from u in _userlocations where u.
            var query = from c in _unitOfWork.UserRepository.TableNoTracking
                        orderby c.Id
                        where c.Email.ToLower() == email.ToLower()
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }
        /// <summary>
        /// Get User by nationalCode
        /// </summary>
        /// <param name="nationalCode">NationalCode</param>
        /// <returns>User</returns>
        public virtual User GetUserByNationalCode(string nationalCode)
        {
            if (string.IsNullOrWhiteSpace(nationalCode))
                return null;

            var query = from c in _unitOfWork.UserRepository.TableNoTracking
                        orderby c.Id
                        where c.NationalCode == nationalCode.Trim() && c.Active && !c.Deleted
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }





        public bool ExistUsername(string username, int id = 0)
        {
            return _unitOfWork.UserRepository.TableNoTracking.AsNoTracking().Any(x => x.UserName == username && !x.Deleted && x.Id != id);
        }

        public bool ExistMobileNumber(string mobileNumber)
        {
            if (string.IsNullOrEmpty(mobileNumber))
            {
                return false;
            }
            return _unitOfWork.UserRepository.TableNoTracking.AsNoTracking().Any(x => x.UserName == mobileNumber && !x.Deleted);

        }




        public IList<SelectListItem> GetAllUserForSelect()
        {
            var query =
                _unitOfWork.UserRepository.TableNoTracking
                .Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.FirstName + " " + x.LastName + " - " + x.OperationUnitCode
                }).ToList();

            return query;
        }

        public IList<SelectListItem> GetAvailableUser(string systemName)
        {
            var role = _unitOfWork.RoleRepository.TableNoTracking.FirstOrDefault(x => x.SystemName == systemName);
            if (role == null)
                return _unitOfWork.UserRepository.TableNoTracking
                .Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.FirstName + " " + x.LastName + " - " + x.OperationUnitCode
                }).ToList();

            var query =
                _unitOfWork.UserRepository.TableNoTracking.AsNoTracking().Where(x => x.UserInRoles.Any(y => y.RoleId == role.Id))
                .Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.FirstName + " " + x.LastName + " - " + x.OperationUnitCode
                })
                    .ToList();
            return query;
        }

        public IList<User> GetAvailableUserBySystemName(string systemName)
        {
            var role = _unitOfWork.RoleRepository.TableNoTracking.FirstOrDefault(x => x.SystemName == systemName);

            if (role == null)
                return null;
            var query =
                _unitOfWork.UserRepository.TableNoTracking.AsNoTracking().Where(x => x.UserInRoles.Any(y => y.RoleId == role.Id));
            return query.ToList();
        }

        public IList<SelectListItem> GetAvailableUserOrderByUserLocationId(string systemName)
        {
            var role = _unitOfWork.RoleRepository.TableNoTracking.FirstOrDefault(x => x.SystemName == systemName);
            if (role == null)
                return _unitOfWork.UserRepository.TableNoTracking
                    .Select(x => new SelectListItem()
                    {
                        Value = x.Id.ToString(),
                        Text = x.FirstName + " " + x.LastName + " - " + x.OperationUnitCode
                    }).ToList();

            var query =
                _unitOfWork.UserRepository.TableNoTracking.Where(x => x.UserInRoles.Any(y => y.RoleId == role.Id)).OrderByDescending(p => p.UserLocationId)
                    .Select(x => new SelectListItem()
                    {
                        Value = x.Id.ToString(),
                        Text = x.FirstName + " " + x.LastName + " - " + x.OperationUnitCode
                    })
                    .ToList();
            return query;
        }

        public async Task<IList<UserSimpleDto>> GetAvailableUser(List<string> systemNames)
        {
            var roles = _unitOfWork.RoleRepository.TableNoTracking.Where(x => systemNames.Contains(x.SystemName));
            if (!roles.Any())
                return null;
            var roleIDs = roles.Select(x => x.Id);
            var response = _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles)
                .Where(x => x.UserInRoles.Any(y => roleIDs.Contains(y.RoleId))).Select(c => new UserSimpleDto
                {
                    FullNameAndOperationUnitCode = $"{c.FirstName} {c.LastName}-{c.OperationUnitCode}",
                    OperationUnitCode = c.OperationUnitCode
                });
            return await response.ToListAsync();

        }


        public IList<string> MobileOfUserRole(int roleId)
        {
            var query =
                _unitOfWork.UserRepository.TableNoTracking.Where(x => x.UserInRoles.Any(y => y.RoleId == roleId))
                .Select(x => x.Mobile);

            return query.ToList();
        }

        public IList<string> MobileOfPersonel(bool AllPersonel = true)
        {
            var query = _unitOfWork.UserRepository.TableNoTracking.Where(x => x.Active);
            
            return query.Select(x => x.Mobile).ToList();
        }

        public List<int> RoleOfUser(int userId)
        {

            var user = _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles)
                .FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return new List<int>();
            return user.UserInRoles.Select(x => x.RoleId).ToList();
        }

        public bool IsAgentOrBroker(int userId)
        {
            var roles = RoleOfUser(userId);
            return roles.Contains(6) || roles.Contains(4);
        }

        public bool IsPersonel(int userId)
        {
            var user = _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles).ThenInclude(x => x.Role).FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return false;
            return user.UserInRoles.Any(x => x.Role.SystemName == SystemUserRoleNames.Personel);
        }


        public int NumberOfAvailableUser(int roleId)
        {
            var query =
                _unitOfWork.UserRepository.TableNoTracking.Where(x => x.Active && !x.Deleted && x.UserInRoles.Any(y => y.RoleId == roleId))
                .Select(x => new
                {
                    Value = x.Id
                });
            return query.Count();
        }


        public IPagedList<UserListDto> UserList(PageFilterDto command, UserSearchDto search)
        {


            var query = _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search.OperationUnitCode))
            {
                query = query.Where(u => u.OperationUnitCode == search.OperationUnitCode);
            }

            if (search.Role.HasValue)
                query = query.Where(x => x.UserInRoles.Select(c => c.RoleId).Contains(search.Role.Value));
            if (search.CreatedFromUtc.HasValue)
                query = query.Where(c => search.CreatedFromUtc.Value <= c.CreatedOn);
            if (search.CreatedToUtc.HasValue)
                query = query.Where(c => search.CreatedToUtc.Value >= c.CreatedOn);

            if (!string.IsNullOrWhiteSpace(search.Email))
                query = query.Where(c => c.Email.Contains(search.Email));
            if (!string.IsNullOrWhiteSpace(search.Username))
                query = query.Where(c => c.UserName.Contains(search.Username));
            if (!string.IsNullOrWhiteSpace(search.FirstName))
            {
                query = query.Where(u => u.FirstName.Contains(search.FirstName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(search.LastName))
            {
                query = query.Where(u => u.LastName.Contains(search.LastName.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(search.NationalCode))
                query = query.Where(c => c.NationalCode.Contains(search.NationalCode));
            if (search.JobTypes != null && search.JobTypes.Any())
            {
                query = query.Where(c => c.JobType.HasValue && search.JobTypes.Contains(c.JobType.Value));
            }
            if (search.Types != null && search.Types.Any())
            {
                query = query.Where(c => c.Type.HasValue && search.Types.Contains(c.Type.Value));
            }
          
            var result = _mapper.ProjectTo<UserListDto>(query);
            return new PagedList<UserListDto>(result.DtoOrderByCommand(command), command.PageIndex, command.PageSize);
        }


        public List<int> UserRoleIds(int userId)
        {
            var user = _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles)
                .FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return new List<int>();
            return user.UserInRoles.Select(x => x.RoleId).ToList();
        }






        public UserLocation GetUserLocationByCode(int code)
        {
            var query = _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserLocation).FirstOrDefault(p => p.UserLocation.Code == code && p.UserLocation.IsOprational);
            return query.UserLocation;
        }



        public IList<int> UserofLocationWithSystemRoleName(int userLocationId, string systemRoleName)
        {
            var role = _unitOfWork.RoleRepository.TableNoTracking.FirstOrDefault(x => x.SystemName == systemRoleName);
            if (role == null)
                return new List<int>();
            return _unitOfWork.UserRepository.TableNoTracking.Include(b => b.UserInRoles).Where(x => x.UserLocationId == userLocationId && x.UserInRoles.Any(s => s.RoleId == role.Id)).Select(s => s.Id).ToList();
        }

        public async Task<bool> ChangePersonelEmail()
        {
   
            return true;


        }

        public IList<PersonelListDto> GetAllPersonelUser()
        {
            var query =
                _unitOfWork.UserRepository.TableNoTracking.Include(x => x.UserInRoles).ThenInclude(x => x.Role).Where(x => x.UserInRoles.Any(y => y.Role.SystemName == SystemUserRoleNames.Personel) && !x.Deleted).Select(z => new PersonelListDto
                {
                    Id = z.Id,
                    Mobile = z.Mobile,
                    OperationUnitCode = z.OperationUnitCode,
                    FirstName = z.FirstName,
                    LastName = z.LastName,
                    Active = z.Active,
                    NationalCode = z.NationalCode,
                    Email = z.Email,
                    Username = z.UserName,
                    CreatedOn = z.CreatedOn,
                });
            return query.ToList();
        }

        #endregion

        #region UserLocation
        public virtual void DeleteUserLocation(UserLocation userLocation)
        {
            if (userLocation == null)
                throw new ArgumentNullException("userLocation");

            _unitOfWork.UserLocationRepository.Remove(userLocation.Id);
            _unitOfWork.CommitAsync();
            //event notification
            //_eventPublisher.EntityDeleted(userLocation);
        }


        public virtual UserLocation GetUserLocationById(int userLocationId)
        {
            if (userLocationId == 0)
                return null;

            return _unitOfWork.UserLocationRepository.TableNoTracking.First(x => x.Id == userLocationId);
        }

        public virtual UserLocation GetUserLocationByName(string userLocation)
        {
            if (string.IsNullOrEmpty(userLocation))
                return null;

            return _unitOfWork.UserLocationRepository.TableNoTracking.FirstOrDefault(x => x.Title.ToLower() == userLocation);
        }

        public IPagedList<UserLocationDto> SearchAllUserLocation(PageFilterDto command, UserLocationDto search)
        {
            var query = (from a in _unitOfWork.UserLocationRepository.TableNoTracking
                         select new UserLocationDto
                         {
                             Id = a.Id,
                             Title = a.Title + " " + a.Code,
                             Code = a.Code,
                             IsActiveInRecruit = a.IsActiveInRecruit,
                             DisplayOrder = a.DisplayOrder,
                             IsOprational = a.IsOprational
                         });
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.Title))
                    query = query.Where(x => x.Title.Contains(search.Title));
            }

            return new PagedList<UserLocationDto>(query.DtoOrderByCommand(command), command.PageIndex, command.PageSize);
        }

        public virtual IList<UserLocation> GetAllUserLocations()
        {

            var query = from cr in _unitOfWork.UserLocationRepository.TableNoTracking
                        orderby cr.DisplayOrder
                        select cr;
            var userLocations = query.ToList();
            return userLocations;

        }


        public virtual IList<UserLocation> GetAllActiveUserLocationByIsOprational()
        {

            var query = _unitOfWork.UserLocationRepository.TableNoTracking.Where(p => p.IsOprational);
            return query.ToList();

        }

        public IList<UserLocation> GetAllUserLocationsByIsActiveInRecruit(bool isActiveInRecruit)
        {
            var query = from cr in _unitOfWork.UserLocationRepository.TableNoTracking.Where(p => p.IsActiveInRecruit == isActiveInRecruit && p.Code == 1)
                        orderby cr.DisplayOrder
                        select cr;
            var userLocations = query.ToList();
            return userLocations;
        }

        public IPagedList<UserLocationDto> GetAllUserLocationsSearch(PageFilterDto command, UserLocationDto model)
        {
            var query = (from a in _unitOfWork.UserLocationRepository.TableNoTracking.Where(p => p.Code == 1)

                         select new UserLocationDto
                         {
                             Id = a.Id,
                             IsActiveInRecruit = a.IsActiveInRecruit,
                             Title = a.Title
                         });
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.Title))
                    query = query.Where(x => x.Title.Contains(model.Title));
            }
            return new PagedList<UserLocationDto>(query.DtoOrderByCommand(command), command.PageIndex, command.PageSize);
        }


        public virtual void InsertUserLocation(UserLocation userLocation)
        {
            if (userLocation == null)
                throw new ArgumentNullException("userLocation");

            _unitOfWork.UserLocationRepository.AddUserLocation(userLocation);
            _unitOfWork.CommitAsync();

        }


        public virtual void UpdateUserLocation(UserLocation userLocation)
        {
            if (userLocation == null)
                throw new ArgumentNullException("userLocation");

            _unitOfWork.UserLocationRepository.UpdateUserLocation(userLocation);
            _unitOfWork.CommitAsync();

        }



        public IList<UserLocation> AllAssitance()
        {
            return _unitOfWork.UserLocationRepository.TableNoTracking.Where(x => x.Code == 2).ToList();
        }




        public IList<string> MobileOfUserLocation(int userLocationId)
        {
            var query =
                _unitOfWork.UserRepository.TableNoTracking.Where(x => x.UserLocationId == userLocationId)
                .Select(x => x.Mobile);

            return query.ToList();
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return null;
            }
            Regex regex = new Regex(@"^[0-9]*$");
            if (!regex.IsMatch(phoneNumber))
            {
                return null;
            }

            var count = _unitOfWork.UserRepository.TableNoTracking.Count(p => p.Phone == phoneNumber);
            if (count == 0)
            {
                return null;
            }

            if (count > 1)
            {
                return null;
            }

            var query = _unitOfWork.UserRepository.TableNoTracking.FirstOrDefault(p => p.Phone == phoneNumber);
            return query;
        }
        public async Task<SignInResult> AdminLogin(User user, string password)
        {
            return await _baseUserManager.CheckPasswordAsync(user, password) ? SignInResult.Success : SignInResult.Failed;
        }

        #endregion
    }


}
