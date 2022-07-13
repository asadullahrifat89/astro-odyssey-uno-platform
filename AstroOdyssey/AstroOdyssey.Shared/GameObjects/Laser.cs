using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

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

        public void SetAttributes(double speed, double height = 20, double width = 5, bool isPoweredUp = false)
        {
            Speed = speed;
            Height = height;
            Width = width;
            IsPoweredUp = isPoweredUp;

            if (IsPoweredUp)
                Background = new SolidColorBrush(Colors.Goldenrod);
            else
                Background = new SolidColorBrush(Colors.White);
        } 

        #endregion
    }
}
