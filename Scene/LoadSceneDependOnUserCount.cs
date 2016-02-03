namespace HSA.RehaGame.Scene
{
    using DB;
    using UnityEngine;

    public class LoadSceneDependOnUserCount : MonoBehaviour
    {
        private int userCount;

        // Use this for initialization
        void Start()
        {
            userCount = DBManager.Query("editor_patient", "SELECT name FROM editor_patient;").RowCount;
        }

        // Update is called once per frame
        public void LoadSceneDependingOnUserCount()
        {
            if (userCount > 0)
                LoadScene.LoadUsersSlection();
            else
                LoadScene.LoadNewUser();
        }
    }
}