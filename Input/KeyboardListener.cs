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
        private static int inputElementIndex;

        // Use this for initialization
        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            inputElementIndex = 0;
            overlayMenu = GameObject.FindGameObjectWithTag("OverlayMenu");
        }

        private void EscapePressed()
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                LoadScene.ReturnToWindows();
            }
            else
            {
                
            }
        }

        private void ArrowPressed(Event e)
        {
            var inputElements = GameObject.FindGameObjectsWithTag("inputElement");

            switch (e.keyCode)
            {
                case KeyCode.LeftArrow:
                case KeyCode.UpArrow:
                    inputElementIndex = (inputElementIndex - 1) % inputElements.Length;
                    break;

                case KeyCode.RightArrow:
                case KeyCode.DownArrow:
                    inputElementIndex = (inputElementIndex + 1) % inputElements.Length;
                    break;
            }

            GUI.FocusControl(inputElements[inputElementIndex].name);
        }

        // Update is called once per frame
        void OnGui()
        {
            switch(Event.current.keyCode)
            {
                case KeyCode.Escape:
                    EscapePressed();
                    break;

                case KeyCode.DownArrow:
                case KeyCode.RightArrow:
                case KeyCode.UpArrow:
                case KeyCode.LeftArrow:
                    //ArrowPressed(Event.current);
                    break;
            }
        }
    }
}