using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace SpaceShooterGame
{
    public class GameEnvironment : Canvas
    {
        #region Fields

        private readonly List<GameObject> _destroyableGameObjects = new();

        #endregion

        #region Properties        

        public bool IsWarpingThroughSpace { get; set; }

        public bool IsBossEngaged { get; set; }

        public double HalfWidth => Width > 0 && double.IsFinite(Width) ? Width / 2 : 0;

        public double GameObjectScale { get; set; }

        #endregion

        #region Ctor

        public GameEnvironment()
        {
            CanDrag = false;
        }

        #endregion        

        #region Methods

        #region Public

        /// <summary>
        /// Get frame time buffer according to game view width.
        /// </summary>
        /// <returns></returns>
        public double GetFrameTimeBuffer()
        {
            return 0;
            //switch (Width)
            //{
            //    case <= 400:
            //        return 3.5;
            //    case <= 500:
            //        return 2.5;
            //    case <= 700:
            //        return 2;
            //    case <= 900:
            //        return 1;
            //    default:
            //        return 0;
            //}
        }

        public void SetSize(double height, double width)
        {
            Height = height;
            Width = width;

            SetGameObjectScale();
        }

        public IEnumerable<T> GetGameObjects<T>()
        {
            return Children.OfType<T>();
        }

        public void AddGameObject(GameObject gameObject)
        {
            Children.Add(gameObject);
        }

        public void AddDestroyableGameObject(GameObject destroyable)
        {
            _destroyableGameObjects.Add(destroyable);
        }

        public void RemoveDestroyableGameObjects()
        {
            foreach (var destroyable in _destroyableGameObjects)
            {
                RemoveGameObject(destroyable);
            }

            ClearDestroyableGameObjects();
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
            _destroyableGameObjects.Clear();
        }

        /// <summary>
        /// Removes a game object from game view if applicable. 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool CheckAndAddDestroyableGameObject(GameObject gameObject)
        {
            // if game object is out of bounds of game view
            if (IsRecyclable(gameObject))
            {
                AddDestroyableGameObject(gameObject);
                return true;
            }

            return false;
        }

        public bool IsRecyclable(GameObject gameObject)
        {
            return gameObject.GetY() > Height || gameObject.GetX() > Width || gameObject.GetX() + gameObject.Width < 0;
        }

        /// <summary>
        /// Get destructible objects that have health and intersects with the projectile.
        /// </summary>
        /// <param name="projectileBounds"></param>
        /// <returns></returns>
        public IEnumerable<GameObject> GetDestructibles(Rect projectileBounds)
        {
            return GetGameObjects<GameObject>().Where(x => x.IsDestructible && x.GetY() > 0 && x.GetRect().Intersects(projectileBounds));
        }

        #endregion

        #region Private

        private void SetGameObjectScale()
        {
            GameObjectScale = ScalingHelper.GetGameObjectScale(Width);
        }

        #endregion

        #endregion
    }
}
