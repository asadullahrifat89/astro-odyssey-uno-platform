using Windows.Foundation;

namespace AstroOdyssey
{
    public static class Constants
    {
        public const double DefaultGameObjectSize = 70;
        public const double DefaultPlayerHeight = 130;

        public const string PLAYER = "player";

        public const string ENEMY = "enemy";
        public const string METEOR = "meteor";

        public const string HEALTH = "health";
        public const string POWERUP = "powerup";

        public const string PROJECTILE = "projectile";

        public const string STAR = "star";

        public enum GameLevel
        {
            Level_1,
            Level_2,
            Level_3,
            Level_4,
            Level_5,
            Level_6,
            Level_7,
            Level_8,
        }

        public enum SoundType
        {
            BACKGROUND_MUSIC,
            ROUNDS_FIRE,
            ROUNDS_HIT,
            RAPIDSHOT_ROUNDS_FIRE,
            DEADSHOT_ROUNDS_FIRE,
            SONICSHOT_ROUNDS_FIRE,
            ENEMY_DESTRUCTION,
            METEOR_DESTRUCTION,
            POWER_UP,
            POWER_DOWN,
            LEVEL_UP,
            HEALTH_GAIN,
            HEALTH_LOSS,
            GAME_START,
            GAME_OVER
        }

        public static bool IsNullOrBlank(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
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

            if (source.Width >= 0.0
                && target.Width >= 0.0
                && targetX <= sourceX + sourceWidth
                && targetX + targetWidth >= sourceX
                && targetY <= sourceY + sourceHeight)
            {
                return targetY + targetHeight >= sourceY;
            }

            return false;
        }
    }
}
