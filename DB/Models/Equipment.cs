namespace HSA.RehaGame.DB.Models
{
    public class Equipment : UnityModel
    {
        private string name;
        private string description;
        private string auditiveDescription;
        private string image;

        public Equipment(string unityObjectName) : base (unityObjectName)
        {

        }

        [TranslationColumn]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.name = value;
            }
        }

        [TranslationColumn]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.description = value;
            }
        }

        [TranslationColumn]
        [ResourceColumn]
        public string AuditiveDescription
        {
            get
            {
                return auditiveDescription;
            }

            set
            {
                this.auditiveDescription = value;
            }
        }

        [ResourceColumn]
        public string Image
        {
            get
            {
                return image;
            }

            set
            {
                this.image = value;
            }
        }
    }
}
