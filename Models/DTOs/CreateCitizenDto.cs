namespace CitizensWebApi.Models.DTOs
{
    public class CreateCitizenDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
    }
}