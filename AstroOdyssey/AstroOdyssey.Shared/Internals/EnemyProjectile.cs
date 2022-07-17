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
        }

        #endregion

        #region Methods

        public void SetAttributes(double speed, GameLevel gameLevel, double scale = 1)
        {
            Speed = speed;

            double height = 0, width = 0;

            switch (gameLevel)
            {
                case GameLevel.Level_1:
                    {
                        height = 10; width = 7;
                    }
                    break;
                case GameLevel.Level_2:
                    {
                        height = 11; width = 8;
                    }
                    break;
                case GameLevel.Level_3:
                    {
                        height = 12; width = 9;
                    }
                    break;
                case GameLevel.Level_4:
                    {
                        height = 13; width = 10;
                    }
                    break;
                case GameLevel.Level_5:
                    {
                        height = 14; width = 11;
                    }
                    break;
                case GameLevel.Level_6:
                    {
                        height = 15; width = 12;
                    }
                    break;
                case GameLevel.Level_7:
                    {
                        height = 16; width = 13;
                    }
                    break;
                case GameLevel.Level_8:
                    {
                        height = 17; width = 14;
                    }
                    break;
                default:
                    break;
            }

            Background = new SolidColorBrush(Colors.White);

            Height = height * scale;
            Width = width * scale;
        }

        #endregion
    }
}
