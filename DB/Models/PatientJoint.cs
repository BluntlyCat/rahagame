namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
    using Mono.Data.Sqlite;

    public class PatientJoint : BaseJoint
    {
        private long id;
        private bool active = true;

        private KinectJoint kinectJoint;

        public PatientJoint(long id)
        {
            this.id = id;
        }

        public PatientJoint(KinectJoint joint)
        {
            this.kinectJoint = joint;
        }

        [PrimaryKey]
        public long ID
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        [TableColumn("active")]
        public bool Active
        {
            get
            {
                return this.active;
            }

            set
            {
                this.active = value;
            }
        }

        [ForeignKey("kinectjoint", "kinectJoint_id", true)]
        public KinectJoint KinectJoint
        {
            get
            {
                if (kinectJoint == null)
                    return null;

                else if (!kinectJoint.IsInstance)
                    kinectJoint.SetData();

                return this.kinectJoint;
            }

            set
            {
                this.kinectJoint = value;
            }
        }
    }
}
