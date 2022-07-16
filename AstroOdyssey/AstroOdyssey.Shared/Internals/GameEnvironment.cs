using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;

namespace AstroOdyssey
{
    public class GameEnvironment : Canvas
    {
        private readonly List<GameObject> destroyableGameObjects = new List<GameObject>();

        public GameEnvironment()
        {

        }

        /// <summary>
        /// Gets scaling factor for a game object according to game view width.
        /// </summary>
        /// <returns></returns>
        public double GetGameObjectScale()
        {
            return Width <= 500 ? 0.6 : (Width <= 700 ? 0.8 : (Width <= 800 ? 0.9 : 1));
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

        public void RemoveGameObject(GameObject destroyable)
        {
            Children.Remove(destroyable);
        }

        public void RemoveDestroyableGameObjects()
        {
            foreach (var destroyable in destroyableGameObjects)
            {
                RemoveGameObject(destroyable);
            }

            ClearDestroyableGameObjects();
        }

        public void ClearDestroyableGameObjects()
        {
            destroyableGameObjects.Clear();
        }
    }
}
