namespace SpaceShooterGame
{
    public static class PlayerCredentialsHelper
    {
        #region Properties

        public static PlayerCredentials PlayerCredentials { get; set; }

        #endregion

        #region Methods

        public static PlayerCredentials GetCachedPlayerCredentials()
        {
            if (PlayerCredentials is not null)
            {
                PlayerCredentials.Password = PlayerCredentials.Password.Decrypt();
                return PlayerCredentials;
            }

            return null;
        }

        public static void SetPlayerCredentials(string userName, string password)
        {

            PlayerCredentials = new PlayerCredentials(
                userName: userName,
                password: password.Encrypt());
        }

        #endregion
    }
}
