﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
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
        public JsonResult GetAllCountry()
        {
            var AllCountry = _applicationDbContext.Countries.Select(s => new SelectListItem
            {
                Value = s.country_id.ToString(),
                Text = s.CountryName
            }).ToList();
            return Json(AllCountry,JsonRequestBehavior.AllowGet);
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

            List<UserData> users = _applicationDbContext.Users.Where(c=>c.IsActive == true).ToList();

            foreach (var user in users)
            {
                var City = _applicationDbContext.Cities.FirstOrDefault(c=>c.city_id == user.SelectedCityId);
                user.selectedCity = City != null ? City.CityName : "N/A";

                var State = _applicationDbContext.states.FirstOrDefault(s=>s.state_id == City.state_id);
                user.selectedState = State != null ? State.StateName : "N/A";

                var Country = _applicationDbContext.Countries.FirstOrDefault(c=>c.country_id == State.country_id);
                user.SelectedCountry = Country != null ? Country.CountryName : "N/A";
               PopulateSelectLists(user);
            }
            return View(users);
        }

        [HttpPost]
        public ActionResult Index(UserData user, HttpPostedFileBase file)
        {
            try
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

                        if (extension.ToUpper() == ".JPG" || extension.ToUpper() == ".JPEG" || extension.ToUpper() == ".PNG")
                        {
                            file.SaveAs(filePath);

                            user.ImagePath = newFileName;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid file format");
                            PopulateSelectLists(user);
                            return Json(new { success = false, message = "Invalid file format" });

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Data format");
                        PopulateSelectLists(user);
                        return Json(new { success = false, message = "Invalid Data format" });
                    }
                    var unique_email = !_applicationDbContext.Users
                         .Any(c => c.Email == user.Email && c.user_id != user.user_id);

                    if (unique_email)
                    {
                        user.IsActive = true;
                        _applicationDbContext.Users.Add(user);
                        _applicationDbContext.SaveChanges();
                        return Json(new { success = true, message = "User created successfully!" });

                    }
                    else
                    {
                        ModelState.AddModelError("", "Email is already registered");
                        PopulateSelectLists(user);
                        return Json(new { success = false, message = "Email is already registered" });
                    }

                }
                else
                {
                    var errors = ModelState.Values
                             .SelectMany(v => v.Errors)
                             .Select(e => e.ErrorMessage)
                             .ToList();
                    Debug.WriteLine("Validation errors: " + string.Join(", ", errors));
                    return Json(new { success = false, message = "Validation failed." +string.Join(", ", errors)});
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Invalid file format", ex);
                var errors = ModelState.Values
                             .SelectMany(v => v.Errors)
                             .Select(e => e.ErrorMessage)
                             .ToList();
                return Json(new { success = false, message = "Validation failed.", errors = errors });
            }
        }


        [HttpPost]
        public JsonResult Edit(int Id)
        {
            var Edituser = _applicationDbContext.Users.FirstOrDefault(u => u.user_id == Id);
          
            if (Edituser != null)
            {

                PopulateSelectLists(Edituser);
                Edituser.ConfirmPassword = Edituser.Password;
                return Json(new { success = true, message = Edituser, JsonRequestBehavior.DenyGet });
            }
            return Json(new { success = false, message = "Error accure" }, JsonRequestBehavior.DenyGet);

        }

        [HttpGet]

        public ActionResult Edit(UserData user, HttpPostedFileBase file)
        {
            if (user == null || user.user_id == 0)
            {
                ModelState.AddModelError("", "Invalid user data.");
                PopulateSelectLists(user);
                return View(user);
            }

            var Edituser = _applicationDbContext.Users.FirstOrDefault(u=>u.user_id==user.user_id);

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
                        var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".JPG", ".JPEG" };
                        if (allowedExtensions.Contains(extension))
                        {
                            file.SaveAs(filePath);
                            Edituser.ImagePath = newFileName;
                        }
                        else
                        {
                            ModelState.AddModelError("FileError", "Invalid file format. Only JPG, JPEG are allowed.");
                            PopulateSelectLists(user);
                            return Json(new { success = false, message = "FileError Invalid file format. Only JPG, JPEG are allowed." });
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("FileError", "Error uploading file: " + ex.Message);
                        return Json(new { success = false, message = ex.Message });
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
                Edituser.IsActive = true;
                try
                {
                    var unique_email = !_applicationDbContext.Users
                         .Any(c => c.Email == Edituser.Email );

                    if (unique_email || Edituser.Email == user.Email)
                    {
                        _applicationDbContext.SaveChanges();
                        return Json(new { success = true, message = "User Edited successfully!" });
                    }

                    return Json(new { success = false, message = "Email is Already registered with other User!" });
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {

                        ModelState.AddModelError("", validationErrors.ToString());
                    }
                        PopulateSelectLists(user);
                    return Json(new { success = false, message = "Some error is coming"});
                }
            }
            else
            {
                PopulateSelectLists(user);
                return Json(new { success = false, message = "MOdel is not valid" });
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var deleteuser = _applicationDbContext.Users.FirstOrDefault(u=>u.user_id==Id);
                Debug.WriteLine(deleteuser);
                deleteuser.IsActive = false;
                _applicationDbContext.Configuration.ValidateOnSaveEnabled = false;

                _applicationDbContext.SaveChanges();
                return Json(new { success = true, response = "Deleted Successfully", Name = deleteuser.FirstName });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "No record Found"+ ex });
             }

            }
        [HttpGet]
        public ActionResult Details(int Id)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(u => u.user_id == Id);
            if (user != null)
            {
                var City = _applicationDbContext.Cities.Find(user.SelectedCityId);
                user.selectedCity = City != null ? City.CityName : "N/A";

                var State = _applicationDbContext.states.Find(City.state_id);
                user.selectedState = State != null ? State.StateName : "N/A";

                var Country = _applicationDbContext.Countries.Find(State.country_id);
                user.SelectedCountry = Country != null ? Country.CountryName : "N/A";

                return View(user);
            }
            return RedirectToAction("ViewUser");        }

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
                    var countryid = _applicationDbContext.Countries.FirstOrDefault(c=>c.country_id == StateName.country_id);
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