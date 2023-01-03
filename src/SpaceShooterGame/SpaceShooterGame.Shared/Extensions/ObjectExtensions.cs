using System.Collections.Generic;

namespace SpaceShooterGame
{
    public static class ObjectExtensions
    {
        public static List<KeyValuePair<string, string>> GetProperties(this object me)
        {
            List<KeyValuePair<string, string>> result = new();

            foreach (var property in me.GetType().GetProperties())
            {
                result.Add(new KeyValuePair<string, string>(property.Name, property.GetValue(me).ToString()));
            }

            return result;
        }
    }
}
