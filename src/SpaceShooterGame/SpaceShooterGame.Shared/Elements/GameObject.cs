using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Point = Windows.Foundation.Point;

namespace SpaceShooterGame
{
    public class GameObject : Border
    {
        #region Fields

        private readonly CompositeTransform _compositeTransform = new()
        {
            CenterX = 0.5,
            CenterY = 0.5,
            Rotation = 0,
            ScaleX = 1,
            ScaleY = 1,
        };

        #endregion

        #region Properties

        public double Health { get; set; } = 1;

        public double HitPoint { get; set; } = 1;

        public double Speed { get; set; } = 1;

        public new double Rotation { get; set; } = 0;

        public YDirection YDirection { get; set; } = YDirection.NONE;

        public XDirection XDirection { get; set; } = XDirection.NONE;

        public bool HasNoHealth => Health <= 0;

        public bool HasHealth => Health > 0;

        public double ExplosionCounter { get; set; } = 1;

        public bool HasExploded => ExplosionCounter <= 0;

        public bool IsOverPowered { get; set; } = false;

        public bool IsDestructible { get; set; }

        public bool IsProjectile { get; set; }

        public bool IsPickup { get; set; }

        public bool IsCollectible { get; set; }

        public bool IsPlayer { get; set; }

        public double HalfWidth { get; set; }

        private bool _IsDestroyedByCollision;

        public bool IsDestroyedByCollision
        {
            get { return _IsDestroyedByCollision; }
            set
            {
                _IsDestroyedByCollision = value;

                if (_IsDestroyedByCollision)
                {
                    var tag = (ElementType)Tag;

                    switch (tag)
                    {
                        case ElementType.ENEMY:
                            {
                                Background = new SolidColorBrush(Colors.Goldenrod);
                                BorderBrush = new SolidColorBrush(Colors.DarkGoldenrod);

                                Height = 50 * 1.5;
                                Width = 50 * 1.5;

                                BorderThickness = new Thickness(5);
                                CornerRadius = new CornerRadius(100);

                                Child.Opacity = 0;
                            }
                            break;
                        case ElementType.METEOR:
                            {
                                Background = new SolidColorBrush(Colors.Crimson);
                                BorderBrush = new SolidColorBrush(Colors.DarkRed);

                                Height = 50 * 1.5;
                                Width = 50 * 1.5;

                                BorderThickness = new Thickness(5);
                                CornerRadius = new CornerRadius(100);

                                Child.Opacity = 0;
                            }
                            break;
                        case ElementType.PLAYER_PROJECTILE:
                            {
                                Height = 15 * 1.5;
                                Width = 15 * 1.5;

                                YDirection = YDirection.NONE;
                                CornerRadius = new CornerRadius(50);
                            }
                            break;
                        case ElementType.ENEMY_PROJECTILE:
                            {
                                Height = 15 * 1.5;
                                Width = 15 * 1.5;

                                YDirection = YDirection.NONE;
                                CornerRadius = new CornerRadius(50);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        #region Ctor

        public GameObject()
        {
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = _compositeTransform;

            CanDrag = false;
        }

        #endregion

        #region Methods

        public void SetSize(double size)
        {
            Height = size;
            Width = size;
        }

        public void SetRotation(double rotation)
        {
            Rotation = rotation;
            _compositeTransform.Rotation = Rotation;
        }

        public void Rotate()
        {
            _compositeTransform.Rotation += Rotation;
        }

        public void GainHealth()
        {
            Health += HitPoint;
        }

        public void GainHealth(double health)
        {
            Health += health;
        }

        public void LooseHealth()
        {
            Health -= HitPoint;
        }

        public void LooseHealth(double health)
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

        public void Explode()
        {
            ExplosionCounter -= 0.1d;

            if (ExplosionCounter <= 0.5d)
                Opacity -= 0.1d;

            ScaleUp();
        }

        public void ScaleUp()
        {
            _compositeTransform.ScaleX += 0.3d;
            _compositeTransform.ScaleY += 0.3d;
        }

        public void Widen()
        {
            Width += 1.2d;
            Height += 0.08d;
        }

        public void Lengthen()
        {
            Height += 0.6d;
        }

        public void OverPower()
        {
            Height = Height * 1.5;
            Width = Width * 1.5;
            HalfWidth = Width / 2;
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

    public enum ElementType
    {
        NONE,
        PLAYER,
        ENEMY,
        METEOR,
        HEALTH,
        POWERUP,
        COLLECTIBLE,
        PLAYER_PROJECTILE,
        ENEMY_PROJECTILE,
        CELESTIAL_OBJECT,
    }

    public enum ImageType
    {
        STAR,
        PLANET,
        COLLECTIBLE,
        BOSS_APPEARED,
        BOSS_CLEARED,
        GAME_OVER,
        HEALTH,
        POWERUP,
        GAME_INTRO,
        SCORE_MULTIPLIER,
        BOSS,
        ENEMY,
        METEOR,
        PLAYER_SHIP,
        PLAYER_SHIP_THRUST,
        PLAYER_RAGE
    }
}
