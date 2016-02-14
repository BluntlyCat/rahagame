namespace HSA.RehaGame.Scene
{
    using System;
    using UnityEngine;

    public class Timeout : MonoBehaviour
    {
        public double seconds;

        private double now;
        private double start;
        private double timeDelta;

        private LoadSceneDependOnUserCount dependLoader;

        // Use this for initialization
        void Start()
        {
            dependLoader = this.GetComponent<LoadSceneDependOnUserCount>();
            now = DateTime.Now.TimeOfDay.TotalSeconds;
            start = now;
        }

        // Update is called once per frame
        void Update()
        {
            now = DateTime.Now.TimeOfDay.TotalSeconds;
            timeDelta = now - start;

            if (timeDelta >= seconds)
                dependLoader.LoadSceneDependingOnUserCount();
        }
    }
}
