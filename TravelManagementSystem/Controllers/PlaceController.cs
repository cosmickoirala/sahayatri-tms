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
    public class PlaceController : BaseController
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment webHost;
        private SignInManager<AppUser> signInManager;
        private UserManager<AppUser> userManager;
        public PlaceController(ApplicationDbContext _db, IWebHostEnvironment _webHost,UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        {
            db = _db;
            webHost = _webHost;
            userManager = _userManager;
            signInManager = _signInManager;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Place";
            List<PlaceModel> placelist = new List<PlaceModel>();
            List<Image> images = new List<Image>();
            //List<Place1> places = db.Places.Select(X=>X).Where(Y=>Y.IsAdminVerified==true).ToList();
            List<Place1> places = db.Places.ToList();
            foreach (Place1 place in places)
            {
                PlaceModel placeModel = new PlaceModel();
                placeModel.Id = place.Id;
                placeModel.Name = place.PlaceName;
                placeModel.City = place.City;
                placeModel.state = place.State;
                placeModel.Country = place.Country;
                placeModel.Description = place.Description;
                if(db.Images.FirstOrDefault(X => X.Place == place && X.IsCover == true)!=null)
                {
                    placeModel.CoverImagePath = db.Images.FirstOrDefault(X => X.Place == place && X.IsCover == true).ImagePath ?? "";
                }
                else
                {
                    placeModel.CoverImagePath = "";
                }
                var rating = db.Ratings.Select(X=>X).Where(y=>y.Place==place).ToList();
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

                        placeModel.Rating = ApplicationUtilities.GetHtmlRating(Convert.ToDouble(allrating));
                    }
                   
                }
                images = db.Images.Select(x => x).Where(X => X.Place == place).ToList();
                placeModel.ImagesList = images;
                placelist.Add(placeModel);
            }


            return View(placelist);
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


        [HttpGet]
        public IActionResult AddStreet()
        {
            PlaceModel placeModel = new PlaceModel();
            placeModel.CityList = ApplicationUtilities.LoadDropdowns("cities", db);
            return View(placeModel);
        }
        [HttpPost]
        public IActionResult AddStreet(PlaceModel placeModel)
        {
            placeModel.CityList = ApplicationUtilities.LoadDropdowns("cities", db);
            Street street = new Street();
            street.StreetName = placeModel.Name;
            street.Description = placeModel.Description;
            street.City.Id = Convert.ToInt32(placeModel.CityId);
            db.Streets.Add(street);
            return View(placeModel);
        }
        [HttpGet]
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
        [HttpGet]
        public IActionResult AddState()
        {
            PlaceModel placeModel = new PlaceModel();


            placeModel.CountryList = ApplicationUtilities.LoadDropdowns("countries", db);



            return View(placeModel);
        }
        [HttpPost]
        public IActionResult AddState(PlaceModel placeModel)
        {
            placeModel.CountryList = ApplicationUtilities.LoadDropdowns("countries", db);
            State state = new State();
            state.StateName = placeModel.StateId;
            state.Description = placeModel.Description;
            state.Country = db.Countries.Find(Convert.ToInt32(placeModel.CountryId));
            db.States.Add(state);
            db.SaveChanges();
            var StateId = db.States.FirstOrDefault(x => x.StateName == state.StateName && x.Country == state.Country).Id;
            if (placeModel.Images != null)
            {
                foreach (var image in placeModel.Images)
                {
                    Image image1 = new Image();
                    image1 = ApplicationUtilities.UploadImage(webHost, image, "State");
                    image1.State = db.States.Find(StateId);
                    db.Images.Add(image1);
                    db.SaveChanges(true);
                }
            }
            return View(placeModel);
        }

        [HttpGet]
        public IActionResult AddCountry()
        {
            PlaceModel placeModel = new PlaceModel();
            return View(placeModel);
        }
        [HttpPost]

        public IActionResult AddCountry(PlaceModel placeModel)
        {
            Country country = new Country();
            country.CountryName = placeModel.Name;
            country.Description = placeModel.Description;
            db.Countries.Add(country);
            db.SaveChanges(true);
            var countryId = db.Countries.FirstOrDefault(x => x.CountryName == country.CountryName).Id;
            if (placeModel.Images != null)
            {
                foreach (var image in placeModel.Images)
                {
                    Image image1 = new Image();
                    image1 = ApplicationUtilities.UploadImage(webHost, image, "Country");
                    image1.Country = db.Countries.Find(countryId);
                    db.Images.Add(image1);
                    db.SaveChanges();
                }
            }

            int result = db.SaveChanges();


            return View(placeModel);
        }



        [HttpGet]
        public IActionResult AddCity()
        {
            PlaceModel placeModel = new PlaceModel();
            placeModel.StateList = ApplicationUtilities.LoadDropdowns("states", db);
            return View(placeModel);
        }
        [HttpPost]
        public IActionResult AddCity(PlaceModel placeModel)
        {
            placeModel.StateList = ApplicationUtilities.LoadDropdowns("states", db);
            City city = new City();
            city.CityName = placeModel.Name;
            city.Description = placeModel.Description;
            city.State = db.States.Find(Convert.ToInt32(placeModel.StateId));
            db.Cities.Add(city);
            db.SaveChanges(true);
            var CityId = db.Cities.FirstOrDefault(x => x.CityName == city.CityName && x.State == city.State).Id;
            if (placeModel.Images != null)
            {
                foreach (var image in placeModel.Images)
                {
                    Image image1 = new Image();
                    image1 = ApplicationUtilities.UploadImage(webHost, image, "City");
                    image1.City = db.Cities.Find(CityId);
                    db.Images.Add(image1);
                    db.SaveChanges(true);
                }
            }

            int result = db.SaveChanges();


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
            var allRatings = db.Ratings.Select(X => X).Where(Y => Y.Place == place && Y.User!= null).ToList();
            var USER = db.Ratings.Select(X => X.User).ToList();
            foreach (var review in allRatings)
            {
                ReviewRating reviewRating = new ReviewRating();
                reviewRating.Id = review.Id;
                reviewRating.Rating = review.value;
                reviewRating.HtmlRating = ApplicationUtilities.GetHtmlRating(Convert.ToDouble(review.value));
                reviewRating.Review = review.Review;
                reviewRating.Reviewer = new UserModel();
                try
                {
                    reviewRating.Reviewer.Name = review.User.UserName;
                    if (db.Images.FirstOrDefault(Y => Y.User == review.User && Y.IsProfile == true).ImagePath != null)
                    {
                        reviewRating.Reviewer.ProfileImage = db.Images.FirstOrDefault(Y => Y.User == review.User && Y.IsProfile == true).ImagePath;
                    }
                   
                }
                catch(Exception ex)
                {

                }
                reviewRatings.Add(reviewRating);

            }
            return reviewRatings;
        }





    }
}
