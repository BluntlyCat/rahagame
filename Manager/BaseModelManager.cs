namespace HSA.RehaGame.Manager
{
    using System.Collections.Generic;
    using DB.Models;
    using UnityEngine;

    public class BaseModelManager<T> : MonoBehaviour where T : Model
    {
        protected static IDictionary<object, T> models = Model.All<T>();
    }
}
