namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
    using Kinect = Windows.Kinect;

    public class KinectJoint : BaseJoint
    {
        private string name;
        private long value;
        private string translation;

        private bool stressed = false;
        private bool activeInExercise = false;

        private KinectJoint parent;
        private Dictionary<string, KinectJoint> children;

        public KinectJoint(string name)
        {
            this.name = name;
        }

        [PrimaryKey]
        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                this.name = value;
            }
        }

        [TableColumn]
        public long Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        [TranslationColumn]
        public string Translation
        {
            get
            {
                return this.translation;
            }

            set
            {
                this.translation = value;
            }
        }

        [ForeignKey("kinectjoint", "parent_id", true)]
        public KinectJoint Parent
        {
            get
            {
                if (parent == null)
                    return null;

                else if(!parent.IsInstance)
                    parent.SetData();

                return this.parent;
            }

            set
            {
                this.parent = value;
            }
        }

        [ManyToManyRelation(
            "name",
            "kinectjoint",
            "from_kinectjoint_id",
            "kinectjoint_children",
            "to_kinectjoint_id",
            "kinectjoint",
            "name"
        )]
        public Dictionary<string, KinectJoint> Children
        {
            get
            {
                return this.children;
            }

            set
            {
                this.children = value;
            }
        }

        public Kinect.JointType Type
        {
            get
            {
                return (Kinect.JointType)this.value;
            }

            set
            {
                this.value = (long)value;
            }
        }

        public bool Stressed
        {
            get
            {
                return this.stressed;
            }

            set
            {
                this.stressed = value;
            }
        }

        public bool ActiveInExercise
        {
            get
            {
                return this.activeInExercise;
            }

            set
            {
                this.activeInExercise = value;
            }
        }
    }
}
