namespace HSA.RehaGame.Kinect
{
    using System.Collections.Generic;
    using Kinect = Windows.Kinect;

    public class RGJoint
    {
        private RGJoint parent;
        private Dictionary<Kinect.JointType, RGJoint> children = new Dictionary<Kinect.JointType, RGJoint>();
        private Kinect.JointType _type;
        private bool active = true;

        public RGJoint(Kinect.JointType type)
        {
            this._type = type;
        }

        public Kinect.JointType type
        {
            get
            {
                return _type;
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public RGJoint Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public Dictionary<Kinect.JointType, RGJoint> Children
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
            }
        }

        public RGJoint GetChild(Kinect.JointType name)
        {
            if (children.ContainsKey(name))
                return children[name];

            return null;
        }

        public bool Activate(bool active)
        {
            this.Active = active;

            foreach(var child in this.Children)
            {
                child.Value.Activate(active);
            }

            return this.Active;
        }
    }
}
