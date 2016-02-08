namespace HSA.RehaGame.DB
{
    using UE = UnityEngine;
    using Models;

    public class Modeltest : UE.MonoBehaviour
    {
        public UE.GameObject database;

        // Use this for initialization
        void Start()
        {
            IDatabase db = database.GetComponent<Database>();
            Model model = new Joint("SpineShoulder", db);
            model.Get();

            UE.Debug.Log(model);
        }
    }
}