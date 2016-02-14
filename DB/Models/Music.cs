namespace HSA.RehaGame.DB.Models
{
    using UnityEngine;

    public class Music : UnityModel
    {
        private AudioClip title;

        public Music(string unityObjectName) : base(unityObjectName)
        {

        }

        [TableColumn]
        [ResourceColumn]
        public AudioClip Title
        {
            get
            {
                return title;
            }

            set
            {
                this.title = value;
            }
        }
    }
}
