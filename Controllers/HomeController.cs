using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using task1.Models;

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

        [HttpPost]
        public ActionResult Index(UserData user, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string newFileName = "";
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        newFileName = fileName + "_" + Guid.NewGuid() + extension;
                        string directoryPath = Server.MapPath("~/App_Data/");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string filePath = Path.Combine(directoryPath, newFileName);
                        file.SaveAs(filePath);
                        user.ImagePath = "/App_Data/" + newFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid file format! Please upload only JPG, JPEG, or PNG images.");
                        // Repopulate select lists before returning view
                        PopulateSelectLists(user);
                        return View(user);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Data format");
                    PopulateSelectLists(user);
                    return View(user);
                }

                _applicationDbContext.Users.Add(user);
                _applicationDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                PopulateSelectLists(user);
                return View(user);
            }
        }

        private void PopulateSelectLists(UserData user)
        {
            user.CountryList = _applicationDbContext.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.country_id.ToString(),
                    Text = c.CountryName
                }).ToList();

            user.StateList = new List<SelectListItem>(); 
            user.CityList = new List<SelectListItem>(); 
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