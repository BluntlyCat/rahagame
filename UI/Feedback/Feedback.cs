namespace HSA.RehaGame.UI.Feedback
{
    using AuditiveExercise;
    using AuditiveExercise.PitchSounds;
    using DB.Models;
    using Logging;
    using Manager;
    using Manager.Audio;
    using Math;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using VisualExercise;
    using Kinect = Windows.Kinect;
    public class Feedback : MonoBehaviour
    {
        private static Logger<Feedback> logger = new Logger<Feedback>();

        public GameObject gameManager;
        public GameObject debugTextPrefab;
        public GameObject infoTextPrefab;
        public GameObject circlePrefab;
        public GameObject linePrefab;
        public GameObject informationPanel;
        public GameObject sidebar;

        private Text informationText;
        private ShowCircle showCircle;
        private ShowLine showLine;

        private IList<IDrawable> drawables = new List<IDrawable>();

        private IDictionary<string, Text> debugTexts = new Dictionary<string, Text>();

        private PitchManager pitchManager;
        private GameObject patient;
        private IDictionary<object, KinectJoint> joints;

        private bool notVisualized = true;

        void Start()
        {
            joints = Model.All<KinectJoint>();

            logger.AddLogAppender<ConsoleAppender>();

            pitchManager = gameManager.GetComponentInChildren<PitchManager>();
        }

        public void VisualizePatient()
        {
            patient = GameObject.Find(GameManager.ActivePatient.Name);

            if (notVisualized)
            {
                foreach (var joint in joints)
                {
                    Text debugText;

                    debugTextPrefab = Instantiate(debugTextPrefab);
                    debugText = debugTextPrefab.GetComponent<Text>();
                    debugText.name = string.Format("{0}_debugText", joint);
                    debugText.transform.SetParent(patient.transform, true);
                    debugTexts.Add(joint.ToString(), debugText);
                }

                circlePrefab = Instantiate(circlePrefab);
                circlePrefab.transform.SetParent(sidebar.transform, false);
                showCircle = circlePrefab.GetComponent<ShowCircle>();
                drawables.Add(showCircle);

                linePrefab = Instantiate(linePrefab);
                linePrefab.transform.SetParent(patient.transform, true);
                showLine = linePrefab.GetComponent<ShowLine>();
                drawables.Add(showLine);

                infoTextPrefab = Instantiate(infoTextPrefab);
                informationText = infoTextPrefab.GetComponent<Text>();
                informationText.transform.SetParent(informationPanel.transform, false);

                notVisualized = false;
            }
        }

        public void ShowInformation(string information)
        {
            if (informationText != null)
                informationText.text = information;
        }

        public void PitchValue(PitchType pitchType, params object[] value)
        {
            pitchManager.Pitch(pitchType, value);
        }

        public void PitchFullfilledSound()
        {
            pitchManager.Pitch(PitchType.pitchFullfilled);
        }

        public void DrawCircle(double initialAngle, double currentAngle, KinectJoint joint)
        {
            showCircle.Active = true;
            showCircle.Redraw(initialAngle, currentAngle, joint);
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

        public void DrawDebug(Kinect.Body body, IDictionary<string, KinectJoint> stressedJoints)
        {
            foreach (var patientJoint in stressedJoints.Values)
            {
                var name = patientJoint.Name;
                var debugText = debugTexts[name];
                var joint = body.Joints[patientJoint.Type];
                var angle = Calculations.GetAngle(
                    body.Joints[patientJoint.Type], body.Joints[patientJoint.Parent.Type], body.Joints[patientJoint.Children.First().Value.Type]
                    );
                var position = Calculations.GetVector3FromJoint(joint);

                position.Set(position.x, position.y, position.z - 1);
                debugText.transform.position = position;

                debugText.text = string.Format("Angle: {0}°\nX: {1}\nY: {2}\nZ: {3}", angle.ToString("0"), joint.Position.X.ToString("0.000"), joint.Position.Y.ToString("0.000"), joint.Position.Z.ToString("0.000"));
            }
        }
        public void DrawDebug(Kinect.Body body, KinectJoint patientJoint)
        {
            var debugText = debugTexts[patientJoint.Name];
            var joint = body.Joints[patientJoint.Type];
            var angle = Calculations.GetAngle(
                joint, body.Joints[patientJoint.Parent.Type], body.Joints[patientJoint.Children.First().Value.Type]
                );
            var position = Calculations.GetVector3FromJoint(joint);

            position.Set(position.x, position.y, position.z - 1);
            debugText.transform.position = position;

            debugText.text = string.Format("Angle: {0}°\nX: {1}\nY: {2}\nZ: {3}", angle.ToString("0"), joint.Position.X.ToString("0.000"), joint.Position.Y.ToString("0.000"), joint.Position.Z.ToString("0.000"));
        }
    }
}
