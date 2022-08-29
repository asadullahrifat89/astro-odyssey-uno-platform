using System;
using Windows.Foundation;

namespace AstroOdyssey
{
    public static class Constants
    {
        #region Fields

        public const double DESTRUCTIBLE_OBJECT_SIZE = 70;
        public const double PLAYER_HEIGHT = 80;

        public const string PLAYER = "player";

        public const string ENEMY = "enemy";
        public const string METEOR = "meteor";

        public const string HEALTH = "health";
        public const string POWERUP = "powerup";

        public const string PLAYER_PROJECTILE = "player_projectile";
        public const string ENEMY_PROJECTILE = "enemy_projectile";

        public const string STAR = "star";

        public const double RAGE_THRESHOLD = 25;
        public const double POWER_UP_METER = 100;

        public static CelestialObjectTemplate[] STAR_TEMPLATES = new CelestialObjectTemplate[]
        {
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star1.png", UriKind.RelativeOrAbsolute), size : 25),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star2.png", UriKind.RelativeOrAbsolute), size : 25),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star3.png", UriKind.RelativeOrAbsolute), size : 20),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star4.png", UriKind.RelativeOrAbsolute), size : 20),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star5.png", UriKind.RelativeOrAbsolute), size : 20),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star6.png", UriKind.RelativeOrAbsolute), size : 20),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star7.png", UriKind.RelativeOrAbsolute), size : 20),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star8.png", UriKind.RelativeOrAbsolute), size : 20),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star9.png", UriKind.RelativeOrAbsolute), size : 20),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star10.png", UriKind.RelativeOrAbsolute), size : 20),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star1.png", UriKind.RelativeOrAbsolute), size : 30),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star2.png", UriKind.RelativeOrAbsolute), size : 30),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star3.png", UriKind.RelativeOrAbsolute), size : 30),           
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/nebula1.png", UriKind.RelativeOrAbsolute), size : 130),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/nebula2.png", UriKind.RelativeOrAbsolute), size : 130),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy1.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy2.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy3.png", UriKind.RelativeOrAbsolute), size: 200),
            new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid1.png", UriKind.RelativeOrAbsolute), size: 250),
            new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid2.png", UriKind.RelativeOrAbsolute), size: 250),
            new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid3.png", UriKind.RelativeOrAbsolute), size: 250),
            new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid4.png", UriKind.RelativeOrAbsolute), size : 250),
        };

        public static CelestialObjectTemplate[] PLANET_TEMPLATES = new CelestialObjectTemplate[]
        {
            new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/black-hole2.png", UriKind.RelativeOrAbsolute), size: 1400),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet1.png", UriKind.RelativeOrAbsolute), size : 1400),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet2.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet3.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet4.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet5.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet6.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet7.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet8.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/planet9.png", UriKind.RelativeOrAbsolute), size : 700),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/sun1.png", UriKind.RelativeOrAbsolute), size : 700),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/sun2.png", UriKind.RelativeOrAbsolute), size : 700),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/sun3.png", UriKind.RelativeOrAbsolute), size : 700),
        };

        public static DestructibleObjectTemplate[] ENEMY_TEMPLATES = new DestructibleObjectTemplate[]
        {
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/enemy_1.png", UriKind.RelativeOrAbsolute), health: 1),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/enemy_2.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/enemy_3.png", UriKind.RelativeOrAbsolute), health: 3),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/enemy_4.png", UriKind.RelativeOrAbsolute), health: 1),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/enemy_5.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/enemy_6.png", UriKind.RelativeOrAbsolute), health: 3),
        };

        public static DestructibleObjectTemplate[] METEOR_TEMPLATES = new DestructibleObjectTemplate[]
        {
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock1.png", UriKind.RelativeOrAbsolute), health: 1),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock2.png", UriKind.RelativeOrAbsolute), health: 1),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock3.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock4.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock5.png", UriKind.RelativeOrAbsolute), health: 3),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock6.png", UriKind.RelativeOrAbsolute), health: 3),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock7.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock8.png", UriKind.RelativeOrAbsolute), health: 3),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock9.png", UriKind.RelativeOrAbsolute), health: 1),
        };

        public static PlayerShipTemplate[] PLAYER_SHIP_TEMPLATES = new PlayerShipTemplate[]
        {
            new PlayerShipTemplate(name: "Antimony", assetUri: "ms-appx:///Assets/Images/player_ship1.png", shipClass: ShipClass.DEFENDER),
            new PlayerShipTemplate(name: "Bismuth", assetUri: "ms-appx:///Assets/Images/player_ship2.png", shipClass: ShipClass.BERSERKER),
            new PlayerShipTemplate(name: "Curium", assetUri: "ms-appx:///Assets/Images/player_ship3.png", shipClass: ShipClass.SPECTRE),
        };

        public static LocalizationTemplate[] LOCALIZATION_TEMPLATES = new LocalizationTemplate[]
        {
            new LocalizationTemplate(key: "ApplicationName", cultureValues: new (string Culture, string Value)[]{ new ("en", "AstroOdyssey") }),
            new LocalizationTemplate(key: "Language", cultureValues: new (string Culture, string Value)[]{ new ("en", "en") }),

            new LocalizationTemplate(key: "Antimony", cultureValues: new (string Culture, string Value)[]{ new ("en", "Antimony") }),
            new LocalizationTemplate(key: "Bismuth", cultureValues: new (string Culture, string Value)[]{ new ("en", "Bismuth") }),
            new LocalizationTemplate(key: "Curium", cultureValues: new (string Culture, string Value)[]{ new ("en", "Curium") }),

            new LocalizationTemplate(key: "BEAM_CANNON", cultureValues: new (string Culture, string Value)[]{ new ("en", "BEAM CANNON") }),
            new LocalizationTemplate(key: "BLAZE_BLITZ", cultureValues: new (string Culture, string Value)[]{ new ("en", "BLAZE BLITZ") }),
            new LocalizationTemplate(key: "BOSS", cultureValues: new (string Culture, string Value)[]{ new ("en", "BOSS") }),
            new LocalizationTemplate(key: "CLOAK_DOWN", cultureValues: new (string Culture, string Value)[]{ new ("en", "CLOAK DOWN") }),
            new LocalizationTemplate(key: "CLOAK_UP", cultureValues: new (string Culture, string Value)[]{ new ("en", "CLOAK UP") }),
            new LocalizationTemplate(key: "COMPLETE", cultureValues: new (string Culture, string Value)[]{ new ("en", "COMPLETE") }),
            new LocalizationTemplate(key: "ENEMY_APPROACHES", cultureValues: new (string Culture, string Value)[]{ new ("en", "ENEMY APPROACHES") }),
            new LocalizationTemplate(key: "FANTASTIC_GAME", cultureValues: new (string Culture, string Value)[]{ new ("en", "Fantastic game") }),
            new LocalizationTemplate(key: "FIREPOWER_DOWN", cultureValues: new (string Culture, string Value)[]{ new ("en", "FIREPOWER DOWN") }),
            new LocalizationTemplate(key: "FIREPOWER_UP", cultureValues: new (string Culture, string Value)[]{ new ("en", "FIREPOWER UP") }),
            new LocalizationTemplate(key: "GAME_PAUSED", cultureValues: new (string Culture, string Value)[]{ new ("en", "GAME PAUSED") }),
            new LocalizationTemplate(key: "GOOD_GAME", cultureValues: new (string Culture, string Value)[]{ new ("en", "Good game") }),
            new LocalizationTemplate(key: "GREAT_GAME", cultureValues: new (string Culture, string Value)[]{ new ("en", "Great game") }),
            new LocalizationTemplate(key: "LEVEL", cultureValues: new (string Culture, string Value)[]{ new ("en", "LEVEL") }),
            new LocalizationTemplate(key: "NO_LUCK", cultureValues: new (string Culture, string Value)[]{ new ("en", "No luck") }),
            new LocalizationTemplate(key: "PLASMA_BOMB", cultureValues: new (string Culture, string Value)[]{ new ("en", "PLASMA BOMB") }),
            new LocalizationTemplate(key: "POWER_DOWN", cultureValues: new (string Culture, string Value)[]{ new ("en", "POWER DOWN") }),
            new LocalizationTemplate(key: "QUIT_GAME", cultureValues: new (string Culture, string Value)[]{ new ("en", "QUIT GAME?") }),
            new LocalizationTemplate(key: "SCORE", cultureValues: new (string Culture, string Value)[]{ new ("en", "Score") }),
            new LocalizationTemplate(key: "SHIELD_DOWN", cultureValues: new (string Culture, string Value)[]{ new ("en", "SHIELD DOWN") }),
            new LocalizationTemplate(key: "SHIELD_UP", cultureValues: new (string Culture, string Value)[]{ new ("en", "SHIELD UP") }),
            new LocalizationTemplate(key: "SHIP_REPAIRED", cultureValues: new (string Culture, string Value)[]{ new ("en", "SHIP REPAIRED") }),
            new LocalizationTemplate(key: "SONIC_BLAST", cultureValues: new (string Culture, string Value)[]{ new ("en", "SONIC BLAST") }),
            new LocalizationTemplate(key: "SUPREME_GAME", cultureValues: new (string Culture, string Value)[]{ new ("en", "Supreme game") }),
            new LocalizationTemplate(key: "TAP_ON_SCREEN_TO_BEGIN", cultureValues: new (string Culture, string Value)[]{ new ("en", "TAP ON THE SCREEN TO BEGIN") }),
            new LocalizationTemplate(key: "TAP_TO_QUIT", cultureValues: new (string Culture, string Value)[]{ new ("en", "TAP TO QUIT") }),
            new LocalizationTemplate(key: "TAP_TO_RESUME", cultureValues: new (string Culture, string Value)[]{ new ("en", "TAP TO RESUME") }),

            new LocalizationTemplate(key: "GameOverPage_PlayAgainButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "PLAY AGAIN") }),
            new LocalizationTemplate(key: "GameOverPage_Tagline.Text", cultureValues: new (string Culture, string Value)[]{ new ("en", "GAME OVER") }),
            new LocalizationTemplate(key: "GameStartPage_AssetsCreditButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "Assets by redfoc.com 😘") }),
            new LocalizationTemplate(key: "GameStartPage_BanglaButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "Bangla") }),
            new LocalizationTemplate(key: "GameStartPage_DeutschButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "Deutsch") }),
            new LocalizationTemplate(key: "GameStartPage_DeveloperProfileButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "Made with ❤️ by Asadullah Rifat") }),
            new LocalizationTemplate(key: "GameStartPage_EnglishButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "English") }),
            new LocalizationTemplate(key: "GameStartPage_FrenchButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "French") }),
            new LocalizationTemplate(key: "GameStartPage_PlayButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "START GAME") }),
            new LocalizationTemplate(key: "GameStartPage_Tagline.Text", cultureValues: new (string Culture, string Value)[]{ new ("en", "A classic rock metal ⚡ space shooter on WebAssembly.") }),
            new LocalizationTemplate(key: "ShipSelectionPage_ChooseButton.Content", cultureValues: new (string Culture, string Value)[]{ new ("en", "SELECT") }),
            new LocalizationTemplate(key: "ShipSelectionPage_ControlInstructions.Text", cultureValues: new (string Culture, string Value)[]{ new ("en", "Press or touch left or right arrow keys or edges on your keyboard or screen to move the ship.") }),
            new LocalizationTemplate(key: "ShipSelectionPage_Tagline.Text", cultureValues: new (string Culture, string Value)[]{ new ("en", "Select a Ship") }),


        };

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
            string[] nameSplit = name.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

            string initials = "";

            foreach (string item in nameSplit)
            {
                initials += item.Substring(0, 1).ToUpper();
            }

            return initials;
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
