
using System.Configuration;

namespace RegisterationMvc.Controllers {
    public static class Helper {

        public static string DefaultConnection() {

            return ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;

        }



    }
}