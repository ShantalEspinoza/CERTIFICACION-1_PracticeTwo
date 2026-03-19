namespace CitizensWebApi.Models
{
    public class Citizen
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        
        // Use string to ensure the exact formatting in the CSV and Swagger
        public string BloodGroup { get; set; } = string.Empty; 
        
        public string PersonalAsset { get; set; } = string.Empty;
    }
}