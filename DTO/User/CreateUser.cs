namespace PetAdopt.DTO
{
    public class CreateUser
    {
        public string Account { get; set; }

        public string Display { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}