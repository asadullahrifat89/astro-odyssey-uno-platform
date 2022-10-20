using System;
using System.Collections.Generic;

namespace SpaceShooterGame
{
    public static class Constants
    {
        #region Fields

        public const string GAME_ID = "space-shooter";

        #region Measurements

        public const double DEFAULT_FRAME_TIME = 18;

        public const double DESTRUCTIBLE_OBJECT_SIZE = 60 * 1.5;
        public const double COLLECTIBLE_OBJECT_SIZE = 80 * 1.5;
        public const double POWERUP_OBJECT_SIZE = 70 * 1.5;
        public const double PLAYER_HEIGHT = 70 * 1.5;

        public const double PLAYER_SPEED = 11;
        public const double BOSS_BASE_HEALTH = 25;
        public const double SCORE_MULTIPLIER_THREASHOLD = 8;

        public const double POWER_UP_METER = 100;

        #endregion

        #region Images

        public static (ImageType ImageType, Uri AssetUri, double Size)[] IMAGE_TEMPLATES = new (ImageType ImageType, Uri AssetUri, double Size)[]
        {
            new(ImageType.STAR, new Uri("ms-appx:///Assets/Images/Star/star1.png"), 25),
            new(ImageType.STAR, new Uri("ms-appx:///Assets/Images/Star/star2.png"), 25),
            new(ImageType.STAR, new Uri("ms-appx:///Assets/Images/Star/star3.png"), 130),
            new(ImageType.STAR, new Uri("ms-appx:///Assets/Images/Star/star4.png"), 130),
            new(ImageType.STAR, new Uri("ms-appx:///Assets/Images/Star/star5.png"), 30),

            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet1.png"), 1500),
            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet2.png"), 800),
            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet3.png"), 800),
            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet4.png"), 800),
            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet5.png"), 800),
            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet6.png"), 800),
            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet7.png"), 800),
            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet8.png"), 800),
            new(ImageType.PLANET, new Uri("ms-appx:///Assets/Images/Planet/planet9.png"), 800),

            new(ImageType.COLLECTIBLE, new Uri("ms-appx:///Assets/Images/Collectible/collectible1.png"), COLLECTIBLE_OBJECT_SIZE),
            new(ImageType.COLLECTIBLE, new Uri("ms-appx:///Assets/Images/Collectible/collectible2.png"), COLLECTIBLE_OBJECT_SIZE),
            new(ImageType.COLLECTIBLE, new Uri("ms-appx:///Assets/Images/Collectible/collectible3.png"), COLLECTIBLE_OBJECT_SIZE),
            new(ImageType.COLLECTIBLE, new Uri("ms-appx:///Assets/Images/Collectible/collectible4.png"), COLLECTIBLE_OBJECT_SIZE),
            new(ImageType.COLLECTIBLE, new Uri("ms-appx:///Assets/Images/Collectible/collectible5.png"), COLLECTIBLE_OBJECT_SIZE),

            new(ImageType.BOSS, new Uri("ms-appx:///Assets/Images/Boss/boss1.png"), 1),
            new(ImageType.BOSS, new Uri("ms-appx:///Assets/Images/Boss/boss2.png"), 1),
            new(ImageType.BOSS, new Uri("ms-appx:///Assets/Images/Boss/boss3.png"), 1),

            new(ImageType.BOSS_APPEARED, new Uri("ms-appx:///Assets/Images/Boss/boss_appeared.png"), 1),
            new(ImageType.BOSS_CLEARED, new Uri("ms-appx:///Assets/Images/Boss/boss_cleared.png"), 1),

            new(ImageType.ENEMY, new Uri("ms-appx:///Assets/Images/Enemy/enemy1.png"), DESTRUCTIBLE_OBJECT_SIZE),
            new(ImageType.ENEMY, new Uri("ms-appx:///Assets/Images/Enemy/enemy2.png"), DESTRUCTIBLE_OBJECT_SIZE),
            new(ImageType.ENEMY, new Uri("ms-appx:///Assets/Images/Enemy/enemy3.png"), DESTRUCTIBLE_OBJECT_SIZE),
            new(ImageType.ENEMY, new Uri("ms-appx:///Assets/Images/Enemy/enemy4.png"), DESTRUCTIBLE_OBJECT_SIZE),
            new(ImageType.ENEMY, new Uri("ms-appx:///Assets/Images/Enemy/enemy5.png"), DESTRUCTIBLE_OBJECT_SIZE),
            new(ImageType.ENEMY, new Uri("ms-appx:///Assets/Images/Enemy/enemy6.png"), DESTRUCTIBLE_OBJECT_SIZE),

            new(ImageType.METEOR, new Uri("ms-appx:///Assets/Images/Meteor/meteor1.png"), DESTRUCTIBLE_OBJECT_SIZE),
            new(ImageType.METEOR, new Uri("ms-appx:///Assets/Images/Meteor/meteor2.png"), DESTRUCTIBLE_OBJECT_SIZE),
            new(ImageType.METEOR, new Uri("ms-appx:///Assets/Images/Meteor/meteor3.png"), DESTRUCTIBLE_OBJECT_SIZE),
            new(ImageType.METEOR, new Uri("ms-appx:///Assets/Images/Meteor/meteor4.png"), DESTRUCTIBLE_OBJECT_SIZE),

            new(ImageType.PLAYER_SHIP, new Uri("ms-appx:///Assets/Images/Player/player_ship1.png"), (int)ShipClass.DEFENDER),
            new(ImageType.PLAYER_SHIP, new Uri("ms-appx:///Assets/Images/Player/player_ship2.png"), (int)ShipClass.BERSERKER),
            new(ImageType.PLAYER_SHIP, new Uri("ms-appx:///Assets/Images/Player/player_ship3.png"), (int)ShipClass.SPECTRE),

            new(ImageType.PLAYER_SHIP_THRUST, new Uri("ms-appx:///Assets/Images/Player/space_thrust1.png"), (int)ShipClass.DEFENDER),
            new(ImageType.PLAYER_SHIP_THRUST, new Uri("ms-appx:///Assets/Images/Player/space_thrust2.png"), (int)ShipClass.BERSERKER),
            new(ImageType.PLAYER_SHIP_THRUST, new Uri("ms-appx:///Assets/Images/Player/space_thrust3.png"), (int)ShipClass.SPECTRE),

            new(ImageType.PLAYER_RAGE, new Uri("ms-appx:///Assets/Images/Rage/rage1.png"), (int)ShipClass.DEFENDER),
            new(ImageType.PLAYER_RAGE, new Uri("ms-appx:///Assets/Images/Rage/rage2.png"), (int)ShipClass.BERSERKER),
            new(ImageType.PLAYER_RAGE, new Uri("ms-appx:///Assets/Images/Rage/rage3.png"), (int)ShipClass.SPECTRE),

            new(ImageType.HEALTH, new Uri("ms-appx:///Assets/Images/Health/health.png"), 1),
            new(ImageType.POWERUP, new Uri("ms-appx:///Assets/Images/Powerup/powerup.png"), 1),

            new(ImageType.SCORE_MULTIPLIER, new Uri("ms-appx:///Assets/Images/score_multiplier.png"), 1),

            new(ImageType.GAME_INTRO, new Uri("ms-appx:///Assets/Images/game_intro.gif"), 1),
            new(ImageType.GAME_OVER, new Uri("ms-appx:///Assets/Images/game_over.png"), 1),
        };

        #endregion

        #region Sounds

        public static KeyValuePair<SoundType, string>[] SOUND_TEMPLATES = new KeyValuePair<SoundType, string>[]
        {
            new KeyValuePair<SoundType, string>(SoundType.MENU_SELECT, "Assets/Sounds/menu_select.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.INTRO, "Assets/Sounds/Intro/intro1.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.INTRO, "Assets/Sounds/Intro/intro2.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.BACKGROUND, "Assets/Sounds/Music/music1.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.BACKGROUND, "Assets/Sounds/Music/music2.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.BACKGROUND, "Assets/Sounds/Music/music3.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.BACKGROUND, "Assets/Sounds/Music/music4.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.BACKGROUND, "Assets/Sounds/Music/music5.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.GAME_START, "Assets/Sounds/Game/game_start.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.GAME_OVER, "Assets/Sounds/Game/game_over.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.POWER_UP, "Assets/Sounds/Power/power_up.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.POWER_DOWN, "Assets/Sounds/Power/power_down.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.HEALTH_GAIN, "Assets/Sounds/Health/health_gain.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.HEALTH_LOSS, "Assets/Sounds/Health/health_loss.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.COLLECTIBLE_COLLECTED, "Assets/Sounds/Collectible/collectible.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.BOSS_APPEARANCE, "Assets/Sounds/Boss/boss1.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.BOSS_APPEARANCE, "Assets/Sounds/Boss/boss2.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.BOSS_APPEARANCE, "Assets/Sounds/Boss/boss3.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.BOSS_DESTRUCTION, "Assets/Sounds/Boss/boss-destruction.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.PLAYER_ROUNDS_FIRE, "Assets/Sounds/Player/player_rounds_fire.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE, "Assets/Sounds/Player/player_blaze_blitz.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE, "Assets/Sounds/Player/player_plasma_bomb.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.PLAYER_BEAM_CANNON_ROUNDS_FIRE, "Assets/Sounds/Player/player_beam_cannon.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.PLAYER_SONIC_BLAST_ROUNDS_FIRE, "Assets/Sounds/Player/player_sonic_blast.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.ROUNDS_HIT, "Assets/Sounds/rounds_hit.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.RAGE_UP, "Assets/Sounds/Rage/rage_up.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.RAGE_DOWN, "Assets/Sounds/Rage/rage_down.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.ENEMY_INCOMING, "Assets/Sounds/Enemy/enemy_incoming.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.ENEMY_ROUNDS_FIRE, "Assets/Sounds/Enemy/enemy_rounds_fire.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.ENEMY_DESTRUCTION, "Assets/Sounds/Meteor/meteor_destruction.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.METEOR_DESTRUCTION, "Assets/Sounds/Meteor/meteor_destruction.mp3"),

            new KeyValuePair<SoundType, string>(SoundType.SCORE_MULTIPLIER_ON, "Assets/Sounds/Collectible/score_multiplier_on.mp3"),
            new KeyValuePair<SoundType, string>(SoundType.SCORE_MULTIPLIER_OFF, "Assets/Sounds/Collectible/score_multiplier_off.mp3"),
        };

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
