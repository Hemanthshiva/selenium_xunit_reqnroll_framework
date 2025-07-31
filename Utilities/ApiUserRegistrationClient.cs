using RestSharp;

namespace selenium_xunit_reqnroll_framework.Utilities
{
    public static class ApiUserRegistrationClient
    {
        private const string BaseUrl = "https://automationexercise.com/api/";

        public static async Task<RestResponse> RegisterUserAsync(UserRegistrationModel user)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("createAccount", Method.Post);

            // Required fields
            request.AddParameter("name", user.Name);
            request.AddParameter("email", user.Email);
            request.AddParameter("password", user.Password);

            // Optional fields (add only if not null or empty)
            var optionalFields = new Dictionary<string, string?>
            {
                { "title", user.Title },
                { "birth_date", user.BirthDate },
                { "birth_month", user.BirthMonth },
                { "birth_year", user.BirthYear },
                { "firstname", user.FirstName },
                { "lastname", user.LastName },
                { "company", user.Company },
                { "address1", user.Address1 },
                { "address2", user.Address2 },
                { "country", user.Country },
                { "zipcode", user.ZipCode },
                { "state", user.State },
                { "city", user.City },
                { "mobile_number", user.MobileNumber }
            };

            foreach (var field in optionalFields)
            {
                if (!string.IsNullOrWhiteSpace(field.Value))
                    request.AddParameter(field.Key, field.Value);
            }
            return await client.ExecuteAsync(request);
        }
    }
}