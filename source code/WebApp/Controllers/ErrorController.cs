using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("Error", Name = "ErrorPage")]
    public class ErrorController : Controller
    {
        public IActionResult Index(string? message)
        {

            if(string.IsNullOrEmpty(message))
            {
                return RedirectToAction("Index", "Home", new {area = "Customer"});
            }

            ViewBag.ErrorMsg = message;
            return View();
        }
    }
}
