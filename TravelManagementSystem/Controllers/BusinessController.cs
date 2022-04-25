using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TravelManagementSystem.Data;
using TravelManagementSystem.Models;
using TravelManagementSystem.Utilities.Helpers;
using TravelManagementSystem.ViewModels;
using UserBehavior.Recommenders;
using TravelManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace TravelManagementSystem.Controllers
{
    public class BusinessController : BaseController
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment webHost;
        private SignInManager<AppUser> signInManager;
        private UserManager<AppUser> userManager;
        public BusinessController(ApplicationDbContext _db, IWebHostEnvironment _webHost,UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        {
            db = _db;
            webHost = _webHost;
            userManager = _userManager;
            signInManager = _signInManager;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Businesses";
            List<BusinessModel> Businesslist = new List<BusinessModel>();
            List<Image> images = new List<Image>();
            //List<Place1> places = db.Places.Select(X=>X).Where(Y=>Y.IsAdminVerified==true).ToList();
            List<Business> AllBusinesss = db.Businesses.ToList();
            foreach (Business business in AllBusinesss)
            {
                BusinessModel businessModel = new BusinessModel();
                businessModel.Id = business.Id;
                businessModel.Name =  business.Name;
                var place = db.Places.Find(business.Place);
                businessModel.City =  place.City;
                businessModel.state = place.State;
                businessModel.Country = place.Country;
                businessModel.Description = business.Description;
                if(db.Images.FirstOrDefault(X => X.Bussiness == business && X.IsCover == true)!=null)
                {
                    businessModel.CoverImagePath = db.Images.FirstOrDefault(X => X.Bussiness == business && X.IsCover == true).ImagePath ?? "";
                }
                else
                {
                    businessModel.CoverImagePath ="";
                }
                if (db.Images.FirstOrDefault(X => X.Bussiness == business && X.IsLogo == true) != null)
                {
                    businessModel.LogoPath = db.Images.FirstOrDefault(X => X.Bussiness == business && X.IsLogo == true).ImagePath ?? "";
                }
                else
                {
                    businessModel.LogoPath = "";
                }
                var rating = db.Ratings.Select(X=>X).Where(y=>y.Business==business).ToList();
                if(rating.Count > 0)
                {
                    var totalNoOfRating = rating.Count;
                    decimal allrating = 0;
                    decimal avgrating = 0;
                        foreach (var ratingItem in rating)
                    {
                        allrating += ratingItem.value;

                    }
                    if(allrating>0)
                    {
                        avgrating = allrating / totalNoOfRating;
                        if((allrating - Math.Floor(allrating)) > 0.5m)
                        {
                            allrating += 0.5m;
                        }

                        businessModel.Rating = ApplicationUtilities.GetHtmlRating(Convert.ToDouble(allrating));
                    }
                   
                }
                images = db.Images.Select(x => x).Where(X => X.Place == place).ToList();
                businessModel.ImagesList = images;
                Businesslist.Add(businessModel);
            }


            return View(Businesslist);
        }






        [HttpGet]
        public IActionResult AddPlace()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userType))
            {
                ErrorMessage = "You Need to Login to add a Place";
                return RedirectToAction("Index", "Account");
            }
            PlaceModel placeModel = new PlaceModel();
            placeModel.CityList = ApplicationUtilities.LoadDropdowns("cities", db);

            placeModel.CountryList = ApplicationUtilities.LoadDropdowns("countries", db);

            placeModel.StateList = ApplicationUtilities.LoadDropdowns("states", db);
            placeModel.PlaceTypeList = ApplicationUtilities.LoadDropdowns("placetype", db);

            return View(placeModel);
        }

        [HttpPost]
        public IActionResult AddPlace(PlaceModel placeModel)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userType))
            {
                ErrorMessage = "You Need to Login to add a Place";
                return RedirectToAction("Index", "Account");
            }
            else {
                placeModel.CityList = ApplicationUtilities.LoadDropdowns("cities", db);
                placeModel.CountryList = ApplicationUtilities.LoadDropdowns("countries", db);
                placeModel.StateList = ApplicationUtilities.LoadDropdowns("states", db);
                placeModel.PlaceTypeList = ApplicationUtilities.LoadDropdowns("placetype", db);

                Place1 place = new Place1();
                place.PlaceName = placeModel.Name;
                place.Description = placeModel.Description;
                place.Country = db.Countries.Find(Convert.ToInt32(placeModel.CountryId));
                place.State = db.States.Find(Convert.ToInt32(placeModel.StateId));
                place.City = db.Cities.Find(Convert.ToInt32(placeModel.CityId));
                place.PlaceType = placeModel.PlaceType;
                place.UpdatedBy = db.Users.Find(Convert.ToInt32(userId));
                place.UpdatedDate = DateTime.Now;
                place.CreatedDate = DateTime.Now;
                place.AddedBy = db.Users.Find(Convert.ToInt32(userId));
                place.IsAdminVerified = false;
                place.AdminVerificationStatus = "P";
                if (!string.IsNullOrEmpty(placeModel.StreetId))
                    place.Streets = db.Streets.Find(Convert.ToInt32(placeModel.StreetId));
                bool IfExistsPlace = db.Places.Select(x => x.PlaceName == place.PlaceName && x.State == place.State && x.Country == place.Country && x.Streets == place.Streets).First();
                if(IfExistsPlace == true)
                {
                    ErrorMessage = "Place with same details alredy Exists";
                    return View(placeModel);
                }
                db.Places.Add(place);
                db.SaveChanges();

                var placeId = db.Places.FirstOrDefault(x => x.PlaceName == place.PlaceName && x.State == place.State && x.Country == place.Country && x.Streets == place.Streets).Id;

                if (placeModel.Images != null)
                {
                    foreach (var image in placeModel.Images)
                    {
                        Image image1 = new Image();
                        image1 = ApplicationUtilities.UploadImage(webHost, image, "Place");
                        image1.Place = db.Places.Find(placeId);
                        image1.State = place.State;
                        image1.Country = place.Country;
                        image1.City = place.City;
                        image1.IsCover = false;
                        image1.IsLogo = false;
                        image1.IsProfile = false;
                        db.Images.Add(image1);
                        db.SaveChanges();
                    }


                }
                if (placeModel.CoverImage != null)
                {
                    Image image1 = new Image();
                    image1 = ApplicationUtilities.UploadImage(webHost, placeModel.CoverImage, "Place");
                    image1.Place = db.Places.Find(placeId);
                    image1.State = place.State;
                    image1.Country = place.Country;
                    image1.City = place.City;
                    image1.IsCover = false;
                    image1.IsLogo = false;
                    image1.IsCover = true;
                    db.Images.Add(image1);
                    db.SaveChanges();

                }

                SuccessMessage = "Place added successfully and sent to Admin Verification!!!!";

            return RedirectToAction("Detail", new { id = placeId });
            }
        }


        public IActionResult Detail(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return RedirectToAction("Index");
            }
            var place = db.Places.Find(id);
            PlaceModel placeModel = new PlaceModel();
            if (place != null)
            {
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
            }


            var images = db.Images.Select(x => x).Where(X => X.Place == place).ToList();
            if (images != null)
            {
                placeModel.ImagesList = images;
            }
            placeModel.ReviewsList = ReviewList(place.Id);

            if (placeModel.ReviewsList!=null)
            {
              var currentUser = db.Users.Find(Convert.ToInt32(userId));
              var ratingExists = db.Ratings.Select(X => X).Where(Y => Y.User == currentUser && Y.Place == place).FirstOrDefault();
                if(ratingExists!=null)
                {
                    placeModel.IsRated = true;
                }
               


            }
            
            return View(placeModel);
        }




    

        
       

        [HttpPost]
        public IActionResult AddReview(string UserReview,int placeId ,decimal UserRating)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userType))
            {
                ErrorMessage = "You Need to be logged in to perform this action";
                return RedirectToAction("Index", "Account");
            }
            var place=db.Places.Find(Convert.ToInt32(placeId));
            var user = db.Users.Find(Convert.ToInt32(userId));
            if(place !=null && user!= null)
            {
                Rating rating = new Rating();
                rating.Place=place;
                rating.User=user;
                rating.Review = UserReview;
                rating.value = UserRating;
                db.Ratings.Add(rating);
                db.SaveChanges();
                SuccessMessage = "Review sent sucessfully!!!";
                return RedirectToAction("Detail", new { id=placeId});
            }
            ErrorMessage="Something went wrong please log in and try again!!";
            return RedirectToAction("Index","Account");
        }

        [HttpGet]
        public IActionResult Recomended()
        {
            ViewData["ActiveMenu"] = "Place";
            List<PlaceModel> placelist = new List<PlaceModel>();
            List<Place1> places = db.Places.ToList();
            //UserCollaborativeFilterRecommender collaborativeFilterRecommender = new UserCollaborativeFilterRecommender();

            foreach (Place1 place in places)
            {
                PlaceModel placeModel = new PlaceModel();
                placeModel.Id = place.Id;
                placeModel.Name = place.PlaceName;
                placeModel.City = place.City;
                placeModel.state = place.State;
                placeModel.Country = place.Country;
                placeModel.Description = place.Description;
                placeModel.Description = place.Description;
                if (db.Images.FirstOrDefault(X => X.Place == place && X.IsCover == true) != null)
                {
                    placeModel.CoverImagePath = db.Images.FirstOrDefault(X => X.Place == place && X.IsCover == true).ImagePath;
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
                var AllImages = db.Images.Select(x => x).Where(X => X.Place == place).ToList();
                if (AllImages != null)
                {
                    placeModel.ImagesList = AllImages;
                }
                placelist.Add(placeModel);
            }
                return View(placelist);
            

        }
        //get review list by placeId
        public List<ReviewRating> ReviewList(int placeId)
        {

            List<ReviewRating> reviewRatings = new List<ReviewRating>();
            var place = db.Places.Find(placeId);
            var allRatings = db.Ratings.Select(X => X).Where(Y => Y.Place == place).ToList();
            foreach (var review in allRatings)
            {
                ReviewRating reviewRating = new ReviewRating();
                reviewRating.Id = review.Id;
                reviewRating.Rating = review.value;
                reviewRating.HtmlRating = ApplicationUtilities.GetHtmlRating(Convert.ToDouble(review.value));
                reviewRating.Review = review.Review;
                reviewRating.Reviewer = new UserModel();
                //if (db.Images.FirstOrDefault(Y => Y.User == review.User && Y.IsProfile == true).ImagePath!=null)
                //{
                //   reviewRating.Reviewer.ProfileImage = db.Images.FirstOrDefault(Y => Y.User == review.User && Y.IsProfile == true).ImagePath;
                //}
                //reviewRating.Reviewer.Name = review.User.UserName;
                reviewRatings.Add(reviewRating);

            }
            return reviewRatings;
        }





    }
}
