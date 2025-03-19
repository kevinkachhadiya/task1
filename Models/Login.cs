﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace task1.Models
{
    public class Login
    {
        [Key]
        [Required(ErrorMessage = "An Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "A Confirm Password is required.")]
        [MinLength(6, ErrorMessage = "A Confirm Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}