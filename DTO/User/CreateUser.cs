namespace PetAdopt.DTO.User
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