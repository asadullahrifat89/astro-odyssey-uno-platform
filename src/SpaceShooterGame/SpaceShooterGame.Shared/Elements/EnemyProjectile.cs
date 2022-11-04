using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace SpaceShooterGame
{
    public class EnemyProjectile : GameObject
    {
        #region Ctor

        public EnemyProjectile()
        {
            Tag = ElementType.ENEMY_PROJECTILE;

            CornerRadius = new CornerRadius(25);
            YDirection = YDirection.DOWN;

            IsProjectile = true;
        }

        #endregion

        #region Methods

        public void SetAttributes(
            Enemy enemy,
            double scale = 1,
            bool isOverPowered = false)
        {
            Speed = enemy.IsBoss
                ? enemy.Speed * 1.50
                : enemy.Speed * 1.65;

            double height = 20;
            double width = 20;

            if (enemy.IsBoss)
            {
                switch (enemy.BossClass)
                {
                    case BossClass.GREEN:
                        {
                            Background = new SolidColorBrush(Colors.LightGreen);
                            BorderBrush = new SolidColorBrush(Colors.Green);
                            BorderThickness = new Thickness(3 * scale);

                            width += 5;
                        }
                        break;
                    case BossClass.CRIMSON:
                        {
                            Background = new SolidColorBrush(Colors.Orange);
                            BorderBrush = new SolidColorBrush(Colors.OrangeRed);
                            BorderThickness = new Thickness(3 * scale);

                            height += 5;
                        }
                        break;
                    case BossClass.BLUE:
                        {
                            Background = new SolidColorBrush(Colors.SkyBlue);
                            BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
                            BorderThickness = new Thickness(3 * scale);
                        }
                        break;
                    default:
                        break;
                }

                if (isOverPowered)
                    CornerRadius = new CornerRadius(15 * scale);
                else
                    CornerRadius = new CornerRadius(25 * scale);
            }
            else
            {
                if (isOverPowered)
                {
                    Background = new SolidColorBrush(Colors.Violet);
                    BorderBrush = new SolidColorBrush(Colors.DarkViolet);
                    BorderThickness = new Thickness(3 * scale);
                    CornerRadius = new CornerRadius(15 * scale);
                }
                else
                {
                    switch (enemy.EnemyClass)
                    {
                        case EnemyClass.RED:
                            {
                                Background = new SolidColorBrush(Colors.Orange);
                                BorderBrush = new SolidColorBrush(Colors.OrangeRed);
                                BorderThickness = new Thickness(2 * scale);
                            }
                            break;
                        case EnemyClass.GREEN:
                            {
                                Background = new SolidColorBrush(Colors.LightGreen);
                                BorderBrush = new SolidColorBrush(Colors.Green);
                                BorderThickness = new Thickness(2 * scale);
                            }
                            break;
                        default:
                            break;
                    }

                    CornerRadius = new CornerRadius(25 * scale);
                }
            }

            Height = height * scale * (isOverPowered ? 1.5 : 1);
            Width = width * scale * (isOverPowered ? 1.5 : 1);

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
