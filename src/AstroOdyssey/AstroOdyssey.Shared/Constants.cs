using System;
using Windows.Foundation;

namespace AstroOdyssey
{
    public static class Constants
    {
        #region Fields

        public const double DESTRUCTIBLE_OBJECT_SIZE = 70;
        public const double COLLECTIBLE_OBJECT_SIZE = 90;
        public const double PICKUP_OBJECT_SIZE = 80;

        public const double PLAYER_HEIGHT = 80;

        public const string PLAYER = "player";

        public const string ENEMY = "enemy";
        public const string METEOR = "meteor";

        public const string HEALTH = "health";
        public const string POWERUP = "powerup";
        public const string COLLECTIBLE = "collectible";

        public const string PLAYER_PROJECTILE = "player_projectile";
        public const string ENEMY_PROJECTILE = "enemy_projectile";

        public const string STAR = "star";

        public const double RAGE_THRESHOLD = 25;
        public const double POWER_UP_METER = 100;

#if DEBUG
        public const string GAME_API_BASEURL = "https://localhost:7238";
#else
        public const string GAME_API_BASEURL = "https://astro-odyssey-web-api.herokuapp.com";
#endif
        public const string GAME_ID = "astro-odyssey";

        public const string Action_Ping = "/api/Query/Ping";

        public const string Action_Authenticate = "/api/Command/Authenticate";
        public const string Action_SignUp = "/api/Command/SignUp";

        public const string Action_SubmitGameScore = "/api/Command/SubmitGameScore";

        public const string Action_GetGameProfile = "/api/Query/GetGameProfile";
        public const string Action_GetGameProfiles = "/api/Query/GetGameProfiles";
        public const string Action_GetGameScores = "/api/Query/GetGameScores";        
        public const string Action_GetUser = "/api/Query/GetUser";
       
        #endregion

        #region Methods

        /// <summary>
        /// Checks if the provided string is null or empty or white space.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrBlank(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Get initials from a given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetInitials(string name)
        {
            string[] nameSplit = name.Split(new string[] { ",", ".", " " }, StringSplitOptions.RemoveEmptyEntries);

            string initials = "";

            foreach (string item in nameSplit)
            {
                initials += item.Substring(0, 1).ToUpper();
            }

            return initials.ToUpperInvariant();
        }

        /// <summary>
        /// Checks if a two rects intersect.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Intersects(this Rect source, Rect target)
        {
            var targetX = target.X;
            var targetY = target.Y;
            var sourceX = source.X;
            var sourceY = source.Y;

            var sourceWidth = source.Width;
            var sourceHeight = source.Height;

            var targetWidth = target.Width;
            var targetHeight = target.Height;

            if (source.Width >= 0.0 && target.Width >= 0.0
                && targetX <= sourceX + sourceWidth && targetX + targetWidth >= sourceX
                && targetY <= sourceY + sourceHeight)
            {
                return targetY + targetHeight >= sourceY;
            }

            return false;
        }

        #endregion
    }
}
