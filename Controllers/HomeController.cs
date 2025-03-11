
using System;
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
            return Json(AllCountry, JsonRequestBehavior.AllowGet);
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
            List<UserData> users = _applicationDbContext.Users.Where(c => c.IsActive == true).ToList();
            foreach (var user in users)
            {
                var City = _applicationDbContext.Cities.FirstOrDefault(c => c.city_id == user.SelectedCityId);
                user.selectedCity = City != null ? City.CityName : "N/A";

                var State = _applicationDbContext.states.FirstOrDefault(s => s.state_id == City.state_id);
                user.selectedState = State != null ? State.StateName : "N/A";

                var Country = _applicationDbContext.Countries.FirstOrDefault(c => c.country_id == State.country_id);
                user.SelectedCountry = Country != null ? Country.CountryName : "N/A";
                PopulateSelectLists(user);
            }
            return View(users);
        }

        [HttpPost]
        public ActionResult serverdata(DataTableRequest dataTableRequest)
        {
            int draw = dataTableRequest.Draw;
            int start = dataTableRequest.Start;
            int length = dataTableRequest.Length;
            string searchValue = dataTableRequest.Search?.Value?.ToLower();

            // Start with the Users query
            var query = _applicationDbContext.Users
                .Where(c => c.IsActive == true)
                .Join(_applicationDbContext.Cities,
                      user => user.SelectedCityId,
                      city => city.city_id,
                      (user, city) => new { User = user, City = city })
                .Join(_applicationDbContext.states,
                      uc => uc.City.state_id,
                      state => state.state_id,
                      (uc, state) => new { uc.User, uc.City, State = state })
                .Join(_applicationDbContext.Countries,
                      ucs => ucs.State.country_id,
                      country => country.country_id,
                      (ucs, country) => new { ucs.User, ucs.City, ucs.State, Country = country });

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(ucs => ucs.User.FirstName.ToLower().Contains(searchValue) ||
                                          ucs.City.CityName.ToLower().Contains(searchValue) ||
                                          ucs.State.StateName.ToLower().Contains(searchValue) ||
                                          ucs.Country.CountryName.ToLower().Contains(searchValue) ||
                                          ucs.User.FirstName.ToLower().Contains(searchValue) ||
                                          ucs.User.LastName.ToLower().Contains(searchValue) ||
                                          ucs.User.Email.ToLower().Contains(searchValue) ||
                                          ucs.User.MobileNo.ToLower().Contains(searchValue) ||
                                          ucs.User.Gender_.ToLower().Contains(searchValue) ||
                                          ucs.User.Dob.ToLower().Contains(searchValue) ||
                                          ucs.User.Address.ToLower().Contains(searchValue));

            }

            if (dataTableRequest.Order != null && dataTableRequest.Order.Any())
            {
                var order = dataTableRequest.Order[0];

                switch (order.Column)
                {
                    case 0: // number
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.User.user_id) : query.OrderByDescending(ucs => ucs.User.user_id);

                        break;
                    case 1: // FirstName
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.User.FirstName) : query.OrderByDescending(ucs => ucs.User.FirstName);
                        break;
                    case 2: // LastName
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.User.LastName) : query.OrderByDescending(ucs => ucs.User.LastName);
                        break;
                    case 3: // Email
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.User.Email) : query.OrderByDescending(ucs => ucs.User.Email);
                        break;
                    case 4: // MobileNo
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.User.MobileNo) : query.OrderByDescending(ucs => ucs.User.MobileNo);
                        break;
                    case 5: // Gender
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.User.Gender_) : query.OrderByDescending(ucs => ucs.User.Gender_);
                        break;

                    case 6: // DOB
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.User.Dob) : query.OrderByDescending(ucs => ucs.User.Dob);
                        break;

                    case 7: // Address
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.User.Address) : query.OrderByDescending(ucs => ucs.User.Address);
                        break;

                    case 9: // City
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.City.CityName) : query.OrderByDescending(ucs => ucs.City.CityName);

                        break;
                    case 10: // State
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.State.StateName) : query.OrderByDescending(ucs => ucs.State.StateName);
                        break;
                    case 11: // Country
                        query = order.Dir == "asc" ? query.OrderBy(ucs => ucs.Country.CountryName) : query.OrderByDescending(ucs => ucs.Country.CountryName);
                        break;
                    default:
                        query = query.OrderBy(ucs => ucs.User.FirstName); // Default sorting
                        break;
                }
            }

            else
            {
                query = query.OrderBy(ucs => ucs.User.user_id);

            }

            var totalRecords = _applicationDbContext.Users.Count(c => c.IsActive == true);
            var filteredRecords = query.Count();
            var data = query.Skip(start).Take(length).ToList();
            bool isDesending = (dataTableRequest.Order[0].Column == 0 && dataTableRequest.Order[0].Dir == "desc");
            int baseIndex = isDesending ? (filteredRecords - start + 1) : start;
            int indexIncrement = isDesending ? -1 : +1;
            var result = new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = data.Select((ucs, idx) => new
                {
                    iid = baseIndex + (idx + 1) * indexIncrement,
                    id = ucs.User.user_id,
                    ucs.User.FirstName,
                    ucs.User.LastName,
                    ucs.User.Email,
                    ucs.User.MobileNo,
                    ucs.User.Gender_,
                    ucs.User.Dob,
                    ucs.User.Address,
                    ucs.User.ImagePath,
                    selectedCity = ucs.City.CityName,
                    selectedState = ucs.State.StateName,
                    SelectedCountry = ucs.Country.CountryName

                })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
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

                    var unique_MobileNumber = !_applicationDbContext.Users
                        .Any(c => c.MobileNo == user.MobileNo && c.user_id != user.user_id);

                    if (unique_email && unique_MobileNumber)
                    {
                        user.IsActive = true;
                        _applicationDbContext.Users.Add(user);
                        _applicationDbContext.SaveChanges();
                        return Json(new { success = true, message = "User created successfully!" });

                    }
                    else
                    {
                        return Json(new { success = false, message = "Email or PhoneNumber is already registered" });
                    }

                }
                else
                {
                    var errors = ModelState.Values
                             .SelectMany(v => v.Errors)
                             .Select(e => e.ErrorMessage)
                             .ToList();

                    var errorMessage = "Validation failed." + Environment.NewLine + string.Join(Environment.NewLine, errors);
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Invalid file format", ex);
                var errors = ModelState.Values
                             .SelectMany(v => v.Errors)
                             .Select(e => e.ErrorMessage)
                             .ToList();
                var errorMessage = "Validation failed." + Environment.NewLine + string.Join(Environment.NewLine, errors);
                return Json(new { success = false, message = errorMessage });
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

        [HttpPost]
        public ActionResult EditUser(UserData user, HttpPostedFileBase file)
        {
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid user data.");
                PopulateSelectLists(user);
                return View();
            }


            var Edituser = _applicationDbContext.Users.FirstOrDefault(u => u.user_id == user.user_id);

            var OldEmail = Edituser.Email;

            var OldPhoneNumber = Edituser.MobileNo;

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
                         .Any(c => c.Email == Edituser.Email);

                    var unique_MobileNumber = !_applicationDbContext.Users
                       .Any(c => c.MobileNo == Edituser.MobileNo);
                    if (!(unique_MobileNumber || OldPhoneNumber == Edituser.MobileNo))
                    {

                        return Json(new { success = false, message = "mobile number is Already registered with other User!" });
                    }
                    else if (unique_email || OldEmail.ToString() == Edituser.Email.ToString())
                    {
                        _applicationDbContext.SaveChanges();
                        return Json(new { success = true, message = "User Edited successfully!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Email is Already registered with other User!" });

                    }



                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {

                        ModelState.AddModelError("", validationErrors.ToString());
                    }
                    PopulateSelectLists(user);
                    return Json(new { success = false, message = "Some error is coming" });
                }
            }
            else
            {
                var errors = ModelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .ToList();


                var errorMessage = "Validation failed." + Environment.NewLine + string.Join(Environment.NewLine, errors);

                return Json(new { success = false, message = errorMessage });

            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                var deleteuser = _applicationDbContext.Users.FirstOrDefault(u => u.user_id == Id);
                deleteuser.IsActive = false;
                _applicationDbContext.Configuration.ValidateOnSaveEnabled = false;

                _applicationDbContext.SaveChanges();
                return Json(new { success = true, response = "Deleted Successfully", Name = deleteuser.FirstName });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "No record Found" + ex });
            }

        }

        [HttpPost]
        public JsonResult Details(int Id)
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


                return Json(new { success = true, message = user }, JsonRequestBehavior.DenyGet); ;
            }
            return Json(new { success = false, message = "Error accure" }, JsonRequestBehavior.DenyGet);
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
                    var countryid = _applicationDbContext.Countries.FirstOrDefault(c => c.country_id == StateName.country_id);
                    user.SelectedCountryId = countryid.country_id;
                }
            }

            var CountryName = _applicationDbContext.Countries.All(s => s.country_id == s.country_id);

            user.CountryList = _applicationDbContext.Countries.Where(c => c.country_id == c.country_id)
               .Select(c => new SelectListItem
               {
                   Value = c.country_id.ToString(),
                   Text = c.CountryName,
                   Selected = c.country_id == user.SelectedCountryId
               }).ToList();
        }

      
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Index()
        {
            return View();
            
        }
    }
}