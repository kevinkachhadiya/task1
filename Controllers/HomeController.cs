using System;
using System.Collections.Generic;
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

            System.Console.WriteLine(City);

            return Json(City, JsonRequestBehavior.AllowGet);
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