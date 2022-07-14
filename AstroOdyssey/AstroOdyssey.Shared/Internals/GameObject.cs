using Windows.Foundation;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace AstroOdyssey
{
    public class GameObject : Border
    {
        #region Ctor

        public GameObject()
        {
#if DEBUG
            BorderBrush = new SolidColorBrush(Colors.White);
            BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
#endif
        }

        #endregion

        #region Properties

        public int Health { get; set; }

        public int HealthSlot { get; set; } = 1;

        public bool IsDestructible { get; set; }

        public bool MarkedForFadedRemoval { get; set; }

        public double Speed { get; set; } = 1;

        public YDirection YDirection { get; set; } = YDirection.DOWN;

        public XDirection XDirection { get; set; } = XDirection.NONE;

        public bool HasNoHealth => Health <= 0;

        public bool HasFadedAway => Opacity <= 0;

        #endregion

        #region Methods

        public void GainHealth()
        {
            Health += HealthSlot;
        }

        public void GainHealth(int health)
        {
            Health += health;
        }

        public void LooseHealth()
        {
            Health -= HealthSlot;
        }

        public void LooseHealth(int health)
        {
            Health -= health;
        }

        public Rect GetRect()
        {
            return new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), Width, Height);
        }

        public double GetY()
        {
            return Canvas.GetTop(this);
        }

        public double GetX()
        {
            return Canvas.GetLeft(this);
        }

        public void SetY(double top)
        {
            Canvas.SetTop(this, top);
        }

        public void SetX(double left)
        {
            Canvas.SetLeft(this, left);
        }

        public void MoveX()
        {
            Canvas.SetLeft(this, GetX() + (Speed * GetXDirectionModifier()));
        }

        public void MoveX(double left)
        {
            Canvas.SetLeft(this, GetX() + (left * GetXDirectionModifier()));
        }

        public void MoveX(double left, XDirection xDirection)
        {
            Canvas.SetLeft(this, GetX() + (left * GetXDirectionModifier(xDirection)));
        }

        public void MoveY()
        {
            Canvas.SetTop(this, GetY() + (this.Speed * GetYDirectionModifier()));
        }

        public void MoveY(double top)
        {
            Canvas.SetTop(this, GetY() + (top * GetYDirectionModifier()));
        }

        public void MoveY(double top, YDirection yDirection)
        {
            Canvas.SetTop(this, GetY() + (top * GetYDirectionModifier(yDirection)));
        }

        public void SetPosition(double top, double left)
        {
            Canvas.SetTop(this, top);
            Canvas.SetLeft(this, left);
        }

        private int GetXDirectionModifier(XDirection? xDirection = null)
        {
            var modifier = 0;
            var xDirectionConsider = xDirection ?? XDirection;

            switch (xDirectionConsider)
            {
                case XDirection.NONE:
                    modifier = 0;
                    break;
                case XDirection.LEFT:
                    modifier = -1;
                    break;
                case XDirection.RIGHT:
                    modifier = 1;
                    break;
                default:
                    break;
            }

            return modifier;
        }

        private int GetYDirectionModifier(YDirection? yDirection = null)
        {
            var modifier = 0;
            var yDirectionConsider = yDirection ?? YDirection;

            switch (yDirectionConsider)
            {
                case YDirection.UP:
                    modifier = -1;
                    break;
                case YDirection.DOWN:
                    modifier = 1;
                    break;
                default:
                    break;
            }

            return modifier;
        }

        public void AddToGameEnvironment(double top, double left, GameEnvironment gameEnvironment)
        {
            SetPosition(top, left);
            gameEnvironment.AddGameObject(this);
        }

        public void Fade()
        {
            Opacity -= 0.1d;
        }

        #endregion
    }

    public enum YDirection
    {
        UP,
        DOWN,
    }

    public enum XDirection
    {
        NONE,
        LEFT,
        RIGHT,
    }
}
