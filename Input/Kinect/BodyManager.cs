namespace HSA.RehaGame.Input.Kinect
{
    using System.Collections.Generic;
    using Logging;
    using Manager;
    using UnityEngine;
    using Kinect = Windows.Kinect;

    public delegate void BodyDetectedEventHandler(Kinect.Body body);
    public delegate void BodyLostEventHandler();

    public class BodyManager : MonoBehaviour
    {
        public event BodyDetectedEventHandler BodyDetected;
        public event BodyLostEventHandler BodyLost;

        public GameObject bodySourceManager;
        public GameObject bodyViewManager;
        public GameObject exerciseManagerObject;

        private static Logger<BodyManager> logger = new Logger<BodyManager>();

        private BodySourceManager bodyManager;
        private BodyManagerView viewManager;
        //private ExerciseManager exerciseManager;

        private List<ulong> trackedIds = new List<ulong>();
        private Dictionary<ulong, GameObject> kinectBodies = new Dictionary<ulong, GameObject>();

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            bodyManager = bodySourceManager.GetComponent<BodySourceManager>();
            viewManager = bodyViewManager.GetComponent<BodyManagerView>();
            //exerciseManager = exerciseManagerObject.GetComponent<ExerciseManager>();
        }

        private Kinect.Body[] GetBodies()
        {
            var bodies = bodyManager.GetData();

            if (bodies == null)
                return null;

            trackedIds = new List<ulong>();
            foreach (var body in bodies)
            {
                if (body == null)
                    continue;

                if (body.IsTracked)
                    trackedIds.Add(body.TrackingId);
            }

            return bodies;
        }

        private void DeleteUntrackedBodies()
        {
            List<ulong> knownIds = new List<ulong>(kinectBodies.Keys);

            // First delete untracked bodies
            foreach (ulong trackingId in knownIds)
            {
                if (!trackedIds.Contains(trackingId))
                {
                    Destroy(kinectBodies[trackingId]);
                    kinectBodies.Remove(trackingId);
                }
            }
        }

        void Update()
        {
            Kinect.Body[] bodies = GetBodies();

            if (bodies == null)
            {
                if (this.BodyLost != null)
                    this.BodyLost();

                return;
            }

            DeleteUntrackedBodies();

            foreach (var body in bodies)
            {
                if (body == null)
                    continue;

                if (body.IsTracked)
                {
                    if (!kinectBodies.ContainsKey(body.TrackingId))
                        kinectBodies[body.TrackingId] = viewManager.CreateBodyObject(body.TrackingId);

                    viewManager.RefreshBodyObject(body, kinectBodies[body.TrackingId]);

                    if (this.BodyDetected != null)
                        this.BodyDetected(body);

                    return;
                }
            }
        }
    }
}