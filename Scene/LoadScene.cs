namespace HSA.RehaGame.Scene
{
    using UnityEngine;
    using Logging;
    using System.Linq;
    using MusicPlayer;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;
    using InGame;

    public class LoadScene : MonoBehaviour
    {
        private static Logger<LoadScene> logger = new Logger<LoadScene>();

        private static GameObject menuItems;
        private static List<string> previousScenes = new List<string>();

        public static MusicPlayer musicPlayer;

        // Use this for initialization
        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            musicPlayer = new MusicPlayer(this.GetComponent<AudioSource>());

            musicPlayer.playlist = SceneManager.GetActiveScene().name;
            musicPlayer.Play();
        }

        private static void LoadNewScene(string scene, bool addPrevious = true)
        {
            if (addPrevious)
                previousScenes.Add(SceneManager.GetActiveScene().name);

            SceneManager.LoadScene(scene);
        }

        public static void LoadExercise()
        {
            if (GameState.ActiveExercise != null)
                LoadNewScene(GameState.ActiveExercise.UnityObjectName);
        }

        public static void ReloadSettings()
        {
            LoadNewScene("Settings", false);
        }

        public static void LoadNewUser()
        {
            LoadNewScene("NewUser");
        }
        public static void LoadStatistics()
        {
            LoadNewScene("Statistics");
        }

        public static void ReturnToWindows()
        {
            Application.Quit();
        }

        public static void GoOneSceneBack()
        {
            var last = previousScenes.Last();
            previousScenes.Remove(last);

            LoadNewScene(last, false);
        }

        public static void LoadTrainingMode()
        {
            LoadNewScene("TrainingMode");
        }

        public static void LoadMainMenu()
        {
            LoadNewScene("MainMenu");
        }

        public static void LoadUsersSlection()
        {
            LoadNewScene("UserSelection");
        }

        public void PreviousScene()
        {
            GoOneSceneBack();
        }

        public void NewUser()
        {
            LoadNewScene("NewUser");
        }

        public void UserSelection()
        {
            LoadNewScene("UserSelection");
        }

        public void TrainingMode()
        {
            LoadNewScene("TrainingMode");
        }

        public void Settings()
        {
            LoadNewScene("Settings");
        }

        public void Credits()
        {
            LoadNewScene("Credits");
        }

        public void MainMenu()
        {
            LoadNewScene("MainMenu");
        }

        public void Exit()
        {
            ReturnToWindows();
        }
    }
}
