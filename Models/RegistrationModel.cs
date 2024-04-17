using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegisterationMvc.Models {
    public class RegistrationModel {

        // below are all the validations we need to install ComponentModel.DataAnnotations to get this sever side validations 

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"(?!^\s+$)^[a-zA-Z\s\.]+$", ErrorMessage = "Special character and numbers not allowed")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"(?!^\s+$)^[a-zA-Z\s\.]+$", ErrorMessage = "Special character and numbers not allowed")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,10}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [RegularExpression(@"^(?!\s*$).+[\s\S]*", ErrorMessage = "Special characters not allowed")]
        [StringLength(100, ErrorMessage = "Only 100 characters allowed for Company Name.")]
        [Required(ErrorMessage = "Company Name is required")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Please enter your Mobile Number")]
        [StringLength(20, ErrorMessage = "Mobile Number cannot exceed 20 digits")]
        [RegularExpression(@"[0-9][0-9]{2,20}", ErrorMessage = "Not a valid Mobile Number ")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public List<string> AvailableGenders { get; set; }

        [Required(ErrorMessage = "Please select your Country")]
        [DisplayName("Country  *")]
        public string SelectedCountryName { get; set; }

        public Country CountryModel { get; set; }

        public string SelectedCountryText { get; set; }

    }
}