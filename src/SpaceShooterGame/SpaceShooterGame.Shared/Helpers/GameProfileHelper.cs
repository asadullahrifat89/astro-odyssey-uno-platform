namespace SpaceShooterGame
{
    public static class GameProfileHelper
    {
        #region Fields

        public static GameProfile GameProfile { get; set; }

        #endregion

        #region Methods

        public static bool HasUserLoggedIn()
        {
            return GameProfile is not null && GameProfile.User is not null && !GameProfile.User.UserId.IsNullOrBlank() && !GameProfile.User.UserName.IsNullOrBlank();
        }

        #endregion
    }
}
