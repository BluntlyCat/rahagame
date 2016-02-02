namespace HSA.RehaGame.Input.Kinect
{
    using Logging;
    using UnityEngine;
    using Kinect = Windows.Kinect;

    public class BodyManager : MonoBehaviour
    {
        public GameObject bodySourceManager;

        private static Logger<BodyManager> logger = new Logger<BodyManager>();
        private BodySourceManager bodyManager;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            bodyManager = bodySourceManager.GetComponent<BodySourceManager>();
        }

        public Kinect.Body GetBody()
        {
            var bodies = bodyManager.GetData();

            if (bodies == null)
                return null;

            foreach (var body in bodies)
            {
                if (body == null)
                    continue;

                if (body.IsTracked)
                    return body;
            }

            return null;
        }
    }
}