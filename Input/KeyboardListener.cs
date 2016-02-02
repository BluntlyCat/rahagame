namespace HSA.RehaGame.Input
{
    using InGame;
    using Logging;
    using Scene;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class KeyboardListener : MonoBehaviour
    {
        private static Logger<KeyboardListener> logger = new Logger<KeyboardListener>();

        // Use this for initialization
        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
        }

        private void EscapePressed()
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                LoadScene.ReturnToWindows();
            }
            else
            {
                if (GameState.ActiveExercise != null && (GameState.ExerciseIsActive || !GameState.HasKinectUser))
                {
                    LoadScene.LoadTrainingMode();
                }
                else
                {
                    LoadScene.GoOneSceneBack();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                EscapePressed();
        }
    }
}