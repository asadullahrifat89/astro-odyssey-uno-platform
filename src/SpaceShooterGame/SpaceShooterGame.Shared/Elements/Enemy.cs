using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace SpaceShooterGame
{
    public class Enemy : GameObject
    {
        #region Fields

        private readonly Image _content = new() { Stretch = Stretch.Uniform };

        private readonly Random random = new();

        #endregion

        #region Properties

        public double ProjectileSpawnCounter { get; set; } = 0;

        public double ProjectileSpawnAfter { get; set; } = 60;

        public double OverPoweredProjectileSpawnCounter { get; set; }

        public double OverPoweredProjectileSpawnAfter { get; set; } = 5;

        public bool IsPlayerColliding { get; set; }

        public bool IsProjectileFiring { get; set; }

        public bool IsHovering { get; set; }

        public bool IsEvading { get; set; }

        public bool IsEvadingTriggered { get; set; }

        public EnemyClass EnemyClass { get; set; }

        public bool IsBoss { get; set; }

        public BossClass BossClass { get; set; }

        #endregion

        #region Ctor

        public Enemy()
        {
            Tag = ElementType.ENEMY;

            IsDestructible = true;

            Child = _content;
            YDirection = YDirection.DOWN;

            Background = new SolidColorBrush(Colors.Transparent);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        }

        #endregion

        #region Methods

        public void SetAttributes(
            double speed,
            int gameLevel,
            double scale = 1,
            bool isBoss = false,
            bool recycle = false)
        {
            Opacity = 1;
            Speed = speed;

            XDirection = XDirection.NONE;
            YDirection = YDirection.DOWN;

            IsDestroyedByCollision = false;
            IsEvadingTriggered = false;

            ProjectileSpawnCounter = 0;
            ProjectileSpawnAfter = 60;
            OverPoweredProjectileSpawnAfter = 5;

            SetRotation(0);

            if (isBoss)
            {
                IsBoss = true;
                IsOverPowered = true;

                var bosses = AssetHelper.BOSS_TEMPLATES;
                var bossType = random.Next(0, bosses.Length);
                var bossTemplate = bosses[bossType]; // TODO: SET TO BOSS TYPE

                BossClass = (BossClass)bossType;
                Speed--;
                ProjectileSpawnAfter -= 5 * scale * ((double)gameLevel / 2);

                Health = Constants.BOSS_BASE_HEALTH * (double)gameLevel;

                double height = 0;
                double width = 0;

                width = 512;
                height = 512;

                Height = (height / 2.5) * scale;
                Width = (width / 2.5) * scale;

                HalfWidth = Width / 2;

                Uri uri = bossTemplate.AssetUri;
                _content.Source = new BitmapImage(uri);
            }
            else
            {
                Health = random.Next(1, 4);
                ProjectileSpawnAfter -= 5 * scale * ((double)gameLevel / 2);

                if (!recycle)
                {
                    var enemies = AssetHelper.ENEMY_TEMPLATES;
                    var enemyType = random.Next(0, enemies.Length);
                    var enemyTemplate = enemies[enemyType];
                    var scaledSize = scale * enemyTemplate.Size;

                    EnemyClass = enemyType < 3 ? EnemyClass.RED : EnemyClass.GREEN;

                    Height = scaledSize;
                    Width = scaledSize;

                    HalfWidth = Width / 2;

                    Uri uri = enemyTemplate.AssetUri;
                    _content.Source = new BitmapImage(uri);
                }
            }
        }

        public void Evade()
        {
            if (XDirection == XDirection.NONE)
            {
                XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                Speed /= 1.85; // decrease speed

                IsEvadingTriggered = true;
            }
        }

        public void SetProjectileHitEffect()
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
                BorderThickness = new Microsoft.UI.Xaml.Thickness(left: BorderThickness.Left - 1, top: 0, right: 0, bottom: 0);

            if (BorderThickness.Right != 0)
                BorderThickness = new Microsoft.UI.Xaml.Thickness(left: 0, top: 0, right: BorderThickness.Right - 1, bottom: 0);
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
        CRIMSON,
        BLUE,
    }
}

