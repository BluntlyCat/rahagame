namespace HSA.RehaGame.Scene
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    class Pause : MonoBehaviour
    {
        private static bool _PAUSE;

        void Start()
        {
            _PAUSE = true;
            Time.timeScale = _PAUSE ? 0 : 1;
        }

        public static bool SetPause()
        {
            _PAUSE = !_PAUSE;
            Time.timeScale = _PAUSE ? 0 : 1;
            return _PAUSE;
        }

        public static bool Paused
        {
            get
            {
                return _PAUSE;
            }
        }
    }
}
