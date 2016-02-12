namespace HSA.RehaGame.DB.Models
{
    public class Music : UnityModel
    {
        private string title;

        public Music(string unityObjectName) : base(unityObjectName)
        {

        }

        [TableColumn]
        [ResourceColumn]
        public string Title
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
