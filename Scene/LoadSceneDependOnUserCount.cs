namespace HSA.RehaGame.Scene
{
    using DB;
    using InGame;
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
            userCount = gameManager.GetComponent<Database>().Query("editor_patient", "SELECT name FROM editor_patient;").RowCount;
        }

        // Update is called once per frame
        public void LoadSceneDependingOnUserCount()
        {
            if (userCount > 0)
                sceneManager.LoadUsersSlection();
            else
                sceneManager.LoadNewUser();
        }
    }
}