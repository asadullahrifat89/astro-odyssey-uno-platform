using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace AstroOdyssey
{
    public class GameEnvironment : Canvas
    {
        #region Fields

        private readonly List<GameObject> destroyableGameObjects = new List<GameObject>();

        #endregion

        #region Ctor

        public GameEnvironment()
        {
            CanDrag = false;
        }

        #endregion

        #region Properties

        public bool IsWarpingThroughSpace { get; set; }

        public bool IsBossEngaged { get; set; }

        public double HalfWidth => Width > 0 && double.IsFinite(Width) ? Width / 2 : 0;

        #endregion

        #region Methods

        /// <summary>
        /// Gets scaling factor for a game object according to game view width.
        /// </summary>
        /// <returns></returns>
        public double GetGameObjectScale()
        {
            switch (Width)
            {
                case <= 300:
                    return 0.70;
                case <= 500:
                    return 0.75;
                case <= 700:
                    return 0.85;
                case <= 900:
                    return 0.90;
                case <= 1000:
                    return 1;
                case <= 1400:
                    return 1.1;
                case <= 2000:
                    return 1.2;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Get frame time buffer according to game view width.
        /// </summary>
        /// <returns></returns>
        public double GetFrameTimeBuffer()
        {
            switch (Width)
            {
                case <= 400:
                    return 3.5;
                case <= 500:
                    return 3;
                case <= 700:
                    return 2;
                case <= 900:
                    return 1;
                default:
                    return 0;
            }
        }

        public void SetSize(double height, double width)
        {
            Height = height;
            Width = width;
        }

        public IEnumerable<T> GetGameObjects<T>()
        {
            return Children.OfType<T>();
        }

        public List<GameObject> GetDestroyableGameObjects()
        {
            return destroyableGameObjects;
        }

        public void AddGameObject(GameObject gameObject)
        {
            Children.Add(gameObject);
        }

        public void AddDestroyableGameObject(GameObject destroyable)
        {
            destroyableGameObjects.Add(destroyable);
        }

        public void RemoveDestroyableGameObjects()
        {
            if (Parallel.ForEach(destroyableGameObjects, destroyable =>
            {
                RemoveGameObject(destroyable);

            }).IsCompleted)
            {
                ClearDestroyableGameObjects();
            }
        }

        public void RemoveGameObject(GameObject destroyable)
        {
            Children.Remove(destroyable);
        }

        /// <summary>
        /// Clears destroyable objects collection.
        /// </summary>
        public void ClearDestroyableGameObjects()
        {
            destroyableGameObjects.Clear();
        }

        /// <summary>
        /// Removes a game object from game view if applicable. 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool CheckAndAddDestroyableGameObject(GameObject gameObject)
        {
            // if game object is out of bounds of game view
            if (gameObject.GetY() > Height || gameObject.GetX() > Width || gameObject.GetX() + gameObject.Width < 0)
            {
                AddDestroyableGameObject(gameObject);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get destructible objects that have health and intersects with the projectile.
        /// </summary>
        /// <param name="projectileBounds"></param>
        /// <returns></returns>
        public IEnumerable<GameObject> GetDestructibles(Rect projectileBounds)
        {
            return GetGameObjects<GameObject>().Where(x => x.IsDestructible && x.HasHealth && !x.IsMarkedForFadedDestruction && x.GetRect().Intersects(projectileBounds));
        }

        #endregion
    }

    public enum GameLevel
    {
        Level_1,
        Level_2,
        Level_3,
        Level_4,
        Level_5,
        Level_6,
        Level_7,
        Level_8,
        Level_9,
        Level_10,
        Level_11,
        Level_12,
        Level_13,
    }
}
