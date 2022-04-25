
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using TravelManagementSystem.Data;
using TravelManagementSystem.Models;
using TravelManagementSystem.ViewModels;
using TravelManagementSystem.Utilities;
using TravelManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using TravelManagementSystem.Utilities.Helpers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TravelManagementSystem.Controllers
{
    public class AccountController: BaseController
    {
        private ApplicationDbContext db;
        private SignInManager<AppUser> signInManager;
        private UserManager<AppUser> userManager;
        private readonly ILogger<AccountController> logger;



        public AccountController(ApplicationDbContext _db,UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager , ILogger<AccountController> _logger)
        {
            db = _db;
            userManager = _userManager;
            signInManager = _signInManager;
            logger = _logger;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["ActiveMenu"] = "login";
            LoginAndRegistrationCommon loginAndRegistrationCommon = new LoginAndRegistrationCommon();
            loginAndRegistrationCommon.RegistrationViewModel = new RegistrationViewModel();
            loginAndRegistrationCommon.LoginModel= new LoginModel();
            loginAndRegistrationCommon.RegistrationViewModel.UserType = ApplicationEnums.UserRoles.User.ToString();
            return View(loginAndRegistrationCommon);
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            ViewData["ActiveMenu"] = "login";
            LoginAndRegistrationCommon loginAndRegistrationCommon = new LoginAndRegistrationCommon();
            loginAndRegistrationCommon.RegistrationViewModel = new RegistrationViewModel();
            loginAndRegistrationCommon.LoginModel = new LoginModel();
            loginAndRegistrationCommon.RegistrationViewModel.UserType = ApplicationEnums.UserRoles.Admin.ToString();
            return View(loginAndRegistrationCommon);
        }

        [HttpGet][Authorize("Admin")]
        public ActionResult RegisterAdmin()
        {
            ViewData["ActiveMenu"] = "login";
            LoginAndRegistrationCommon loginAndRegistrationCommon = new LoginAndRegistrationCommon();
            loginAndRegistrationCommon.RegistrationViewModel = new RegistrationViewModel();
            loginAndRegistrationCommon.LoginModel = new LoginModel();
            loginAndRegistrationCommon.RegistrationViewModel.UserType = ApplicationEnums.UserRoles.Admin.ToString();
            return View(loginAndRegistrationCommon);
        }

        [HttpGet]
        public ActionResult Welcome()
        {
            UserModel userModel = new UserModel();
            userModel.UserId= HttpContext.Session.GetString("UserId");
            userModel.Email = HttpContext.Session.GetString("Email");
            userModel.UserName = HttpContext.Session.GetString("UserName");
            return View(userModel);
        }




        [HttpPost]
        public async Task<IActionResult> LogIn(LoginModel loginModel)
        {
          
            AppUser user = new AppUser();
            ModelState.Remove("Id");
            var IsEmail  = Regex.IsMatch(loginModel.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!IsEmail)
            {
                user.UserName = loginModel.Email;
            }
            else
            {
                
                var Data = await userManager.FindByEmailAsync(loginModel.Email);
                if (Data != null)
                {
                    user.UserName=Data.UserName;
                    user.Email = Data.Email;
                }
                else
                {
                    user.UserName = loginModel.Email;
                }

            }
           
            user.PasswordHash = loginModel.Password;

            var Result = await signInManager.PasswordSignInAsync(user.UserName,user.PasswordHash, true,false);
            try
            {
                if (Result.Succeeded)
                {
                    AppUser person = await userManager.FindByNameAsync(user.UserName);
                    if (person != null)
                    {
                        HttpContext.Session.SetString("UserId", person.Id.ToString());
                        HttpContext.Session.SetString("UserName", person.UserName);
                        HttpContext.Session.SetString("Email", person.Email);
                        var role = db.UserRoles.FirstOrDefault(X => X.UserId == person.Id);
                        AppRole userRole = await db.Roles.FindAsync(role.RoleId);
                        if (userRole != null)
                        {
                            HttpContext.Session.SetString("UserType", userRole.Name.ToString());
                            if(userRole.IsAdmin)
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                        }
                        else
                        {
                            HttpContext.Session.SetString("UserType", "GuestUser");
                        }
                        await signInManager.SignInAsync(person, isPersistent: false);
                        SuccessMessage = "Success fully Signed in " + person.UserName;
                        return RedirectToAction("Welcome");
                    }
                    else
                    {
                        await signInManager.SignOutAsync();
                        ErrorMessage = "Error Occured";
                    }
                }
            }
            catch (Exception ex)
            {
              ErrorMessage= "Failed!! : "+ex.Message;
                return RedirectToAction("Index");
            }
            ErrorMessage = "UserName or Password Invalid!!!";
            return RedirectToAction("Index");
        }

    
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegistrationViewModel RegistrationViewModel)
        {
            LoginAndRegistrationCommon loginAndRegistrationCommon = new LoginAndRegistrationCommon();
            loginAndRegistrationCommon.RegistrationViewModel= RegistrationViewModel;


            #region datavalidation
            if (String.IsNullOrEmpty(RegistrationViewModel.Email))
                {
                    ModelState.AddModelError(nameof(RegistrationViewModel.Email), "Please enter your Email");
                return View("Index",loginAndRegistrationCommon);
               }
                if (String.IsNullOrEmpty(RegistrationViewModel.Username))
                {
                    ModelState.AddModelError(nameof(RegistrationViewModel.Username), "Please enter a Username");
                return View("Index", loginAndRegistrationCommon);
                }
            if (String.IsNullOrEmpty(RegistrationViewModel.Password) || String.IsNullOrEmpty(RegistrationViewModel.ConfirmPassword))
            {
                
                ModelState.AddModelError(nameof(RegistrationViewModel.Password), "Please enter a Password");
                ModelState.AddModelError(nameof(RegistrationViewModel.ConfirmPassword), "Please enter a Password");
                return View("Index", loginAndRegistrationCommon);
            }
            else
            {
                if(RegistrationViewModel.Password!=RegistrationViewModel.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(RegistrationViewModel.ConfirmPassword), "Passwords do not match!!!");
                    return View("Index", loginAndRegistrationCommon);

                }

            }
            #endregion

            AppUser olduser = await userManager.FindByEmailAsync(RegistrationViewModel.Email);
                if (olduser == null)
                {
                    AppUser user = new AppUser();                    
                    user.UserName = RegistrationViewModel.Username;
                    user.Email = RegistrationViewModel.Email;
 
                    user.PasswordHash = RegistrationViewModel.Password;
                    try
                    {
                        IdentityResult Result = await userManager.CreateAsync(user, user.PasswordHash);
                        if (Result.Succeeded)
                        {

                        AppRole role = new AppRole();
                        if(!string.IsNullOrEmpty(RegistrationViewModel.UserType))
                             role.Name=RegistrationViewModel.UserType.ToString();
                         AppUser currentUser = await userManager.FindByEmailAsync(user.Email);
                            var rolesResult = await userManager.AddToRoleAsync(currentUser, role.Name);
                        if (rolesResult.Succeeded)
                        {
                            SuccessMessage = role.Name + "  created successfully!!!";
                        }
                        await signInManager.SignInAsync(user, isPersistent: false);
                        HttpContext.Session.SetString("UserId", currentUser.Id.ToString());
                        HttpContext.Session.SetString("UserName", currentUser.UserName);
                        HttpContext.Session.SetString("Email", currentUser.Email);
                        HttpContext.Session.SetString("UserType", role.Name);
                        TempData["First"] = "true";
                            return RedirectToAction("Welcome");

                        }
                    else
                    {
                        ErrorMessage = "Error Occured"+ Result.Errors;
                    }
                    }
                    catch (Exception ex)
                    {
                      ErrorMessage = "Failed to create User !!:  " + ex.Message;
                    }
                  

                }
                else 
                {
                    ErrorMessage = "UserName or Email Already Exists!!!";
                }

            return RedirectToAction("Index", "Account");
        }
        [HttpGet("/accessdenied")]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ErrorMessage = "LogIn with valid credentials!!!";
            return RedirectToActionPermanent("Index", "Account");
        }

        public async Task<IActionResult> Logout(string returnUrl=null)
        {
           
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Email");
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
            SuccessMessage = "Sign Out Successfully";
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ProfileView()
        {
            ViewData["ActiveMenu"] = "Profile";
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            UserModel userModel = new UserModel();
            userModel.Name = user.UserName;
            var ProfileImage = db.Images.FirstOrDefault(X => X.User == user && X.IsProfile == true);
            if(ProfileImage != null)
            {
                userModel.ProfileImage = ProfileImage.ImagePath;
            }
            userModel.Email = user.Email;
            var userRoleDb = db.UserRoles;
            var userRole = db.UserRoles.FirstOrDefault(X => X.UserId == user.Id);
            userModel.UserRole = db.Roles.FirstOrDefault(Y=>Y.Id==(userRole.RoleId)).Name;
            userModel.Places = MyPlaces();
            userModel.WishList = WishList();
            userModel.ReviewList=ReviewList(user.Id);
            return View(userModel);
        }


        [HttpGet]
        public ActionResult AddBusiness()
        {
            ViewData["ActiveMenu"] = "Register";
            BusinessModel businessModel = new BusinessModel();
            businessModel.CityList = ApplicationUtilities.LoadDropdowns("cities",db);
            businessModel.CountryList = ApplicationUtilities.LoadDropdowns("countries",db);
            businessModel.StateList = ApplicationUtilities.LoadDropdowns("states",db);
           // businessModel.UserType = ApplicationEnums.UserRoles.Business.ToString();
            
            return View(businessModel);
        }

        [HttpPost]
        public ActionResult AddBusiness(BusinessModel model,IFormFile logo,IFormFileCollection Images)
        {
            if (ModelState.IsValid)
            {
                Business business = new Business();
                
           
                

            }
            return View(model);
        }



        //getting all places added by user
        public List<PlaceModel> MyPlaces()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userType = HttpContext.Session.GetString("UserId");
            var currentUser = db.Users.Find(Convert.ToInt32(userId));
            ViewData["ActiveMenu"] = "Place";
            List<PlaceModel> placelist = new List<PlaceModel>();
            List<Image> images = new List<Image>();
            List<Place1> places = db.Places.Select(X => X).Where(Y => Y.AddedBy == currentUser).ToList();
            foreach (Place1 place in places)
            {
                PlaceModel placeModel = new PlaceModel();
                placeModel.Id = place.Id;
                placeModel.Name = place.PlaceName;
                placeModel.City = place.City;
                placeModel.state = place.State;
                placeModel.Country = place.Country;
                placeModel.Description = place.Description;
                if (db.Images.FirstOrDefault(X => X.Place == place && X.IsCover == true) != null)
                {
                    placeModel.CoverImagePath = db.Images.FirstOrDefault(X => X.Place == place && X.IsCover == true).ImagePath ?? "";
                }
                else
                {
                    placeModel.CoverImagePath = "";
                }
                var rating = db.Ratings.Select(X => X).Where(y => y.Place == place).ToList();
                if (rating.Count > 0)
                {
                    var totalNoOfRating = rating.Count;
                    decimal allrating = 0;
                    decimal avgrating = 0;
                    foreach (var ratingItem in rating)
                    {
                        allrating += ratingItem.value;

                    }
                    if (allrating > 0)
                    {
                        avgrating = allrating / totalNoOfRating;
                        if ((allrating - Math.Floor(allrating)) > 0.5m)
                        {
                            allrating += 0.5m;
                        }

                        placeModel.Rating = ApplicationUtilities.GetHtmlRating(Convert.ToDouble(allrating));
                    }

                }
                images = db.Images.Select(x => x).Where(X => X.Place == place).ToList();
                placeModel.ImagesList = images;
                placelist.Add(placeModel);
            }
            return placelist;
        }



        //getting wish list of Current User
        public List<PlaceModel> WishList()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userType = HttpContext.Session.GetString("UserId");
            var currentUser = db.Users.Find(Convert.ToInt32(userId));
            ViewData["ActiveMenu"] = "Place";
            List<PlaceModel> placelist = new List<PlaceModel>();
            List<Image> images = new List<Image>();
            List<Place1> places = db.Places.Select(X => X).Where(Y => Y.AddedBy == currentUser).OrderBy(x=>x.CreatedDate).ToList();
            foreach (Place1 place in places)
            {
                PlaceModel placeModel = new PlaceModel();
                placeModel.Id = place.Id;
                placeModel.Name = place.PlaceName;
                placeModel.City = place.City;
                placeModel.state = place.State;
                placeModel.Country = place.Country;
                placeModel.Description = place.Description;
                if (db.Images.FirstOrDefault(X => X.Place == place && X.IsCover == true) != null)
                {
                    placeModel.CoverImagePath = db.Images.FirstOrDefault(X => X.Place == place && X.IsCover == true).ImagePath ?? "";
                }
                else
                {
                    placeModel.CoverImagePath = "";
                }
                var rating = db.Ratings.Select(X => X).Where(y => y.Place == place).ToList();
                if (rating.Count > 0)
                {
                    var totalNoOfRating = rating.Count;
                    decimal allrating = 0;
                    decimal avgrating = 0;
                    foreach (var ratingItem in rating)
                    {
                        allrating += ratingItem.value;

                    }
                    if (allrating > 0)
                    {
                        avgrating = allrating / totalNoOfRating;
                        if ((allrating - Math.Floor(allrating)) > 0.5m)
                        {
                            allrating += 0.5m;
                        }

                        placeModel.Rating = ApplicationUtilities.GetHtmlRating(Convert.ToDouble(allrating));
                    }

                }
                images = db.Images.Select(x => x).Where(X => X.Place == place).ToList();
                placeModel.ImagesList = images;
                placelist.Add(placeModel);
            }
            return placelist;
        }

        //getting all review list of Current User
        public List<ReviewRating> ReviewList(int Id)
        {

            List<ReviewRating> reviewRatings = new List<ReviewRating>();
            var user = db.Users.Find(Id);
            var allRatings = db.Ratings.Select(X=>X).Where(Y=>Y.User == user).ToList();
            foreach (var review in allRatings)
            {
                ReviewRating reviewRating = new ReviewRating();
                reviewRating.Id = review.Id;
                reviewRating.Rating = review.value;
                reviewRating.HtmlRating = ApplicationUtilities.GetHtmlRating(Convert.ToDouble(review.value));
                reviewRating.Review = review.Review;
                reviewRatings.Add(reviewRating);
               
            }
            return reviewRatings;
        }

    }
}
