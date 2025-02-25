using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
                var City = _applicationDbContext.Cities.Find(user.SelectedCityId);
                user.selectedCity = City != null ? City.CityName : "N/A";

                var State = _applicationDbContext.states.Find(City.state_id);
                user.selectedState = State != null ? State.StateName : "N/A";

                var Country = _applicationDbContext.Countries.Find(State.country_id);
                user.SelectedCountry = Country != null ? Country.CountryName : "N/A";
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
                    try
                    {
                        if (extension.ToUpper() == ".JPG" || extension.ToUpper() == ".JPEG" || extension.ToUpper() == ".PNG")
                        {
                            file.SaveAs(filePath);

                            user.ImagePath = newFileName;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid file format");
                            PopulateSelectLists(user);
                            return View(user);
                       
                        }

                    }
                    catch(Exception ex)
                        {
                        ModelState.AddModelError("Invalid file format",ex);
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
                var unique_email = !_applicationDbContext.Users
                     .Any(c => c.Email == user.Email && c.user_id != user.user_id);

                if (unique_email)
                {
                    _applicationDbContext.Users.Add(user);
                    _applicationDbContext.SaveChanges();
                    return RedirectToAction("ViewUser");

                }
                else
                {
                    ModelState.AddModelError("", "Email is already registered");
                    PopulateSelectLists(user);
                    return View(user);
                }

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
            PopulateSelectLists(Edituser);
            Edituser.ConfirmPassword = Edituser.Password;
            return View(Edituser);
        }
        [HttpPost]
        public ActionResult Edit(UserData user, HttpPostedFileBase file)
        {
            if (user == null || user.user_id == 0)
            {
                ModelState.AddModelError("", "Invalid user data.");
                PopulateSelectLists(user);
                return View(user);
            }

            var Edituser = _applicationDbContext.Users.Find(user.user_id);

            if (Edituser == null)
            {
                return HttpNotFound();
            }

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

                    try
                    {
                     
                        var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".JPG", ".JPEG", ".PNG" };
                  
                        if (allowedExtensions.Contains(extension))
                        {
                           
                            file.SaveAs(filePath);

                            user.ImagePath = newFileName;
                        }
                        else
                        {
                            ModelState.AddModelError("FileError", "Invalid file format. Only JPG, JPEG, and PNG are allowed.");
                            PopulateSelectLists(user);
                            return View(user);
                        }
                    }

                    catch (Exception ex)
                    {
                        ModelState.AddModelError("FileError", "Error uploading file: " + ex.Message);
                        return View(user);
                    }

                }

                Edituser.user_id = user.user_id;
                Edituser.FirstName = user.FirstName;
                Edituser.LastName = user.LastName;
                Edituser.Email = user.Email;
                Edituser.MobileNo = user.MobileNo;
                Edituser.Gender1 = user.Gender1;
                Edituser.Dob = user.Dob;
                Edituser.Password = user.Password;
                Edituser.ConfirmPassword = user.ConfirmPassword;
                Edituser.Address = user.Address;
                Edituser.SelectedCountryId = user.SelectedCountryId;
                Edituser.SelectedStateId = user.SelectedStateId;
                Edituser.SelectedCityId = user.SelectedCityId;

                try
                {
                    _applicationDbContext.SaveChanges();
                    return RedirectToAction("ViewUser");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    PopulateSelectLists(user);
                    return View(user);
                }
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

                var City = _applicationDbContext.Cities.Find(user.SelectedCityId);
                user.selectedCity = City != null ? City.CityName : "N/A";

                var State = _applicationDbContext.states.Find(City.state_id);
                user.selectedState = State != null ? State.StateName : "N/A";

                var Country = _applicationDbContext.Countries.Find(State.country_id);
                user.SelectedCountry = Country != null ? Country.CountryName : "N/A";

            return View(user);     
        }

      
         private void PopulateSelectLists(UserData user)
        {
            var CityName = _applicationDbContext.Cities.Find(user.SelectedCityId);
            user.CityList = _applicationDbContext.Cities
                .Where(c => c.state_id == CityName.state_id)
                .Select(c => new SelectListItem
                {
                    Value = c.city_id.ToString(),
                    Text = c.CityName,
                    Selected = c.city_id == user.SelectedCityId 
                }).ToList();

            var StateName = _applicationDbContext.states.Find(CityName.state_id);
            user.StateList = _applicationDbContext.states
                .Where(c => c.country_id == StateName.country_id)
                .Select(c => new SelectListItem
                {
                    Value = c.state_id.ToString(),
                    Text = c.StateName,
                    Selected = c.state_id == CityName.state_id  
                }).ToList();

            user.SelectedStateId = CityName.state_id;



            foreach (var i in user.StateList)
            {
                if (i.Selected)
                {
                    int stateid = int.Parse(i.Value);
                    var countryid = _applicationDbContext.Countries.FirstOrDefault(c=>c.country_id == stateid);
                    user.SelectedCountryId = countryid.country_id;
                }
            }

            var CountryName = _applicationDbContext.Countries.All(s=>s.country_id == s.country_id);

            user.CountryList = _applicationDbContext.Countries.Where(c=>c.country_id == c.country_id)
               .Select(c => new SelectListItem
               {
                   Value = c.country_id.ToString(),
                   Text = c.CountryName,
                   Selected = c.country_id == user.SelectedCountryId
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