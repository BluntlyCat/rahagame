namespace HSA.RehaGame.Scenes
{
    using UnityEngine;
    using Logging;
    using System.Linq;
    using MusicPlayer;
    using System.Collections.Generic;

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
            logger.AddLogAppender<FileAppender>();

            hud = GameObject.FindGameObjectWithTag("HUD");

            musicPlayer = new MusicPlayer(this.GetComponent<AudioSource>());

            musicPlayer.playlist = Application.loadedLevelName;
            musicPlayer.Play();
        }

        // Update is called once per frame
        void Update()
        {
            if (musicPlayer.isPlaying == false && Pause.IsPause == false)
                musicPlayer.Next();
        }

        private static void LoadNewScene(string scene, bool addPrevious = true)
        {
            logger.Info("Load scene", scene);

            if(addPrevious)
                previousScenes.Add(Application.loadedLevelName);

            Application.LoadLevel(scene);
        }

        public static void LoadExercise(string name)
        {
            LoadNewScene(name);
        }

        public static void MainMenu()
        {
            LoadNewScene("MainMenu");
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

        public void PreviousScene()
        {
            GoOneSceneBack();
        }

        public void KinectView()
        {
            LoadNewScene("KinectView");
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
