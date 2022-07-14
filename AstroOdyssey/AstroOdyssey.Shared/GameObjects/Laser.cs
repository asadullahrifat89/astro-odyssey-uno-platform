using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class Laser : GameObject
    {
        #region Ctor

        public Laser()
        {
            Tag = Constants.LASER;

            Height = 20;
            Width = 5;

            CornerRadius = new CornerRadius(50);
            YDirection = YDirection.UP;
        }

        #endregion

        #region Properties

        public bool IsPoweredUp { get; set; }

        #endregion

        #region Methods

        public void SetAttributes(double speed, GameLevel gameLevel, bool isPoweredUp = false)
        {
            Speed = speed;
           
            IsPoweredUp = isPoweredUp;

            double height = 0, width = 0;

            switch (gameLevel)
            {
                case GameLevel.Level_1:
                    {
                        height = 20; width = 5;
                    }
                    break;
                case GameLevel.Level_2:
                    {
                        height = 25; width = 7;
                    }
                    break;
                case GameLevel.Level_3:
                    {
                        height = 30; width = 9;
                    }
                    break;
                case GameLevel.Level_4:
                    {
                        height = 35; width = 11;
                    }
                    break;
                case GameLevel.Level_5:
                    {
                        height = 40; width = 13;
                    }
                    break;
                case GameLevel.Level_6:
                    {
                        height = 45; width = 15;
                    }
                    break;
                case GameLevel.Level_7:
                    {
                        height = 50; width = 17;
                    }
                    break;
                case GameLevel.Level_8:
                    {
                        height = 55; width = 19;
                    }
                    break;
                default:
                    break;
            }

            Height = height;
            Width = width;

            if (IsPoweredUp)
                Background = new SolidColorBrush(Colors.Goldenrod);
            else
                Background = new SolidColorBrush(Colors.White);
        }

        #endregion
    }
}
