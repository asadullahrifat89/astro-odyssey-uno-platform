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

        public static CelestialObjectTemplate[] STAR_TEMPLATES = new CelestialObjectTemplate[]
        {
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star1.png", UriKind.RelativeOrAbsolute), size : 25),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star2.png", UriKind.RelativeOrAbsolute), size : 25),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star3.png", UriKind.RelativeOrAbsolute), size : 20),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star4.png", UriKind.RelativeOrAbsolute), size : 20),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star5.png", UriKind.RelativeOrAbsolute), size : 20),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star6.png", UriKind.RelativeOrAbsolute), size : 20),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star7.png", UriKind.RelativeOrAbsolute), size : 20),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star8.png", UriKind.RelativeOrAbsolute), size : 20),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star9.png", UriKind.RelativeOrAbsolute), size : 20),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star10.png", UriKind.RelativeOrAbsolute), size : 20),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star1.png", UriKind.RelativeOrAbsolute), size : 30),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star2.png", UriKind.RelativeOrAbsolute), size : 30),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star3.png", UriKind.RelativeOrAbsolute), size : 30),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/nebula1.png", UriKind.RelativeOrAbsolute), size : 130),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/nebula2.png", UriKind.RelativeOrAbsolute), size : 130),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy1.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy2.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy3.png", UriKind.RelativeOrAbsolute), size: 200),
            new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid1.png", UriKind.RelativeOrAbsolute), size: 250),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid2.png", UriKind.RelativeOrAbsolute), size: 250),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid3.png", UriKind.RelativeOrAbsolute), size: 250),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid4.png", UriKind.RelativeOrAbsolute), size : 250),
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

        public static (Uri AssetUri, ShipClass ShipClass)[] PLAYER_SHIP_THRUST_TEMPLATES = new (Uri AssetUri, ShipClass ShipClass)[]
        {
             (new Uri("ms-appx:///Assets/Images/space_thrust1.png", UriKind.RelativeOrAbsolute), ShipClass.DEFENDER),
             (new Uri("ms-appx:///Assets/Images/space_thrust2.png", UriKind.RelativeOrAbsolute), ShipClass.BERSERKER),
             (new Uri("ms-appx:///Assets/Images/space_thrust3.png", UriKind.RelativeOrAbsolute), ShipClass.SPECTRE),
        };

        public static Uri[] COLLECTIBLE_TEMPLATES = new Uri[]
        {
            new Uri("ms-appx:///Assets/Images/pizza1.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/pizza2.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/pizza3.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/pizza4.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/pizza5.png", UriKind.RelativeOrAbsolute),
            new Uri("ms-appx:///Assets/Images/pizza6.png", UriKind.RelativeOrAbsolute),
        };

        public static LocalizationTemplate[] LOCALIZATION_TEMPLATES = new LocalizationTemplate[]
        {
            new LocalizationTemplate(key: "ApplicationName_Header", cultureValues: new (string Culture, string Value)[]
            { 
                new ("en", "Astro Odyssey"),
                new ("bn", "অ্যাস্ট্রো ওডিসি"),
                new ("de", "Astro-Odyssee"),
                new ("fr", "Astro Odyssée"),
            }),            

            new LocalizationTemplate(key: "Antimony", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Antimony"),
                new ("bn", "অ্যান্টিমনি"),
                new ("de", "Antimon"),
                new ("fr", "Antimoine"),
            }),
            new LocalizationTemplate(key: "Bismuth", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Bismuth"),
                new ("bn", "বিসমাথ"),
                new ("de", "Wismut"),
                new ("fr", "Bismuth"),
            }),
            new LocalizationTemplate(key: "Curium", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Curium"),
                new ("bn", "কিউরিয়াম"),
                new ("de", "Gericht"),
                new ("fr", "Rechercher"),
            }),

            new LocalizationTemplate(key: "BEAM_CANNON", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "BEAM CANNON"),
                new ("bn", "রশ্মি কামান"),
                new ("de", "STRAHLKANONE"),
                new ("fr", "CANON À FAISCEAU"),
            }),
            new LocalizationTemplate(key: "BLAZE_BLITZ", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "BLAZE BLITZ"),
                new ("bn", "ব্লেজ ব্লিটজ"),
                new ("de", "BLAZE BLITZ"),
                new ("fr", "BLAZE BLITZ"),
            }),
            new LocalizationTemplate(key: "PLASMA_BOMB", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "PLASMA BOMB"),
                new ("bn", "প্লাজমা বোমা"),
                new ("de", "Plasmabombe"),
                new ("fr", "BOMBE À PLASMA"),
            }),

            new LocalizationTemplate(key: "BOSS", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "BOSS"),
                new ("bn", "বস"),
                new ("de", "BOSS"),
                new ("fr", "CHEF"),
            }),
            new LocalizationTemplate(key: "CLOAK_DOWN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "CLOAK DOWN"),
                new ("bn", "ক্লোক ডাউন"),
                new ("de", "KLAPPEN SIE SICH"),
                new ("fr", "CLOCHEZ-VOUS"),
            }),
            new LocalizationTemplate(key: "CLOAK_UP", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "CLOAK UP"),
                new ("bn", "ক্লোক আপ"),
                new ("de", "KLEIDE DICH"),
                new ("fr", "COUVREZ-VOUS"),
            }),
            new LocalizationTemplate(key: "FIREPOWER_DOWN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "FIREPOWER DOWN"),
                new ("bn", "ফায়ার পাওয়ার ডাউন"),
                new ("de", "FEUERKRAFT AUS"),
                new ("fr", "PUISSANCE DE FEU ARRÊTÉE"),
            }),
            new LocalizationTemplate(key: "FIREPOWER_UP", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "FIREPOWER UP"),
                new ("bn", "ফায়ার পাওয়ার আপ"),
                new ("de", "FEUERKRAFT HOCH"),
                new ("fr", "PUISSANCE DE FEU"),
            }),
            new LocalizationTemplate(key: "SHIELD_DOWN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SHIELD DOWN"),
                new ("bn", "শিল্ড ডাউন"),
                new ("de", "SCHILD NACH UNTEN"),
                new ("fr", "BOUCLIER BAS"),
            }),
            new LocalizationTemplate(key: "SHIELD_UP", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SHIELD UP"),
                new ("bn", "শিল্ড আপ"),
                new ("de", "SCHILD AUF"),
                new ("fr", "BOUCLIER"),
            }),

            new LocalizationTemplate(key: "COMPLETE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "COMPLETE"),
                new ("bn", "সম্পূর্ণ"),
                new ("de", "KOMPLETT"),
                new ("fr", "ACHEVÉ"),
            }),
            new LocalizationTemplate(key: "ENEMY_APPROACHES", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "ENEMY APPROACHES"),
                new ("bn", "শত্রু কাছে পৌঁছেছে"),
                new ("de", "Feindliche Annäherungen"),
                new ("fr", "APPROCHES ENNEMIES"),
            }),
            new LocalizationTemplate(key: "FANTASTIC_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Fantastic game"),
                new ("bn", "চমত্কার খেলা"),
                new ("de", "Fantastisches Spiel"),
                new ("fr", "Jeu fantastique"),
            }),
            new LocalizationTemplate(key: "GAME_PAUSED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "GAME PAUSED"),
                new ("bn", "খেলা থামানো হয়েছে"),
                new ("de", "DAS SPIEL PAUSIERT"),
                new ("fr", "JEU EN PAUSE"),
            }),
            new LocalizationTemplate(key: "GOOD_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Good game"),
                new ("bn", "ভাল খেলা"),
                new ("de", "Gute Partie"),
                new ("fr", "Bon jeu"),
            }),
            new LocalizationTemplate(key: "GREAT_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Great game"),
                new ("bn", "অসাধারন খেলা"),
                new ("de", "Tolles Spiel"),
                new ("fr", "bon jeu"),
            }),
            new LocalizationTemplate(key: "LEVEL", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "LEVEL"),
                new ("bn", "স্তর"),
                new ("de", "EBEN"),
                new ("fr", "NIVEAU"),
            }),
            new LocalizationTemplate(key: "NO_LUCK", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "No luck"),
                new ("bn", "ভাগ্য নেই"),
                new ("de", "Kein Glück"),
                new ("fr", "Pas de chance"),
            }),
            new LocalizationTemplate(key: "POWER_DOWN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "POWER DOWN"),
                new ("bn", "ক্ষমতা হ্রাস"),
                new ("de", "STROMAUSFALL"),
                new ("fr", "ÉTEINDRE"),
            }),
            new LocalizationTemplate(key: "QUIT_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "QUIT GAME?"),
                new ("bn", "খেলা বন্ধ?"),
                new ("de", "SPIEL VERLASSEN?"),
                new ("fr", "QUITTER LE JEU?"),
            }),
            new LocalizationTemplate(key: "SCORE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Score"),
                new ("bn", "স্কোর"),
                new ("de", "Punktzahl"),
                new ("fr", "Score"),
            }),
            new LocalizationTemplate(key: "DESTROYED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Destroyed"),
                new ("bn", "ধ্বংস"),
                new ("de", "Zerstört"),
                new ("fr", "Détruits"),
            }),
            new LocalizationTemplate(key: "ENEMIES_DESTROYED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Enemies"),
                new ("bn", "শত্রু"),
                new ("de", "Feinde"),
                new ("fr", "Ennemis"),
            }),
            new LocalizationTemplate(key: "METEORS_DESTROYED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Meteors"),
                new ("bn", "উল্কা"),
                new ("de", "Meteoren"),
                new ("fr", "Météores"),
            }),
            new LocalizationTemplate(key: "BOSSES_DESTROYED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Bosses"),
                new ("bn", "বস"),
                new ("de", "Bosse"),
                new ("fr", "Boss"),
            }),
            new LocalizationTemplate(key: "COLLECTIBLES_COLLECTED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collectibles"),
                new ("bn", "সংগ্রহযোগ্য"),
                new ("de", "Sammlerstücke"),
                new ("fr", "Objets de collection"),
            }),

            new LocalizationTemplate(key: "SHIP_REPAIRED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SHIP REPAIRED"),
                new ("bn", "জাহাজ মেরামত"),
                new ("de", "SCHIFF REPARIERT"),
                new ("fr", "NAVIRE RÉPARÉ"),
            }),
            //new LocalizationTemplate(key: "COLLECTIBLE_COLLECTED", cultureValues: new (string Culture, string Value)[]
            //{
            //    new ("en", "COMIC BOOK COLLECTED"),
            //    new ("bn", "কমিক বই সংগৃহীত"),
            //    new ("de", "COMIC-BUCH GESAMMELT"),
            //    new ("fr", "BD COLLECTIONNÉE"),
            //}),
            new LocalizationTemplate(key: "SONIC_BLAST", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SONIC BLAST"),
                new ("bn", "সোনিক বিস্ফোরণ"),
                new ("de", "SONIC BLAST"),
                new ("fr", "SONIC SONIC"),
            }),
            new LocalizationTemplate(key: "SUPREME_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Supreme game"),
                new ("bn", "সর্বোচ্চ খেলা"),
                new ("de", "Höchstes Spiel"),
                new ("fr", "Jeu suprême"),
            }),
            new LocalizationTemplate(key: "TAP_ON_SCREEN_TO_BEGIN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "TAP ON THE SCREEN TO BEGIN"),
                new ("bn", "শুরু করতে স্ক্রিনে ট্যাপ করুন"),
                new ("de", "TIPPEN SIE AUF DEN BILDSCHIRM, UM ZU BEGINNEN"),
                new ("fr", "APPUYEZ SUR L'ÉCRAN POUR COMMENCER"),
            }),
            new LocalizationTemplate(key: "TAP_TO_QUIT", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "TAP TO QUIT"),
                new ("bn", "প্রস্থান করতে ট্যাপ করুন"),
                new ("de", "TIPPEN SIE ZUM BEENDEN"),
                new ("fr", "APPUYER POUR QUITTER"),
            }),
            new LocalizationTemplate(key: "TAP_TO_RESUME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "TAP TO RESUME"),
                new ("bn", "আবার শুরু করতে ট্যাপ করুন"),
                new ("de", "TIPPEN, UM WEITERZUFAHREN"),
                new ("fr", "APPUYER POUR REPRENDRE"),
            }),

            new LocalizationTemplate(key: "GameOverPage_PlayAgainButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "PLAY AGAIN"),
                new ("bn", "আবার খেলুন"),
                new ("de", "NOCHMAL ABSPIELEN"),
                new ("fr", "REJOUER"),
            }),
            new LocalizationTemplate(key: "GameOverPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "GAME OVER"),
                new ("bn", "খেলা শেষ"),
                new ("de", "SPIEL IST AUS"),
                new ("fr", "JEU TERMINÉ"),
            }),
            new LocalizationTemplate(key: "GameStartPage_AssetsCreditButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "😘 Assets by redfoc.com"),
                new ("bn", "😘 Assets by redfoc.com"),
                new ("de", "😘 Assets von redfoc.com"),
                new ("fr", "😘 Actifs par redfoc.com"),
            }),

            new LocalizationTemplate(key: "GameStartPage_BanglaButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Bangla"),
                new ("bn", "বাংলা"),
                new ("de", "Bangla"),
                new ("fr", "Bangla"),
            }),
            new LocalizationTemplate(key: "GameStartPage_DeutschButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Deutsch"),
                new ("bn", "ডয়েচ"),
                new ("de", "Deutsch"),
                new ("fr", "Deutsch"),
            }),
            new LocalizationTemplate(key: "GameStartPage_EnglishButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "English"),
                new ("bn", "ইংরেজি"),
                new ("de", "Englisch"),
                new ("fr", "Anglais"),
            }),
            new LocalizationTemplate(key: "GameStartPage_FrenchButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "French"),
                new ("bn", "ফরাসি"),
                new ("de", "Französisch"),
                new ("fr", "Français"),
            }),

            new LocalizationTemplate(key: "GameStartPage_DeveloperProfileButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Made with ❤️ by Asadullah Rifat"),
                new ("bn", "আসাদুল্লাহ রিফাত দ্বারা ❤️ দিয়ে তৈরি"),
                new ("de", "Hergestellt mit ❤️ von Asadullah Rifat"),
                new ("fr", "Fait avec ❤️ par Asadullah Rifat"),
            }),

            new LocalizationTemplate(key: "GameStartPage_PlayButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "START GAME"),
                new ("bn", "খেলা শুরু করুন"),
                new ("de", "SPIEL BEGINNEN"),
                new ("fr", "DÉMARRER JEU"),
            }),
            new LocalizationTemplate(key: "GameStartPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "A classic rock metal ⚡ space shooter on WebAssembly."),
                new ("bn", "WebAssembly-এ একটি ক্লাসিক রক মেটাল ⚡ স্পেস শ্যুটার।"),
                new ("de", "Ein klassischer Rock Metal ⚡ Weltraum-Shooter auf WebAssembly."),
                new ("fr", "Un jeu de tir spatial classique rock métal ⚡ sur WebAssembly."),
            }),
            new LocalizationTemplate(key: "ShipSelectionPage_ChooseButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SELECT"),
                new ("bn", "নির্বাচন করুন"),
                new ("de", "AUSWÄHLEN"),
                new ("fr", "SÉLECTIONNER"),
            }),
            new LocalizationTemplate(key: "ShipSelectionPage_ControlInstructions", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Press or touch left or right arrow keys or edges on your keyboard or screen to move the ship."),
                new ("bn", "জাহাজটি নিয়ন্ত্রন করতে আপনার কীবোর্ড বা স্ক্রিনের বাম বা ডান তীর কী বা প্রান্তগুলি টিপুন বা স্পর্শ করুন৷"),
                new ("de", "Drücke oder berühre die linke oder rechte Pfeiltaste oder die Kanten auf deiner Tastatur oder deinem Bildschirm, um das Schiff zu bewegen."),
                new ("fr", "Appuyez ou touchez les touches fléchées gauche ou droite ou les bords de votre clavier ou de votre écran pour déplacer le navire."),
            }),
            new LocalizationTemplate(key: "ShipSelectionPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Select a Ship"),
                new ("bn", "একটি জাহাজ নির্বাচন করুন"),
                new ("de", "Wählen Sie ein Schiff aus"),
                new ("fr", "Sélectionnez un navire"),
            }),
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
