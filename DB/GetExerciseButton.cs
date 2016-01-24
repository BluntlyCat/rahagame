namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]

    public class GetExerciseButton : MonoBehaviour
    {
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            var list = DBManager.Query("SELECT name, thumbnail from editor_exercise WHERE unityObjectName = '" + this.name + "'");
            var text = this.GetComponentInChildren<Text>();

            text.text = list[0]["name"].ToString();

            var imageFile = list[0]["thumbnail"].ToString().Replace(".jpg", "").Replace("Resources/", "");
            
            this.GetComponentInChildren<RawImage>().texture = Resources.Load(imageFile) as Texture2D;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Play()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Play();
            }
        }
    }
}