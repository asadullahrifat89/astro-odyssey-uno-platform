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

        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets scaling factor for a game object according to game view width.
        /// </summary>
        /// <returns></returns>
        public double GetGameObjectScale()
        {
            return Width >= 1000 ? 1 : (Width <= 300 ? 0.75 : (Width <= 500 ? 0.80 : (Width <= 700 ? 0.75 : (Width <= 900 ? 0.90 : 1))));
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
            return GetGameObjects<GameObject>().Where(destructible => destructible.IsDestructible && destructible.HasHealth && destructible.GetRect().Intersects(projectileBounds));
        }

        #endregion
    }
}
