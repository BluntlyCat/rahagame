namespace HSA.RehaGame.Scenes
{
    using UnityEngine;

    public class Pause : MonoBehaviour
    {
        private static bool pause = false;
        private static GameObject pauseText;

        // Use this for initialization
        void Start()
        {
            pauseText = GameObject.FindGameObjectWithTag("pauseText");
            pauseText.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void SetPause()
        {
            if (pause)
            {
                if (Help.IsHelp)
                {
                    Help.Show();
                }

                if (Development.IsDevelMode)
                {
                    Development.Show();
                }

                pauseText.SetActive(false);
                Time.timeScale = 1;

                LoadScene.musicPlayer.Pause();
            }
            else
            {
                if (Help.IsHelp)
                {
                    Help.Hide();
                }

                if (Development.IsDevelMode)
                {
                    Development.Hide();
                }

                pauseText.SetActive(true);
                Time.timeScale = 0;

                LoadScene.musicPlayer.Pause();
            }

            pause = !pause;
        }

        public static bool IsPause
        {
            get
            {
                return pause;
            }
        }
    }
}