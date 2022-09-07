using System;
using System.Collections.Generic;
using System.Text;

namespace AstroOdyssey
{
    public static class GameObjectTemplates
    {
        public static (Uri AssetUri, ShipClass ShipClass)[] PLAYER_RAGE_TEMPLATES = new (Uri AssetUri, ShipClass ShipClass)[]
        {
             (new Uri("ms-appx:///Assets/Images/rage1.png", UriKind.RelativeOrAbsolute), ShipClass.DEFENDER),
             (new Uri("ms-appx:///Assets/Images/rage2.png", UriKind.RelativeOrAbsolute), ShipClass.BERSERKER),
             (new Uri("ms-appx:///Assets/Images/rage3.png", UriKind.RelativeOrAbsolute), ShipClass.SPECTRE),
        };

        public static CelestialObjectTemplate[] STAR_TEMPLATES = new CelestialObjectTemplate[]
        {
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star1.png", UriKind.RelativeOrAbsolute), size : 25),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/star2.png", UriKind.RelativeOrAbsolute), size : 25),           
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star1.png", UriKind.RelativeOrAbsolute), size : 30),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star2.png", UriKind.RelativeOrAbsolute), size : 30),
            //new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/gas_star3.png", UriKind.RelativeOrAbsolute), size : 30),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/nebula1.png", UriKind.RelativeOrAbsolute), size : 130),
            new CelestialObjectTemplate(assetUri : new Uri("ms-appx:///Assets/Images/nebula2.png", UriKind.RelativeOrAbsolute), size : 130),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy1.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy2.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/galaxy3.png", UriKind.RelativeOrAbsolute), size: 200),
            //new CelestialObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/asteroid1.png", UriKind.RelativeOrAbsolute), size: 250),
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
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock2.png", UriKind.RelativeOrAbsolute), health: 3),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock3.png", UriKind.RelativeOrAbsolute), health: 2),
            new DestructibleObjectTemplate(assetUri: new Uri("ms-appx:///Assets/Images/rock4.png", UriKind.RelativeOrAbsolute), health: 2),
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
    }
}
