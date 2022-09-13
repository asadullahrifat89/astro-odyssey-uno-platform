using System;

namespace AstroOdyssey
{
    public static class GameObjectTemplates
    {
        #region Images

        public static (Uri AssetUri, ShipClass ShipClass)[] PLAYER_RAGE_TEMPLATES = new (Uri AssetUri, ShipClass ShipClass)[]
        {
             (new Uri("ms-appx:///Assets/Images/Rage/rage1.png", UriKind.RelativeOrAbsolute), ShipClass.DEFENDER),
             (new Uri("ms-appx:///Assets/Images/Rage/rage2.png", UriKind.RelativeOrAbsolute), ShipClass.BERSERKER),
             (new Uri("ms-appx:///Assets/Images/Rage/rage3.png", UriKind.RelativeOrAbsolute), ShipClass.SPECTRE),
        };

        public static CelestialObjectTemplate[] STAR_TEMPLATES = new CelestialObjectTemplate[]
        {
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Star/star1.png", UriKind.RelativeOrAbsolute), size : 25),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Star/star2.png", UriKind.RelativeOrAbsolute), size : 25),
            
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Star/gas_star1.png", UriKind.RelativeOrAbsolute), size : 30),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Star/gas_star2.png", UriKind.RelativeOrAbsolute), size : 30),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Star/gas_star3.png", UriKind.RelativeOrAbsolute), size : 30),

            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Star/nebula1.png", UriKind.RelativeOrAbsolute), size : 130),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Star/nebula2.png", UriKind.RelativeOrAbsolute), size : 130),

            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Star/galaxy1.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Star/galaxy2.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Star/galaxy3.png", UriKind.RelativeOrAbsolute), size: 200),

            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Star/asteroid1.png", UriKind.RelativeOrAbsolute), size: 250),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Star/asteroid2.png", UriKind.RelativeOrAbsolute), size: 250),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Star/asteroid3.png", UriKind.RelativeOrAbsolute), size: 250),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Star/asteroid4.png", UriKind.RelativeOrAbsolute), size : 250),
        };

        public static CelestialObjectTemplate[] PLANET_TEMPLATES = new CelestialObjectTemplate[]
        {
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Planet/black-hole2.png", UriKind.RelativeOrAbsolute), size: 1400),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet1.png", UriKind.RelativeOrAbsolute), size : 1400),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet2.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet3.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet4.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet5.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet6.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet7.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet8.png", UriKind.RelativeOrAbsolute), size : 700),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/Planet/planet9.png", UriKind.RelativeOrAbsolute), size : 700),
        };

        public static DestructibleObjectTemplate[] ENEMY_TEMPLATES = new DestructibleObjectTemplate[]
        {
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Enemy/enemy_1.png", UriKind.RelativeOrAbsolute), health: 1),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Enemy/enemy_2.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Enemy/enemy_3.png", UriKind.RelativeOrAbsolute), health: 3),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Enemy/enemy_4.png", UriKind.RelativeOrAbsolute), health: 1),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Enemy/enemy_5.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Enemy/enemy_6.png", UriKind.RelativeOrAbsolute), health: 3),
        };

        public static (Uri AssetUri, BossClass BossClass)[] BOSS_TEMPLATES = new (Uri AssetUri, BossClass BossClass)[]
        {
            (new Uri("ms-appx:///Assets/Images/Boss/boss1.png", UriKind.RelativeOrAbsolute), BossClass.GREEN),
            (new Uri("ms-appx:///Assets/Images/Boss/boss2.png", UriKind.RelativeOrAbsolute), BossClass.CRIMSON),
            (new Uri("ms-appx:///Assets/Images/Boss/boss3.png", UriKind.RelativeOrAbsolute), BossClass.BLUE),
        };

        public static DestructibleObjectTemplate[] METEOR_TEMPLATES = new DestructibleObjectTemplate[]
        {
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Meteor/rock1.png", UriKind.RelativeOrAbsolute), health: 1),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Meteor/rock2.png", UriKind.RelativeOrAbsolute), health: 3),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Meteor/rock3.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/Meteor/rock4.png", UriKind.RelativeOrAbsolute), health: 2),
        };

        public static PlayerShipTemplate[] PLAYER_SHIP_TEMPLATES = new PlayerShipTemplate[]
        {
            new PlayerShipTemplate(name: "Antimony", assetUri: "ms-appx:///Assets/Images/Player/player_ship1.png", shipClass: ShipClass.DEFENDER),
            new PlayerShipTemplate(name: "Bismuth", assetUri: "ms-appx:///Assets/Images/Player/player_ship2.png", shipClass: ShipClass.BERSERKER),
            new PlayerShipTemplate(name: "Curium", assetUri: "ms-appx:///Assets/Images/Player/player_ship3.png", shipClass: ShipClass.SPECTRE),
        };

        public static (Uri AssetUri, ShipClass ShipClass)[] PLAYER_SHIP_THRUST_TEMPLATES = new (Uri AssetUri, ShipClass ShipClass)[]
        {
             (new Uri("ms-appx:///Assets/Images/Player/space_thrust1.png", UriKind.RelativeOrAbsolute), ShipClass.DEFENDER),
             (new Uri("ms-appx:///Assets/Images/Player/space_thrust2.png", UriKind.RelativeOrAbsolute), ShipClass.BERSERKER),
             (new Uri("ms-appx:///Assets/Images/Player/space_thrust3.png", UriKind.RelativeOrAbsolute), ShipClass.SPECTRE),
        };

        public static Uri[] COLLECTIBLE_TEMPLATES = new Uri[]
        {
            new Uri("ms-appx:///Assets/Images/Collectible/pizza1.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/Collectible/pizza2.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/Collectible/pizza3.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/Collectible/pizza4.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/Collectible/pizza5.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/Collectible/pizza6.png", UriKind.RelativeOrAbsolute),
        };

        public static Uri[] GAME_MISC_IMAGE_TEMPLATES = new Uri[]
        {
            new Uri("ms-appx:///Assets/Images/boss_appeared.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/boss_cleared.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/gameOver.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/health.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/powerup.png", UriKind.RelativeOrAbsolute),
        };

        #endregion

        #region Sounds

        public static string[] BACKGROUND_MUSIC_TEMPLATES = new string[]
        {
            "Assets/Sounds/Music/action-stylish-rock-dedication-15038.mp3",
            "Assets/Sounds/Music/electronic-rock-king-around-here-15045.mp3",
            "Assets/Sounds/Music/powerful-stylish-stomp-rock-lets-go-114255.mp3",
            "Assets/Sounds/Music/stomping-rock-four-shots-111444.mp3",
            "Assets/Sounds/Music/stylish-rock-beat-trailer-116346.mp3",
            "Assets/Sounds/Music/modern-fashion-promo-rock-18397.mp3",
            "Assets/Sounds/Music/hard-rock-21056.mp3",
            "Assets/Sounds/Music/crag-hard-rock-14401.mp3",
           //"Assets/Sounds/Music/rockstar-trailer-109945.mp3",
        };

        public static string[] BOSS_APPEARANCE_MUSIC_TEMPLATES = new string[]
        {
            //"Assets/Sounds/Boss/despair-metal-trailer-109943.mp3",
            "Assets/Sounds/Boss/frantic-15190.mp3",
            "Assets/Sounds/Boss/metal-dark-matter-111451.mp3",
            "Assets/Sounds/Boss/rage-15292.mp3",
        };

        public static string[] GAME_INTRO_MUSIC_TEMPLATES = new string[]
        {
            "Assets/Sounds/Intro/fashion-hip-hop-rock-stylish-boy-111449.mp3",
            "Assets/Sounds/Intro/inspiring-motivational-rock-inspire-mesenses-111448.mp3",
        }; 

        #endregion
    }
}
