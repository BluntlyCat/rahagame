namespace HSA.RehaGame.UI.VisualExercise
{
    using Math;
    using UnityEngine;
    using UnityEngine.UI;
    using User;
    using User.Kinect;
    using Kinect = Windows.Kinect;

    public class Drawing : MonoBehaviour
    {
        private Text debugText;
        private Text informationText;
        private GameObject visualInformation;
        private ShowCircle showCircle;

        public GameObject debugTextPrefab;
        public GameObject infoTextPrefab;
        public GameObject circlePrefab;
        public GameObject visualInformationPrefab;

        void Start()
        {
            /*debugTextPrefab = Instantiate(debugTextPrefab) as GameObject;
            debugText = debugTextPrefab.GetComponent<Text>();
            debugText.transform.SetParent(this.transform, false);*/

            visualInformation = Instantiate(visualInformationPrefab);
            visualInformation.transform.SetParent(this.transform, false);

            showCircle = Instantiate(circlePrefab).GetComponent<ShowCircle>();
            showCircle.transform.SetParent(visualInformation.transform, false);

            infoTextPrefab = Instantiate(infoTextPrefab);
            informationText = infoTextPrefab.GetComponent<Text>();
            informationText.transform.SetParent(GameObject.Find("MiddlePanel").transform, false);
        }

        public void ShowInformation(string information)
        {
            if (informationText != null)
                informationText.text = information;
        }

        public void DrawCircle(Kinect.Body body, PatientJoint patientJoint, double initialAngle, double currentAngle)
        {
            var joint = body.Joints[patientJoint.JointType];
            visualInformation.transform.position = Calculations.GetVector3FromJoint(joint);
            showCircle.UpdateCircles((float)initialAngle, (float)currentAngle);
        }

        public void DrawDebug(Kinect.Body body, PatientJoint patientJoint)
        {
            if (debugText != null)
            {
                var joint = body.Joints[patientJoint.JointType];
                var angle = Calculations.GetAngle(KinectJoint.GetJoints(body, patientJoint));
                var position = Calculations.GetVector3FromJoint(joint);

                position.Set(position.x, position.y, position.z - 1);
                debugText.transform.position = position;

                debugText.text = string.Format("Angle: {0}°\nX: {1}\nY: {2}\nZ: {3}", angle.ToString("0"), joint.Position.X.ToString("0.000"), joint.Position.Y.ToString("0.000"), joint.Position.Z.ToString("0.000"));
            }
        }
    }
}
