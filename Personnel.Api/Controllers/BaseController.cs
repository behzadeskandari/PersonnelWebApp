using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personnel.Api.Enum;
using Personnel.Application.Interfaces;

namespace Personnel.Api.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {

        protected int CurrentUserId()
        {
            var WorkContext = HttpContext.RequestServices.GetRequiredService<IWorkContext>();

            var user = WorkContext.CurrentUser;

            if (user == null || user.Id == 0)
                return 0;

            return user.Id;
        }

        protected List<int> CurrentRoleIds()
        {
            var UserService = HttpContext.RequestServices.GetRequiredService<IUserService>();
            return UserService.RoleOfUser(CurrentUserId());
        }


        [NonAction]
        public TService GetService<TService>() where TService : class => HttpContext.RequestServices.GetRequiredService<TService>();

        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("Portal.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }
        public IActionResult GetCalendarInner(int year, int month, RequestMonthType? request)
        {
            return ViewComponent(typeof(PersianCalendarComponent), new { year, month, request });
        }

        protected IActionResult AccessDeniedView()
        {
            return Redirect("/Admin/Security/AccessDenied");
        }

    }
}
