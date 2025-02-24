using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using task1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace task1.Controllers
{
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext _applicationDbContext;
        public HomeController()
        {
            _applicationDbContext = new ApplicationDbContext();
        }

        public HomeController(ApplicationDbContext context)
        {
            _applicationDbContext = context;
        }

        [HttpGet]
        public JsonResult GetStatesByCountry(int countryId)
        {
            var states = _applicationDbContext.states
                .Where(s => s.country_id == countryId)
                .Select(s => new SelectListItem
                {
                    Value = s.state_id.ToString(),
                    Text = s.StateName
                }).ToList();

            return Json(states, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCityByState(int StateId)
        {
            var City = _applicationDbContext.Cities
                .Where(s => s.state_id == StateId)
                .Select(s => new SelectListItem
                {
                    Value = s.city_id.ToString(),
                    Text = s.CityName
                }).ToList();
            return Json(City, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewUser()
        {

            List<UserData> users = _applicationDbContext.Users.ToList();

            foreach (var user in users)
            {
                // Fetch Country Name
                var Country = _applicationDbContext.Countries.Find(user.SelectedCountryId);
                user.SelectedCountry = Country != null ? Country.CountryName : "N/A";

                // Fetch State Name
                var State = _applicationDbContext.states.Find(user.SelectedStateId);
                user.selectedState = State != null ? State.StateName : "N/A";

                // Fetch City Name
                var City = _applicationDbContext.Cities.Find(user.SelectedCityId);
                user.selectedCity = City != null ? City.CityName : "N/A";
            }

            return View(users);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new UserData
            {
                CountryList = _applicationDbContext.Countries
                    .Select(c => new SelectListItem
                    {
                        Value = c.country_id.ToString(),
                        Text = c.CountryName
                    }).ToList(),
                StateList = new List<SelectListItem>(),
                CityList = new List<SelectListItem>(),
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserData user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string newFileName = fileName + "_" + Guid.NewGuid() + extension;

                  
                    string directoryPath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, newFileName);
                    file.SaveAs(filePath);

                    user.ImagePath = newFileName;
                }

                else
                {
                    ModelState.AddModelError("", "Invalid Data format");
                    PopulateSelectLists(user);
                    return View(user);
                }
                _applicationDbContext.Users.Add(user);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("ViewUser");
            }
            else
            {
                PopulateSelectLists(user);
                return View(user);
            }
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var Edituser = _applicationDbContext.Users.FirstOrDefault(u => u.user_id == Id);
            Edituser.ConfirmPassword = Edituser.Password;

            PopulateSelectLists(Edituser);
            return View(Edituser);
        }

        [HttpPost]
        public ActionResult Edit(UserData user, HttpPostedFileBase file)
        {
            var Edituser = _applicationDbContext.Users.FirstOrDefault(u=>u.user_id == user.user_id);

            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string newFileName = fileName + "_" + Guid.NewGuid() + extension;

                    
                    string directoryPath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, newFileName);
                    file.SaveAs(filePath);
                    user.ImagePath = newFileName;
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Data format");
                    PopulateSelectLists(user);
                    return View(user);
                }
                _applicationDbContext.Users.AddOrUpdate(user);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("ViewUser");
            }
            else
            {
                PopulateSelectLists(user);
                return View(user);
            }
        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var user = _applicationDbContext.Users.Find(Id);
            _applicationDbContext.Users.Remove(user);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("ViewUser");        
        }
        [HttpGet]
        public ActionResult Details(int Id)
        {
            var user = _applicationDbContext.Users.Find(Id);

                var Country = _applicationDbContext.Countries.Find(user.SelectedCountryId);
                user.SelectedCountry = Country != null ? Country.CountryName : "N/A";

                var State = _applicationDbContext.states.Find(user.SelectedStateId);
                user.selectedState = State != null ? State.StateName : "N/A";

                var City = _applicationDbContext.Cities.Find(user.SelectedCityId);
                user.selectedCity = City != null ? City.CityName : "N/A";
           
            return View(user);     
        }


        private void PopulateSelectLists(UserData user)
        {

            user.CountryList = _applicationDbContext.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.country_id.ToString(),
                    Text = c.CountryName
                }).ToList();

            var StateName = _applicationDbContext.states.First(us=>us.state_id == user.SelectedStateId);
            user.StateList = null;
            user.StateList = _applicationDbContext.states.Where(c => c.country_id == StateName.country_id).Select(c => new SelectListItem
            {
                Value = c.state_id.ToString(),
                Text = c.StateName
            }).ToList();

            var CityName = _applicationDbContext.Cities.First(us => us.city_id == user.SelectedCityId);
            user.CityList = null;
            user.CityList = _applicationDbContext.Cities.Where(c=>c.state_id == CityName.state_id).Select(c => new SelectListItem
                {
                    Value = c.city_id.ToString(),
                    Text = c.CityName
                }).ToList();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}