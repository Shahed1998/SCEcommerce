using Microsoft.AspNetCore.Mvc;
using Models.BusinessEntities;
using System.Text.Json;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected void Alert()
        {
            if (TempData["Notification"] is string json)
            {
                ViewBag.ToastrNotification = JsonSerializer.Deserialize<NotificationViewModel>(json);
            }
        }
    }
}
