namespace HSA.RehaGame.Input
{
    using System;
    using Logging;
    using UnityEngine;

    public delegate void KeyPressedEventHandler(KeyCode keyCode);

    public class KeyboardEventHandler : MonoBehaviour
    {
        private static Logger<KeyboardEventHandler> logger = new Logger<KeyboardEventHandler>();
        public event KeyPressedEventHandler KeyPressed;
        private KeyCode[] keyCodes;

        // Use this for initialization
        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
        }

        protected virtual void OnKeyPressed(KeyCode keyCode)
        {
            if (KeyPressed != null)
                KeyPressed(keyCode);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {
                foreach(var key in keyCodes)
                {
                    if (Input.GetKeyDown(key))
                    {
                        OnKeyPressed(key);
                        return;
                    }
                }
            }
        }
    }
}