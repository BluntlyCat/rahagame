namespace HSA.RehaGame.Scene
{
    using UnityEngine;

    class Pause : MonoBehaviour
    {
        private static bool pause;

        void Start()
        {
            pause = false;
            Time.timeScale = pause ? 0 : 1;
        }

        public static bool SetPause()
        {
            pause = !pause;
            Time.timeScale = pause ? 0 : 1;
            return pause;
        }

        public bool IsPause
        {
            get
            {
                return pause;
            }
        }
    }
}
