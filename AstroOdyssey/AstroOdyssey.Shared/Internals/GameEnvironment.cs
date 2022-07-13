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

        public void ClearDestroyableGameObjects()
        {
            destroyableGameObjects.Clear();
        }
    }
}
