namespace HSA.RehaGame.UI.VisualExercise
{
    using Math;
    using UnityEngine;
    using Kinect = Windows.Kinect;

    public class ShowCircle : Drawable
    {
        public GameObject referenceCircleObject;
        public GameObject currentCircleObject;

        private Circle referenceCircle;
        private Circle currentCircle;

        void Start()
        {
            this.referenceCircle = referenceCircleObject.GetComponent<Circle>();
            this.currentCircle = currentCircleObject.GetComponent<Circle>();
        }

        public override void Redraw(params object[] args)
        {
            if (active)
            {
                object initialAngle = args[0];
                object currentAngle = args[1];
                Kinect.Joint joint = (Kinect.Joint)args[2];
                bool behind = (bool)args[3];

                this.transform.position = Calculations.GetVector3FromJoint(joint);

                referenceCircle.Redraw(initialAngle, joint, behind ? 1 : 0);
                currentCircle.Redraw(currentAngle, joint, !behind ? 0 : 1);
            }
        }

        public override void Clear()
        {
            referenceCircle.Clear();
            currentCircle.Clear();
        }

        public override void SetActive(bool active)
        {
            referenceCircle.SetActive(active);
            currentCircle.SetActive(active);

            this.active = active;
        }
    }
}
