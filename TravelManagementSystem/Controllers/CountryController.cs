using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TravelManagementSystem.Data;
using TravelManagementSystem.Infrastructure;
using TravelManagementSystem.Models;
using TravelManagementSystem.Utilities.Helpers;
using TravelManagementSystem.ViewModels;
using UserBehavior.Recommenders;

namespace TravelManagementSystem.Controllers
{
    public class CountryController : BaseController
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment webHost;
        public CountryController(ApplicationDbContext _db , IWebHostEnvironment _webHost)
        {
            db = _db;
            webHost = _webHost;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Place";
            List<PlaceModel> placelist = new List<PlaceModel>();
            List<Image> images = new List<Image>();
            List<Place1> places = db.Places.ToList();
          
            foreach (Place1 place in places)
            {
                PlaceModel placeModel = new PlaceModel();
                placeModel.Name = place.PlaceName;
                placeModel.City = place.City;
                placeModel.state = place.State;
                placeModel.Country = place.Country;
                placeModel.Description= place.Description;
                images = db.Images.Select(x=>x).Where(X => X.Place == place).ToList();
                placeModel.ImagesList= images;
                placelist.Add(placeModel);
            }
         

            return View(placelist);
        }

        [HttpGet]
        public IActionResult AddPlace()
        {
            PlaceModel placeModel = new PlaceModel();
            placeModel.CityList = ApplicationUtilities.LoadDropdowns("cities",db);

            placeModel.CountryList = ApplicationUtilities.LoadDropdowns("countries",db);

            placeModel.StateList = ApplicationUtilities.LoadDropdowns("states",db);

            return View(placeModel);
        }

        [HttpPost]
        public IActionResult AddPlace(PlaceModel placeModel)
        {
            placeModel.CityList = ApplicationUtilities.LoadDropdowns("cities", db);
            placeModel.CountryList = ApplicationUtilities.LoadDropdowns("countries", db);
            placeModel.StateList = ApplicationUtilities.LoadDropdowns("states", db);
            Place1 place = new Place1();
            place.PlaceName = placeModel.Name;
            place.Description = placeModel.Description;
            place.Country = db.Countries.Find(Convert.ToInt32(placeModel.CountryId));
            place.State =db.States.Find(Convert.ToInt32(placeModel.StateId));
            place.City = db.Cities.Find(Convert.ToInt32(placeModel.CityId));
            if(!string.IsNullOrEmpty(placeModel.StreetId))
                 place.Streets = db.Streets.Find(Convert.ToInt32(placeModel.StreetId));
            db.Places.Add(place);
            db.SaveChanges();
            var placeId = db.Places.FirstOrDefault(x => x.PlaceName == place.PlaceName && x.State==place.State && x.Country==place.Country && x.Streets==place.Streets).Id;
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
                        db.Images.Add(image1);
                        db.SaveChanges();
                    }
            }
            if(placeModel.CoverImage!=null)
            {
                Image image1 = new Image();
                image1 = ApplicationUtilities.UploadImage(webHost, placeModel.CoverImage, "Place");
                image1.Place = db.Places.Find(placeId);
                image1.State = place.State;
                image1.Country = place.Country;
                image1.City = place.City;
                image1.IsCover = true;
                db.Images.Add(image1);
                db.SaveChanges();

            }


            return RedirectToAction("Index");
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
            var CityId = db.Cities.FirstOrDefault(x => x.CityName == city.CityName && x.State==city.State).Id;
            if (placeModel.Images != null)
            {
                foreach (var image in placeModel.Images)
                {
                    Image image1 = new Image();
                    image1 = ApplicationUtilities.UploadImage(webHost, image, "City");
                    image1.City= db.Cities.Find(CityId);
                    db.Images.Add(image1);
                    db.SaveChanges(true);
                }
            }

            int result = db.SaveChanges();


            return View(placeModel);
        }

        [HttpGet]
        public IActionResult Detail(string id="1")
        {
            var place = db.Places.Find(Convert.ToInt32(id));
            PlaceModel placeModel= new PlaceModel();
            
            return View(placeModel);
        }
        [HttpGet]
        public IActionResult Rate(string id = "1")
        {
            var place = db.Places.Find(Convert.ToInt32(id));
            PlaceModel placeModel= new PlaceModel();
            return View(placeModel);
        }

        [HttpPost]
        public IActionResult Rate(PlaceModel placemodel)
        {
             db.Places.Find(Convert.ToInt32(placemodel.Place.Id));

            return View(placemodel);
        }

        [HttpGet]
        public IActionResult Recomended()
        {
            ViewData["ActiveMenu"] = "Place";
            List<PlaceModel> placelist = new List<PlaceModel>();
            List<Image> images = new List<Image>();
            List<Place1> places = db.Places.ToList();
            //UserCollaborativeFilterRecommender collaborativeFilterRecommender = new UserCollaborativeFilterRecommender();

            foreach (Place1 place in places)
            {
                PlaceModel placeModel = new PlaceModel();
                placeModel.Name = place.PlaceName;
                placeModel.City = place.City;
                placeModel.state = place.State;
                placeModel.Country = place.Country;
                placeModel.Description = place.Description;
                placeModel.Rating = ApplicationUtilities.GetHtmlRating(3.5);
                images = db.Images.Select(x => x).Where(X => X.Place == place).ToList();
                placeModel.ImagesList = images;
                placelist.Add(placeModel);
            }


            return View(placelist);
        }
    }
}
