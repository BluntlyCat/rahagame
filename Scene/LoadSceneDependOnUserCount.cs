namespace HSA.RehaGame.Scene
{
    using DB.Models;
    using Manager;
    using UnityEngine;

    public class LoadSceneDependOnUserCount : MonoBehaviour
    {
        public GameObject gameManager;

        private SceneManager sceneManager;
        private int userCount;

        // Use this for initialization
        void Start()
        {
            sceneManager = gameManager.GetComponent<SceneManager>();
            userCount = Model.All<Patient>().Count;
        }

        // Update is called once per frame
        public void LoadSceneDependingOnUserCount()
        {
            if (userCount > 0)
                sceneManager.LoadPatientSlectionMenu();
            else
                sceneManager.LoadNewPatientMenu();
        }
    }
}