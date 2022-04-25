using System.Diagnostics;
using System.Threading.Tasks;
using TravelManagementSystem.Infrastructure;
using TravelManagementSystem.Infrastructure.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using TravelManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using TravelManagementSystem.ViewModels;
using TravelManagementSystem.Utilities.Helpers;
using TravelManagementSystem.Utilities;

namespace TravelManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : BaseController

    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;



        public AdminController(
            ILogger<HomeController> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/icons")]
        public IActionResult Icons()
        {
            return View();
        }

        [HttpGet("/maps")]
        public IActionResult Maps()
        {
            return View();
        }

        [ImportModelState]
        [HttpGet("/profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                FullName = user.FirstName + " " + user.LastName
            }) ;
        }

        [ExportModelState]
        [HttpPost("/profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromForm]
            ProfileViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Profile));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, input.Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Model state might not be valid anymore if we weren't able to change the e-mail address
            // so we need to check for that before proceeding
            if (ModelState.IsValid)
            {
                if (input.FullName.ToUpper() !=(user.FirstName + " " + user.LastName).ToUpper())
                {
                    
                    string[] name=input.FullName.Split(' ');
                    user.FirstName = name[0];
                    user.LastName = name[name.Length-1];
                }

                await _userManager.UpdateAsync(user);

                await _signInManager.RefreshSignInAsync(user);

                SuccessMessage = "Your profile has been updated";
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpGet("/tables")]
        public IActionResult Tables()
        {
            return View();
        }

        [HttpGet("/upgrade")]
        public IActionResult Upgrade()
        {
            return View();
        }

        [HttpGet("/privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("/login")]
        public IActionResult Login()
        {
            LoginAndRegistrationCommon model = new LoginAndRegistrationCommon();
            model.RegistrationViewModel.UserType = ApplicationEnums.UserRoles.Admin.ToString();
            return View(model);
        }

        [HttpGet("/accessdenied")]
        public IActionResult AccessDenied()
        {
            ErrorMessage = "LogIn with valid credentials!!!";
            return RedirectToActionPermanent("Index", "Account");
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/status-code")]
        public IActionResult StatusCodeHandler(int code)
        {
            ViewBag.StatusCode = code;
            ViewBag.StatusCodeDescription = ReasonPhrases.GetReasonPhrase(code);
            ViewBag.OriginalUrl = null;


            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteFeature != null)
            {
                ViewBag.OriginalUrl =
                    statusCodeReExecuteFeature.OriginalPathBase
                    + statusCodeReExecuteFeature.OriginalPath
                    + statusCodeReExecuteFeature.OriginalQueryString;
            }

            if (code == 404)
            {
                return View("Status404");
            }

            return View("Status4xx");
        }
    }
}
