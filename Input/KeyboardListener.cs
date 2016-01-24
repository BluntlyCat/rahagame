namespace HSA.RehaGame.Input
{
    using UI;
    using Logging;
    using Scene;
    using UnityEngine;
    using Kinect;
    using UnityEngine.SceneManagement;

    public class KeyboardListener : MonoBehaviour
    {
        private static Logger<KeyboardListener> logger = new Logger<KeyboardListener>();
        private static GameObject overlayMenu;

        // Use this for initialization
        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            overlayMenu = GameObject.FindGameObjectWithTag("OverlayMenu");
        }

        // Update is called once per frame
        void Update()
        {
            Event e = Event.current;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                logger.Debug("KEY PRESS");

                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    LoadScene.ReturnToWindows();
                }
                else
                {
                    if (overlayMenu == null)
                    {
                        LoadScene.GoOneSceneBack();
                    }
                    else
                    {
                        if (overlayMenu.activeSelf)
                        {
                            BodySourceManager.ShutdownKinect();
                            LoadScene.GoOneSceneBack();
                        }
                        else
                        {
                            OverlayMenu.HideMenu();
                        }
                    }
                }
            }
        }
    }
}