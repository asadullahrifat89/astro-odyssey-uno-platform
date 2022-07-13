using Windows.Foundation;

namespace AstroOdyssey
{
    public static class Constants
    {
        public const string PLAYER = "player";

        public const string ENEMY = "enemy";
        public const string METEOR = "meteor";

        public const string HEALTH = "health";
        public const string POWERUP = "powerup";

        public const string LASER = "laser";

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
            LASER_FIRE,
            LASER_HIT,
            LASER_FIRE_POWERED_UP,
            ENEMY_DESTRUCTION,
            METEOR_DESTRUCTION,
            POWER_UP,
            POWER_DOWN,
            LEVEL_UP,
            HEALTH_GAIN,
            HEALTH_LOSS,
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

            var sourceWidth = source.Width - 5;
            var sourceHeight = source.Height - 5;

            var targetWidth = target.Width - 5;
            var targetHeight = target.Height - 5;

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
