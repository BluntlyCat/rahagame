namespace HSA.RehaGame.Manager
{
    using System.Collections.Generic;
    using System.Linq;
    using Logging;
    using UnityEngine;
    using USM = UnityEngine.SceneManagement;

    public class SceneManager : MonoBehaviour
    {
        private static Logger<SceneManager> logger = new Logger<SceneManager>();

        private static GameObject menuItems;
        private static List<string> previousScenes = new List<string>();
        private static string sceneName;

        private int sceneCount;
        private USM.Scene[] scenes;

        // Use this for initialization
        void Awake()
        {
            logger.AddLogAppender<ConsoleAppender>();

            sceneCount = USM.SceneManager.sceneCount;
            scenes = new USM.Scene[sceneCount];
            sceneName = USM.SceneManager.GetActiveScene().name;

            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = USM.SceneManager.GetSceneAt(i);
            }
        }

        public static string SceneName
        {
            get
            {
                return sceneName;
            }
        }

        private void LoadNewScene(string scene, bool addPrevious = true)
        {
            if (addPrevious)
                previousScenes.Add(USM.SceneManager.GetActiveScene().name);

            USM.SceneManager.LoadScene(scene);
        }

        public void LoadExercise()
        {
            if (GameManager.ActiveExercise != null)
                this.LoadNewScene(GameManager.ActiveExercise.UnityObjectName);
        }

        public void ReloadSettingsMenu()
        {
            this.LoadNewScene("Settings", false);
        }

        public void LoadNewPatientMenu(bool setActivePatientNull = false)
        {
            if (setActivePatientNull)
                GameManager.ActivePatient = null;

            this.LoadNewScene("NewPatientMenu");
        }
        public void LoadStatisticMenu()
        {
            this.LoadNewScene("Statistic");
        }

        public void ReturnToWindows()
        {
            Application.Quit();
        }

        public void GoOneSceneBack()
        {
            var last = previousScenes.Last();
            previousScenes.Remove(last);

            this.LoadNewScene(last, false);
        }

        public void LoadExerciseSelectionMenu()
        {
            this.LoadNewScene("ExerciseSelectionMenu");
        }

        public void LoadTitleMenu()
        {
            if (GameManager.ActivePatient == null)
                this.LoadPatientSlectionMenu();
            else
                this.LoadNewScene("TitleMenu");
        }

        public void LoadPatientSlectionMenu()
        {
            this.LoadNewScene("PatientSelectionMenu");
        }

        public void LoadScene(GameObject gameObject)
        {
            this.LoadNewScene(gameObject.name, true);
        }

        public void PreviousScene()
        {
            GoOneSceneBack();
        }
    }
}
