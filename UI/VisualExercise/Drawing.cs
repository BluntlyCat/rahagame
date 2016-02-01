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
        private static Text debugText;
        private static Text informationText;
        private static GameObject visualInformation;

        public GameObject debugTextPrefab;
        public GameObject infoTextPrefab;

        private static ShowCircle showCircle;

        void Start()
        {
            /*debugTextPrefab = Instantiate(debugTextPrefab) as GameObject;
            debugText = debugTextPrefab.GetComponent<Text>();
            debugText.transform.SetParent(this.transform, false);*/

            infoTextPrefab = Instantiate(infoTextPrefab) as GameObject;
            informationText = infoTextPrefab.GetComponent<Text>();
            informationText.transform.SetParent(GameObject.Find("MiddlePanel").transform, false);

            visualInformation = GameObject.Find("VisualInformation");

            showCircle = new ShowCircle(new Circle(100, 100, 100, GameObject.Find("Reference"), Color.black),
                new Circle(100, 100, 100, GameObject.Find("Current"), new Color(255, 255, 255, 64)));
        }

        public static void ShowInformation(string information)
        {
            if (informationText != null)
                informationText.text = information;
        }

        public static void DrawCircle(Kinect.Body body, PatientJoint patientJoint, double initialAngle, double currentAngle)
        {
            if (visualInformation != null)
            {
                var joint = body.Joints[patientJoint.JointType];
                visualInformation.transform.position = Calculations.GetVector3FromJoint(joint);
                showCircle.Update((float)initialAngle, (float)currentAngle);
            }
        }

        public static void DrawDebug(Kinect.Body body, PatientJoint patientJoint)
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
