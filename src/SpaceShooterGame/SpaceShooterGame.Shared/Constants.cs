namespace SpaceShooterGame
{
    public static class Constants
    {
        #region Fields

        public const string GAME_ID = "astro-odyssey";

        #region Measurements

        public const double DEFAULT_FRAME_TIME = 18;

        public const double DESTRUCTIBLE_OBJECT_SIZE = 70;
        public const double COLLECTIBLE_OBJECT_SIZE = 90;
        public const double PICKUP_OBJECT_SIZE = 80;

        public const double PLAYER_HEIGHT = 80;
        public const double PLAYER_SPEED = 11;
        public const double BOSS_BASE_HEALTH = 25;
        public const double SCORE_MULTIPLIER_THREASHOLD = 7;

        public const double POWER_UP_METER = 100; 

        #endregion

        #region Tags

        public const string PLAYER_TAG = "player";

        public const string ENEMY_TAG = "enemy";
        public const string METEOR_TAG = "meteor";

        public const string HEALTH_TAG = "health";
        public const string POWERUP_TAG = "powerup";
        public const string COLLECTIBLE_TAG = "collectible";

        public const string PLAYER_PROJECTILE_TAG = "player_projectile";
        public const string ENEMY_PROJECTILE_TAG = "enemy_projectile";

        public const string STAR_TAG = "star";

        #endregion

        #region Web Api Base Urls
#if DEBUG
        public const string GAME_API_BASEURL = "https://localhost:7238";
#else
        public const string GAME_API_BASEURL = "https://astro-odyssey-web-api.herokuapp.com";
#endif
        #endregion

        #region Web Api Endpoints

        public const string Action_Ping = "/api/Query/Ping";

        public const string Action_Authenticate = "/api/Command/Authenticate";
        public const string Action_SignUp = "/api/Command/SignUp";
        public const string Action_SubmitGameScore = "/api/Command/SubmitGameScore";
        public const string Action_GenerateSession = "/api/Command/GenerateSession";
        public const string Action_ValidateSession = "/api/Command/ValidateSession";

        public const string Action_GetGameProfile = "/api/Query/GetGameProfile";
        public const string Action_GetGameProfiles = "/api/Query/GetGameProfiles";
        public const string Action_GetGameScores = "/api/Query/GetGameScores";
        public const string Action_GetUser = "/api/Query/GetUser";

        #endregion

        #region Session Keys

        public const string CACHE_SESSION_KEY = "Session";
        public const string CACHE_LANGUAGE_KEY = "Language";

        #endregion

        #region Cookie Keys

        public const string COOKIE_KEY = "Cookie";
        public const string COOKIE_ACCEPTED_KEY = "Accepted"; 

        #endregion

        #endregion
    }
}
