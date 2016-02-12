namespace HSA.RehaGame.DB
{
    using UE = UnityEngine;
    using Models;

    public class Modeltest : UE.MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            var model = Model.GetModel<Exercise>("exercise1");
            UE.Debug.Log(model);
        }
    }
}