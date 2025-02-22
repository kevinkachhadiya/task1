using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;

namespace task1.Models
{
    public class UserData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int user_id { get; set; }

        [Display(Name = "First Name : ")]
        [Required(ErrorMessage = "A First Name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First Name can only contain letters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name : ")]
        [Required(ErrorMessage = "A Last Name is required.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last Name can only contain letters.")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "An Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "Mobile No : ")]
        [Required(ErrorMessage = "A Mobile Number is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Mobile number must contain only digits.")]
        public string MobileNo { get; set; }

        [Display(Name = "Gender : ")]
        [Required(ErrorMessage = "A Gender is required.")]
        public Gender Gender { get; set; }

        [Display(Name = "Date of Birth : ")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A Dob is required.")]
        public string Dob { get; set; }

        [Display(Name = "Password : ")]
        [Required(ErrorMessage = "A Password is required.")]
        [MinLength(6, ErrorMessage = "A Password must be more than 6 letter")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(Name = "Confirm Password : ")]
        [Required(ErrorMessage = "A Confirm Password is required.")]
        [MinLength(6, ErrorMessage = "A Confirm Password must be more than 6 letter")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }

        [Display(Name = "Address : ")]
        [Required(ErrorMessage = "An Address is required.")]
        public String Address { get; set; }

     
        [Display(Name = "Image : ")]
        [Required(ErrorMessage = "An Image is required.")]
        public string ImagePath { get; set; }

        [Display(Name = "Select a Country")]
        public int SelectedCountryId { get; set; }

        [Display(Name = "Select a State")]
        public int SelectedStateId { get; set; }

        [Display(Name = "Select a city")]
        public int SelectedCityId { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }


    }
    public enum Gender
    { 
       Male,
       Female,
       Other
    }

}