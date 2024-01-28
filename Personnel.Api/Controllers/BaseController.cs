using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personnel.Api.Components.Enums;
using Personnel.Api.Components.PersianCalendarComponent;
using Personnel.Api.Enums;
using Personnel.Api.Models;
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

        protected virtual IActionResult AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            // Assuming you have a NotifyModel class or similar to represent the notification
            var notification = new NotifyModel { Type = type, Message = message };

            // Assuming you have a Notifications property in your base controller to store notifications
            List<NotifyModel> notifications = HttpContext.Items["Notifications"] as List<NotifyModel>;
            if (notifications == null)
            {
                notifications = new List<NotifyModel>();
                HttpContext.Items["Notifications"] = notifications;
            }

            notifications.Add(notification);

            // You can return a response with the notifications if needed
            return Ok(notifications);
        }
        public IActionResult GetCalendarInner(int year, int month, RequestMonthType? request)
        {
          
            return Ok(new PersianCalendarComponentViewModel(year, month, (int)request));
        }

        protected IActionResult AccessDeniedView()
        {
            return Redirect("/Admin/Security/AccessDenied");
        }

    }
}
