namespace HSA.RehaGame.UI.VisualExercise
{
    using System.Collections.Generic;
    using DB;
    using InGame;
    using Logging;
    using Math;
    using UnityEngine;
    using UnityEngine.UI;
    using User;
    using User.Kinect;
    using Kinect = Windows.Kinect;

    public class Drawing : MonoBehaviour
    {
        public GameObject dbManager;
        private static Logger<Drawing> logger = new Logger<Drawing>();

        private Text informationText;
        private ShowCircle showCircle;
        private ShowLine showLine;

        private IList<IDrawable> drawables = new List<IDrawable>();
        private IDictionary<string, Text> debugTexts = new Dictionary<string, Text>();

        public GameObject debugTextPrefab;
        public GameObject infoTextPrefab;
        public GameObject circlePrefab;
        public GameObject linePrefab;

        private GameObject patient;

        void Start()
        {
            var joints = dbManager.GetComponent<Database>().Query("editor_joint", "SELECT name FROM editor_joint");

            logger.AddLogAppender<ConsoleAppender>();

            patient = GameObject.Find(GameState.ActivePatient.Name);

            foreach (var joint in joints.Rows)
            {
                Text debugText;
                string name = joint.GetValue("name");

                debugTextPrefab = Instantiate(debugTextPrefab);
                debugText = debugTextPrefab.GetComponent<Text>();
                debugText.name = string.Format("{0}_debugText", name);
                debugText.transform.SetParent(patient.transform, true);
                debugTexts.Add(name, debugText);
            }

            circlePrefab = Instantiate(circlePrefab);
            circlePrefab.transform.SetParent(patient.transform, true);
            showCircle = circlePrefab.GetComponent<ShowCircle>();
            drawables.Add(showCircle);

            linePrefab = Instantiate(linePrefab);
            linePrefab.transform.SetParent(patient.transform, true);
            showLine = linePrefab.GetComponent<ShowLine>();
            drawables.Add(showLine);

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

            showCircle.Active = true;
            showCircle.Redraw(initialAngle, currentAngle, joint, initialAngle >= currentAngle);
        }

        public void DrawLine(Kinect.Joint activeJoint, Kinect.Joint passiveJoint, float lineWidth)
        {
            showLine.Active = true;
            showLine.Redraw(activeJoint, passiveJoint, lineWidth);
        }

        public void ClearDrawings()
        {
            foreach (var drawable in drawables)
                drawable.Clear();
        }

        public void DrawDebug(Kinect.Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            foreach (var patientJoint in stressedJoints.Values)
            {
                var name = patientJoint.JointType.ToString();
                var debugText = debugTexts[name];
                var joint = body.Joints[patientJoint.JointType];
                var angle = Calculations.GetAngle(KinectJoint.GetJoints(body, patientJoint));
                var position = Calculations.GetVector3FromJoint(joint);

                position.Set(position.x, position.y, position.z - 1);
                debugText.transform.position = position;

                debugText.text = string.Format("Angle: {0}°\nX: {1}\nY: {2}\nZ: {3}", angle.ToString("0"), joint.Position.X.ToString("0.000"), joint.Position.Y.ToString("0.000"), joint.Position.Z.ToString("0.000"));
            }
        }
        public void DrawDebug(Kinect.Body body, PatientJoint patientJoint)
        {
            var debugText = debugTexts[patientJoint.JointType.ToString()];
            var joint = body.Joints[patientJoint.JointType];
            var angle = Calculations.GetAngle(KinectJoint.GetJoints(body, patientJoint));
            var position = Calculations.GetVector3FromJoint(joint);

            position.Set(position.x, position.y, position.z - 1);
            debugText.transform.position = position;

            debugText.text = string.Format("Angle: {0}°\nX: {1}\nY: {2}\nZ: {3}", angle.ToString("0"), joint.Position.X.ToString("0.000"), joint.Position.Y.ToString("0.000"), joint.Position.Z.ToString("0.000"));
        }
    }
}
