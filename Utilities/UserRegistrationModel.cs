namespace selenium_xunit_reqnroll_framework.Utilities
{
    public class UserRegistrationModel
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Title { get; set; }
        public string? BirthDate { get; set; }
        public string? BirthMonth { get; set; }
        public string? BirthYear { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? MobileNumber { get; set; }
    }
}