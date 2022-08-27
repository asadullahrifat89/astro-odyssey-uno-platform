using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class EnemyProjectile : GameObject
    {
        #region Ctor

        public EnemyProjectile()
        {
            Tag = ENEMY_PROJECTILE;

            Height = 10;
            Width = 5;

            CornerRadius = new CornerRadius(50);
            YDirection = YDirection.DOWN;

            IsProjectile = true;
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, GameLevel gameLevel, double scale = 1, bool isOverPowered = false)
        {
            Speed = speed;

            double height = 12;
            double width = 12;

            if (isOverPowered)
            {
                Background = new SolidColorBrush(Colors.Violet);
                BorderBrush = new SolidColorBrush(Colors.DarkViolet);
                BorderThickness = new Thickness(3);
            }
            else
            {
                Background = new SolidColorBrush(Colors.Orange);
                BorderBrush = new SolidColorBrush(Colors.DarkOrange);
                BorderThickness = new Thickness(2);
            }           

            Height = height * scale * (isOverPowered ? 1.5 : 1);
            Width = width * scale * (isOverPowered ? 1.5 : 1);

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
