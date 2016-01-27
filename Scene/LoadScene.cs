namespace HSA.RehaGame.Scene
{
    using UnityEngine;
    using Logging;
    using System.Linq;
    using MusicPlayer;
    using System.Collections.Generic;
    using Scene;
    using UnityEngine.SceneManagement;

    public class LoadScene : MonoBehaviour
    {
        private static Logger<LoadScene> logger = new Logger<LoadScene>();

        private static GameObject menuItems;
        private static GameObject hud;
        private static List<string> previousScenes = new List<string>();

        public static MusicPlayer musicPlayer;

        // Use this for initialization
        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();

            hud = GameObject.FindGameObjectWithTag("HUD");

            musicPlayer = new MusicPlayer(this.GetComponent<AudioSource>());
            
            musicPlayer.playlist = SceneManager.GetActiveScene().name;
            musicPlayer.Play();
        }

        // Update is called once per frame
        void Update()
        {
            if (musicPlayer != null && musicPlayer.isPlaying == false && Pause.Paused == false)
                musicPlayer.Next();
        }

        private static void LoadNewScene(string scene, bool addPrevious = true)
        {
            logger.Info("Load scene", scene);

            if(addPrevious)
                previousScenes.Add(SceneManager.GetActiveScene().name);

            SceneManager.LoadScene(scene);
        }

        public static void LoadExercise(string name)
        {
            LoadNewScene(name);
        }

        public static void ReloadSettings()
        {
            LoadNewScene("Settings", false);
        }

        public static void MainMenu()
        {
            LoadNewScene("MainMenu");
        }

        public static void LoadNewUser()
        {
            LoadNewScene("NewUser");
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

        public static void LoadUser()
        {
            LoadNewScene("NewUser");
        }

        public void PreviousScene()
        {
            GoOneSceneBack();
        }

        public void NewUser()
        {
            LoadNewScene("NewUser");
        }

        public void Users()
        {
            LoadNewScene("Users");
        }

        public void TrainingMode()
        {
            LoadNewScene("TrainingMode");
        }

        public void Settings()
        {
            LoadNewScene("Settings");
        }

        public void Exit()
        {
            ReturnToWindows();
        }
    }
}
