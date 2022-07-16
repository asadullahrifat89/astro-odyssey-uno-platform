using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;

namespace AstroOdyssey
{
    public class GameObject : Border
    {
        private readonly CompositeTransform compositeTransform = new CompositeTransform()
        {
            CenterX = 0.5,
            CenterY = 0.5,
            Rotation = 0,
            ScaleX = 1,
            ScaleY = 1,
        };

        #region Ctor

        public GameObject()
        {
#if DEBUG
            //BorderBrush = new SolidColorBrush(Colors.White);
            //BorderThickness = new Microsoft.UI.Xaml.Thickness(1);
#endif
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = compositeTransform;
        }

        #endregion

        #region Properties

        public int Health { get; set; }

        public int HitPoint { get; set; } = 1;

        public bool IsDestructible { get; set; }

        public bool MarkedForFadedRemoval { get; set; }

        public double Speed { get; set; } = 1;

        public YDirection YDirection { get; set; } = YDirection.NONE;

        public XDirection XDirection { get; set; } = XDirection.NONE;

        public bool HasNoHealth => Health <= 0;

        public bool HasHealth => Health > 0;

        public bool HasFadedAway => Opacity <= 0;

        public new double Rotation { get; set; } = 0;

        public bool IsOverPowered { get; set; } = false;

        #endregion

        #region Methods

        public void SetSize(double size)
        {
            Height = size;
            Width = size;
        }

        public void Rotate()
        {
            compositeTransform.Rotation += Rotation;
        }

        public void GainHealth()
        {
            Health += HitPoint;
        }

        public void GainHealth(int health)
        {
            Health += health;
        }

        public void LooseHealth()
        {
            Health -= HitPoint;
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

        public void MoveX(XDirection xDirection)
        {
            Canvas.SetLeft(this, GetX() + (Speed * GetXDirectionModifier(xDirection)));
        }

        public void MoveY()
        {
            Canvas.SetTop(this, GetY() + (Speed * GetYDirectionModifier()));
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

        public void OverPower()
        {
            Height = Height * 1.5;
            Width = Width * 1.5;
            Speed--;
            Health += 3;

            IsOverPowered = true;
        }
        #endregion
    }

    public enum YDirection
    {
        NONE,
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
