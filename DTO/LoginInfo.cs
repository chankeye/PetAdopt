namespace PetAdopt.DTO
{
    public class LoginInfo
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class FBLoginInfo
    {
        //public string Status { get; set; }

        //public AuthResponse AuthResponse { get; set; }

        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }

    public class AuthResponse
    {
        public string AccessToken { get; set; }

        public string ExpiresIn { get; set; }

        public string SignedRequest { get; set; }

        public string UserID { get; set; }
    }
}