﻿using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace AstroOdyssey
{
    public class ComicBook : GameObject
    {
        #region Fields

        private readonly Image content = new Image() { Stretch = Stretch.Uniform };
        private readonly Random random = new Random();
        #endregion

        #region Ctor

        public ComicBook()
        {
            Tag = Constants.COMIC_BOOK;
            Height = Constants.COLLECTIBLE_OBJECT_SIZE;
            Width = Constants.COLLECTIBLE_OBJECT_SIZE;
            Child = content;
            YDirection = YDirection.DOWN;

            IsCollectible = true;
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, double scale = 1)
        {
            Speed = speed;

            var uri = Constants.COMIC_BOOK_TEMPLATES[random.Next(0, Constants.COMIC_BOOK_TEMPLATES.Length)];

            content.Source = new BitmapImage(uri);

            Height = Constants.COLLECTIBLE_OBJECT_SIZE * scale;
            Width = Constants.COLLECTIBLE_OBJECT_SIZE * scale;

            HalfWidth = Width / 2;
        }

        #endregion
    }
}