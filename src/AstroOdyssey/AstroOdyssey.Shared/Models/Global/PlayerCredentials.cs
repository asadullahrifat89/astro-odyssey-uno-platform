namespace AstroOdyssey
{
    public class PlayerCredentials
    {
        public PlayerCredentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
