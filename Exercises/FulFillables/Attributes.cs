namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;

    public class Attributes
    {
        private IDictionary<string, object> attributes = new Dictionary<string, object>();

        public T GetAttribute<T>(string key) where T : class
        {
            return attributes[key] as T;
        }

        public void AddAttribute<T>(string key, T attribute) where T : class
        {
            attributes.Add(key, attribute);
        }
    }
}
