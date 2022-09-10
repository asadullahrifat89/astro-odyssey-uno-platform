namespace AstroOdyssey
{
    public class PlayerAuthCredentials 
    {
        public PlayerAuthCredentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
