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
            Tag = ENEMY_PROJECTILE_TAG;

            Height = 10;
            Width = 5;

            CornerRadius = new CornerRadius(50);
            YDirection = YDirection.DOWN;

            IsProjectile = true;
        }

        #endregion

        #region Methods

        public void SetAttributes(Enemy enemy, double scale = 1, bool isOverPowered = false)
        {
            Speed = enemy.IsBoss
                ? enemy.Speed * 1.50
                : enemy.Speed * 1.65;

            double height = 12;
            double width = 12;

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
            }
            else
            {
                if (isOverPowered)
                {
                    Background = new SolidColorBrush(Colors.Violet);
                    BorderBrush = new SolidColorBrush(Colors.DarkViolet);
                    BorderThickness = new Thickness(3 * scale);
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
                }
            }

            Height = height * scale * (isOverPowered ? 1.5 : 1);
            Width = width * scale * (isOverPowered ? 1.5 : 1);

            HalfWidth = Width / 2;
        }

        #endregion
    }
}
