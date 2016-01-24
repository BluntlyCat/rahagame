namespace HSA.RehaGame.DB
{
    using Settings;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]

    public class GetMenuHeader : MonoBehaviour
    {
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            var text = this.GetComponent<Text>();
            var header = DBManager.GetMenuHeader(this.name);

            text.text = header["name"].ToString();
            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = header["clip"] as AudioClip;

            if(RGSettings.readingAloud)
                audioSource.Play();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}