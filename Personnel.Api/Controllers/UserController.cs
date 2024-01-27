using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Personnel.Api.Models;
using Personnel.Application.Extensions;
using Personnel.Application.Interfaces;
using Personnel.Domain.Core.Core;
using Personnel.Domain.Entities.Identity;
using Personnel.Domain.Interfaces;
using Personnel.Domain.Security;

namespace Personnel.Api.Controllers
{
    public class UserController : ControllerBase
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


        public IActionResult List()
        {
            if (!_permissionService.Authorize(CurrentUserId(), StandardPermissionProvider.UserList))
                return AccessDeniedView();
            var model = new UserModel();
            PrepareModel(model);
            return View(model);
        }

    }
}
