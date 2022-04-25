using Microsoft.AspNetCore.Mvc;

namespace TravelManagementSystem.Infrastructure
{
    [Route("[controller]/[action]", Name = "[controller]_[action]")]
    public abstract class BaseController : Controller
    {
        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
    }
}
