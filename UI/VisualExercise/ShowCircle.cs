namespace HSA.RehaGame.UI.VisualExercise
{
    using DB.Models;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShowCircle : Drawable
    {
        public GameObject referenceCircleObject;
        public GameObject currentCircleObject;
        public GameObject angleTextObject;
        public GameObject jointNameObject;

        private Circle referenceCircle;
        private Circle currentCircle;
        private Text angleText;
        private Text jointName;

        void Awake()
        {
            this.referenceCircle = referenceCircleObject.GetComponent<Circle>();
            this.currentCircle = currentCircleObject.GetComponent<Circle>();
            this.angleText = angleTextObject.GetComponent<Text>();
            this.jointName = jointNameObject.GetComponent<Text>();
        }

        public override void Redraw(params object[] args)
        {
            if (active)
            {
                object initialAngle = args[0];
                double currentAngle = (double)args[1];
                KinectJoint activeJoint = (KinectJoint)args[2];

                angleText.text = string.Format("{0} °", currentAngle.ToString("0"));
                jointName.text = activeJoint.Translation;
                referenceCircle.Redraw(initialAngle);
                currentCircle.Redraw(currentAngle);
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
