using System;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class Enemy : GameObject
    {
        #region Fields

        //private readonly Grid _body = new Grid();

        private readonly Image _ship = new Image() { Stretch = Stretch.Uniform };

        //private readonly Image _engineThrust = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public Enemy()
        {
            Tag = Constants.ENEMY;
            Height = Constants.DESTRUCTIBLE_OBJECT_SIZE;
            Width = Constants.DESTRUCTIBLE_OBJECT_SIZE;

            IsDestructible = true;

            //_body = new Grid();
            //_body.Children.Add(_engineThrust);
            //_body.Children.Add(_ship);

            Child = _ship;
            YDirection = YDirection.DOWN;

            Background = new SolidColorBrush(Colors.Transparent);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        }

        #endregion

        #region Properties

        public int ProjectileSpawnCounter { get; set; }

        public int ProjectileSpawnDelay { get; set; } = 60;

        public int OverPoweredProjectileSpawnCounter { get; set; }

        public int OverPoweredProjectileSpawnDelay { get; set; } = 5;

        public bool IsPlayerColliding { get; set; }

        public bool IsProjectileFiring { get; set; }

        public bool IsHovering { get; set; }

        public bool IsEvading { get; set; }

        public bool IsEvadingTriggered { get; set; }

        public EnemyClass EnemyClass { get; set; }

        public bool IsBoss { get; set; }

        public BossClass BossClass { get; set; }

        #endregion

        #region Methods

        public void SetAttributes(
            double speed,
            GameLevel gameLevel,
            double scale = 1,
            bool isBoss = false)
        {
            Speed = speed;
            XDirection = XDirection.NONE;
            IsMarkedForFadedDestruction = false;
            Opacity = 1;

            if (isBoss)
            {
                IsBoss = true;
                IsOverPowered = true;

                var bossType = random.Next(0, GameObjectTemplates.BOSS_TEMPLATES.Length);
                var bossTemplate = GameObjectTemplates.BOSS_TEMPLATES[bossType]; // TODO: SET TO BOSS TYPE

                BossClass = bossTemplate.BossClass;

                var height = 0;
                var width = 0;

                switch (BossClass)
                {
                    case BossClass.GREEN:
                        {
                            width = 306;
                            height = 418;
                        }
                        break;
                    case BossClass.PURPLE:
                        {
                            width = 510;
                            height = 458;
                        }
                        break;
                    case BossClass.YELLOW:
                        {
                            width = 553;
                            height = 492;
                        }
                        break;
                    default:
                        break;
                }

                Height = (height / 2.5) * scale;
                Width = (width / 2.5) * scale;

                Speed--;
                ProjectileSpawnDelay -= (3 * (int)gameLevel);

                Health = 50 * (int)gameLevel;

                Uri uri = bossTemplate.AssetUri;
                _ship.Source = new BitmapImage(uri);
            }
            else
            {
                var enemyType = random.Next(0, GameObjectTemplates.ENEMY_TEMPLATES.Length);
                var enemyTemplate = GameObjectTemplates.ENEMY_TEMPLATES[enemyType];

                EnemyClass = enemyType < 3 ? EnemyClass.RED : EnemyClass.GREEN;             

                Uri uri = enemyTemplate.AssetUri;
                Health = enemyTemplate.Health;

                var size = enemyTemplate.Size;
                Height = size * scale;
                Width = size * scale;

                _ship.Source = new BitmapImage(uri);
            }

            HalfWidth = Width / 2;
        }

        public void Evade()
        {
            if (XDirection == XDirection.NONE)
            {
                XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                Speed = Speed / 1.85; // decrease speed

                IsEvadingTriggered = true;
            }
        }

        //public void SetRecoilEffect()
        //{
        //    BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: 0, right: 0, bottom: 4);
        //}

        //public void CoolDownRecoilEffect()
        //{
        //    if (BorderThickness.Bottom != 0)
        //    {
        //        BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: 0, right: 0, bottom: BorderThickness.Bottom - 1);
        //    }
        //}

        public void SetProjectileImpactEffect()
        {
            var effect = random.Next(0, 2);

            switch (effect)
            {
                case 0: BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: 0, right: 5, bottom: 0); break;
                case 1: BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 5, top: 0, right: 0, bottom: 0); break;
                default:
                    break;
            }
        }

        public void CoolDownProjectileImpactEffect()
        {
            if (BorderThickness.Left != 0)
            {
                BorderThickness = new Microsoft.UI.Xaml.Thickness(left: BorderThickness.Left - 1, top: 0, right: 0, bottom: 0);
            }

            if (BorderThickness.Right != 0)
            {
                BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: 0, right: BorderThickness.Right - 1, bottom: 0);
            }
        }

        #endregion
    }

    public enum EnemyClass
    {
        RED,
        GREEN
    }

    public enum BossClass
    {
        GREEN,
        PURPLE,
        YELLOW,
    }
}

