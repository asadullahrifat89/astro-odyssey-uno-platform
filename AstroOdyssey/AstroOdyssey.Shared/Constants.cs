using Microsoft.UI;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Globalization;
using Windows.Foundation;
using Windows.UI;

namespace AstroOdyssey
{
    public static class Constants
    {
        public const double DESTRUCTIBLE_OBJECT_SIZE = 70;
        public const double PLAYER_HEIGHT = 70;

        public const string PLAYER = "player";

        public const string ENEMY = "enemy";
        public const string METEOR = "meteor";

        public const string HEALTH = "health";
        public const string POWERUP = "powerup";

        public const string PLAYER_PROJECTILE = "player_projectile";
        public const string ENEMY_PROJECTILE = "enemy_projectile";

        public const string STAR = "star";

        //public static Color SPECIAL_ROUNDS_COLOR = Colors.White; //Windows.UI.ColorHelper.FromArgb(255, 232, 18, 36);

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
            Level_9,
            Level_10,
        }

        public enum SoundType
        {
            BACKGROUND_MUSIC,
            PLAYER_ROUNDS_FIRE,
            PLAYER_RAPIDSHOT_ROUNDS_FIRE,
            PLAYER_DEADSHOT_ROUNDS_FIRE,
            PLAYER_SONICSHOT_ROUNDS_FIRE,
            ENEMY_ROUNDS_FIRE,
            ENEMY_DESTRUCTION,
            METEOR_DESTRUCTION,
            BOSS_APPEARANCE,
            BOSS_DESTRUCTION,
            ROUNDS_HIT,
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

            if (source.Width >= 0.0 && target.Width >= 0.0
                && targetX <= sourceX + sourceWidth && targetX + targetWidth >= sourceX
                && targetY <= sourceY + sourceHeight)
            {
                return targetY + targetHeight >= sourceY;
            }

            return false;
        }
    }
}
