﻿using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class Enemy : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };

        private readonly Random random = new Random();

        public int ProjectileSpawnCounter;
        public int ProjectileSpawnDelay = 60;

        public int OverPoweredProjectileSpawnCounter;
        public int OverPoweredProjectileSpawnDelay = 5;

        #endregion

        #region Ctor

        public Enemy()
        {
            Tag = Constants.ENEMY;
            Height = Constants.DESTRUCTIBLE_OBJECT_SIZE;
            Width = Constants.DESTRUCTIBLE_OBJECT_SIZE;

            IsDestructible = true;
            Child = content;
            YDirection = YDirection.DOWN;

            Background = new SolidColorBrush(Colors.Transparent);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        }

        #endregion

        #region Properties

        public bool IsBoss { get; set; }

        public bool IsPlayerTargeting { get; set; }

        public bool IsProjectileFiring { get; set; }

        public bool IsHovering { get; set; }

        public bool IsEvading { get; set; }

        public bool IsEvadingTriggered { get; set; }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;
            XDirection = XDirection.NONE;
            IsMarkedForFadedDestruction = false;
            Opacity = 1;

            Uri uri = null;
            var enemyShipType = random.Next(1, 6);

            switch (enemyShipType)
            {
                case 1:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_A.png", UriKind.RelativeOrAbsolute);
                    Health = 2;
                    break;
                case 2:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_B.png", UriKind.RelativeOrAbsolute);
                    Health = 2;
                    break;
                case 3:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_C.png", UriKind.RelativeOrAbsolute);
                    Health = 1;
                    break;
                case 4:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_D.png", UriKind.RelativeOrAbsolute);
                    Health = 3;
                    break;
                case 5:
                    uri = new Uri("ms-appx:///Assets/Images/enemy_E.png", UriKind.RelativeOrAbsolute);
                    Health = 3;
                    break;
            }

            content.Source = new BitmapImage(uri);

            Height = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;
            Width = Constants.DESTRUCTIBLE_OBJECT_SIZE * scale;

            HalfWidth = Width / 2;
        }

        public void Evade()
        {
            if (XDirection == XDirection.NONE)
            {
                XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                Speed = Speed / 2; // decrease speed

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
}

