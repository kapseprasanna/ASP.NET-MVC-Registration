using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using RegisterationMvc.Models;

namespace RegisterationMvc.Controllers {
    public class RegisterController : Controller {
        private string dbConnection = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

        // Action for rendering the registration form
        public ActionResult Register() {
            // Create a new instance of RegistrationModel
            var model = new RegistrationModel();

            // Check if AvailableGenders is null and initialize it if needed
            if (model.AvailableGenders == null) {
                model.AvailableGenders = new List<string>();
            }

            // Populate model.AvailableGenders
            model.AvailableGenders.AddRange(new List<string> { "Male", "Female", "Other" });

            // Retrieve countries from the database using stored procedure
            List<Country> countries = GetCountriesFromDatabase();

            // Create a SelectList containing the countries
            SelectList countryList = new SelectList(countries, "Id", "Name");

            // Add a default option
            countryList = new SelectList(countryList.Items, "Value", "Text", "");

            // Pass the SelectList to the view using ViewBag
            // Inside the Register action
            ViewBag.Countries = GetCountriesFromDatabase().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Name }).ToList();

            // Return the view with the model
            return View(model);
        }

        // Action for handling the form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationModel registrationModel) {
            // Handle form submission logic here
            if (ModelState.IsValid) {
                using (SqlConnection conn = new SqlConnection(dbConnection)) {
                    conn.Open();

                 

                        using (var command = new SqlCommand("InsertEmployee", conn)) {
                            command.CommandType = CommandType.StoredProcedure;

                            // Add parameters to the stored procedure
                            command.Parameters.AddWithValue("@FirstName", registrationModel.FirstName);
                            command.Parameters.AddWithValue("@LastName", registrationModel.LastName);
                            command.Parameters.AddWithValue("@Email", registrationModel.Email);
                            command.Parameters.AddWithValue("@Company", registrationModel.Company);
                            command.Parameters.AddWithValue("@Mobile", registrationModel.Mobile);
                            command.Parameters.AddWithValue("@Gender", registrationModel.Gender);
                            command.Parameters.AddWithValue("@Address", registrationModel.Address);

                            // Pass country name instead of country ID
                            command.Parameters.AddWithValue("@Country", registrationModel.SelectedCountryText);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0) {
                                // The INSERT was successful
                                return RedirectToAction("RegistrationSuccess");
                            } else {
                                // No rows were affected (insert failed)
                                return RedirectToAction("Error");
                            }
                        
                    } 
                }
            }


            // Populate ViewBag.Countries again to ensure the dropdown list is rendered correctly
            ViewBag.Countries = GetCountriesFromDatabase().Select(x => new SelectListItem() { Value = Convert.ToString(x.Id), Text = x.Name }).ToList();
            return View(registrationModel);
        }

        // Method to retrieve countries from the database using a stored procedure
        public List<Country> GetCountriesFromDatabase() {

            var countrylist = new List<Country>();


            using (SqlConnection conn = new SqlConnection(dbConnection)) {
                using (SqlCommand command = new SqlCommand("GetCountries", conn)) {
                    command.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {

                            countrylist.Add(new Country() {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = Convert.ToString(reader["name"]),
                            });
                        }
                    }
                }
            }

            return countrylist;
        }

        // Action for displaying the registration success view
        public ActionResult RegistrationSuccess() {
            return View();
        }

        // Action for displaying the error view
        public ActionResult Error() {
            // You can customize the error view or response here
            return View("Error"); // Assuming you have an "Error.cshtml" view
        }
    }
}
