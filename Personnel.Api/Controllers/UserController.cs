using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Personnel.Api.Models;
using Personnel.Application.Interfaces;
using Personnel.Domain.Core.Core;
using Personnel.Domain.Core.Extensions;
using Personnel.Domain.Dtos;
using Personnel.Domain.Dtos.Users;
using Personnel.Domain.Entities.Identity;
using Personnel.Domain.Interfaces;
using Personnel.Domain.Security;

namespace Personnel.Api.Controllers
{
    public class UserController : BaseController
    {

        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;
        private readonly IWorkContext _workContext;

        public UserController(IHttpContextAccessor httpContextAccessor,
           IRoleService roleService,IUserService userService,IPermissionService permissionService, IMapper mapper,IWorkContext workContext)
        {
            _roleService = roleService;
            _userService = userService;
            _permissionService = permissionService;
            _mapper = mapper;
            _workContext = workContext;
        }



        public IActionResult List()
        {
            if (!_permissionService.Authorize(CurrentUserId(), StandardPermissionProvider.UserList))
                return AccessDeniedView();
            var model = new UserModel();
            PrepareModel(model);
            return Ok(model);
        }

        public async Task<IActionResult> CreateOrUpdate(int id)
        {
            if (!_permissionService.Authorize(_workContext.CurrentUser.Id, StandardPermissionProvider.UpdateUser))
                return AccessDeniedView();

            var model = new UserCreateUpdateModel();
            if (id == 0)
            {

                PrepareNewModel(model);
                return Ok(model);
            }

            var userResult = await _userService.GetUserWithFullInfo(id);
            if (!userResult.IsSuccess)
                return Forbid();
            var user = userResult.Result;
            var userModel = _mapper.Map<UserCreateUpdateModel>(user);
            PrepareNewModel(userModel, user);
            return Ok(userModel);


        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(UserCreateUpdateModel model)
        {
            if (!_permissionService.Authorize(CurrentUserId(), StandardPermissionProvider.UpdateUser))
                return AccessDeniedView();
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                return Ok(new { IsSuccess = false, Message = messages });
            }

            var userDto = _mapper.Map<UserNewDto>(model);
            userDto.UserInRoles =
                (model.SelectedUserRoleIds.Select(x => new UserInRoleNewDto { UserId = model.Id, RoleId = x })).ToList();
            var result = await _userService.CreateOrUpdatePersonel(userDto);
            return Ok(new { IsSuccess = result.IsSuccess, Message = result.ErrorMessage });


        }


        [HttpPost]
        public IActionResult UserList(PageFilterDto command, UserSearchDto search)
        {
            //we use own own binder for searchUserRoleIds property 
            if (!_permissionService.Authorize(_workContext.CurrentUser.Id, StandardPermissionProvider.UserList))
                return AccessDeniedView();

            var query = _userService.UserList(command, search);

            return Content(JsonConvert.SerializeObject(new ResultFilterDto { data = query, total = query.TotalCount }));
        }


        [HttpPost]
        public IActionResult ContactDetailInformation(string Name)
        {
            var record = string.Empty;
            if (record == null)
                return NotFound(new { success = false });
            return Ok(new { success = true, model = record });
        }

        [HttpPost]
        public IActionResult UserAjax(string systemUserRoleNames)
        {
            return Content(JsonConvert.SerializeObject(_userService.GetAvailableUser(systemUserRoleNames)));
        }

        [HttpPost]
        public IActionResult UserNumber(int roleId)
        {
            return Content(JsonConvert.SerializeObject(_userService.NumberOfAvailableUser(roleId)));
        }

        public IActionResult UserProfile()
        {
            var user = _userService.GetUserWithRole(_workContext.CurrentUser.Id);

            //var employee = _namadService.Employee(Convert.ToInt32(user.OperationUnitCode));

            var model = new ProfileModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FirstName + " " + user.LastName,
                Mobile = user.Mobile,
                Phone = user.Phone,
                Location = _userService.GetUserLocationById(user.UserLocationId.Value).Title,
                Code = user.OperationUnitCode,
                Roles = _roleService.UserRole(user.UserInRoles.Select(x => x.RoleId).ToList()),
                UserId = user.Id,
                Email = user.Email,
                NationalCode = user.NationalCode,
            };

            return Ok(model);
        }

        [HttpPost]
        public IActionResult EditProfile(ProfileModel model)
        {
            var user = _userService.GetById(model.UserId);

            if (user == null || user.Id != CurrentUserId())
                return Ok(new { Code = 0 });

            if (string.IsNullOrEmpty(model.Mobile))
                return Ok(new { Code = 2 });

            if (model.Mobile != user.Mobile && _userService.ExistMobileNumber(model.Mobile))
                return Ok(new { Code = 3 });

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Phone = model.Phone;
            user.Mobile = model.Mobile;

            _userService.Update(user);

            return Ok(new { Code = 1 });
        }

        public IActionResult ImageProfile()
        {
            var user = _workContext.CurrentUser;

            var model = new ProfileModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FirstName + " " + user.LastName,
                Mobile = user.Mobile,
                Phone = user.Phone,
                //Roles = user.AdminComment,
                UserId = user.Id,
                Email = user.Email,
                NationalCode = user.NationalCode,
            };

            return Ok(model);
        }



        protected void PrepareModel(UserModel model)
        {
            model.AvailableUserRoles = _roleService.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            model.AvailableLocation = _userService.GetAllUserLocations().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
            model.SelectedUserRoleIds = _userService.RoleOfUser(model.Id);
            model.AvailableJobTypes = Enum.GetValues(typeof(UserJobType)).Cast<UserJobType>().Select(
                v => new SelectListItem
                {
                    Text = v.Description(),
                    Value = ((int)v).ToString(),
                }).ToList();
            model.AvailableUserTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().Select(
                               v => new SelectListItem
                               {
                                   Text = v.Description(),
                                   Value = ((int)v).ToString(),
                               }).ToList();
            model.AvailableShahkarTypes = new List<SelectListItem>
            {
                new SelectListItem {Value = "False", Text = "مطابقت ندارد"},
                new SelectListItem {Value = "True", Text = "مطابقت دارد"}
            };
            if (model.Birthday == null)
                model.Birthday = PersianDateTime.Now;


        }

        protected void PrepareNewModel(UserCreateUpdateModel model, UserNewDto userDto = null)
        {
            if (userDto != null)
            {
                model.AvailableUserRoles = _roleService.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = userDto.UserInRoles.Select(x => x.RoleId).ToList().Contains(x.Id) }).ToList();
                model.AvailableLocation = _userService.GetAllUserLocations().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString(), Selected = userDto.UserLocationId == x.Id }).ToList();
                model.SelectedUserRoleIds = _userService.RoleOfUser(model.Id);
                model.AvailableJobTypes = Enum.GetValues(typeof(UserJobType)).Cast<UserJobType>().Select(
                    v => new SelectListItem
                    {
                        Text = v.Description(),
                        Value = ((int)v).ToString(),
                        Selected = v == userDto.JobType
                    }).ToList();
                model.AvailableUserTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().Select(
                    v => new SelectListItem
                    {
                        Text = v.Description(),
                        Value = ((int)v).ToString(),
                        Selected = v == userDto.Type
                    }).ToList();
            }
            if (userDto == null)
            {
                model.AvailableUserRoles = _roleService.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
                model.AvailableLocation = _userService.GetAllUserLocations().Select(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString() }).ToList();
                model.SelectedUserRoleIds = _userService.RoleOfUser(model.Id);
                model.AvailableJobTypes = Enum.GetValues(typeof(UserJobType)).Cast<UserJobType>().Select(
                    v => new SelectListItem
                    {
                        Text = v.Description(),
                        Value = ((int)v).ToString()

                    }).ToList();
                model.AvailableUserTypes = Enum.GetValues(typeof(UserType)).Cast<UserType>().Select(
                    v => new SelectListItem
                    {
                        Text = v.Description(),
                        Value = ((int)v).ToString()

                    }).ToList();
            }
        }

    }
}
