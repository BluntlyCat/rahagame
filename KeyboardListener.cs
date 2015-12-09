namespace HSA.RehaGame.Input
{
    using Logging;
    using Scenes;
    using UnityEngine;

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

                if (Application.loadedLevelName == "MainMenu")
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