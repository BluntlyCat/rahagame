namespace HSA.RehaGame.Input.Kinect
{
    using UnityEngine;
    using Windows.Kinect;

    public class BodySourceManager : MonoBehaviour
    {
        private static KinectSensor kinectSensor;
        private static BodyFrameReader kinectReader;
        private Body[] body = null;

        public Body[] GetData()
        {
            return body;
        }

        public static void ShutdownKinect()
        {
            if (kinectReader != null)
            {
                kinectReader.Dispose();
                kinectReader = null;
            }

            if (kinectSensor != null)
            {
                if (kinectSensor.IsOpen)
                {
                    kinectSensor.Close();
                }

                kinectSensor = null;
            }
        }

        void Start()
        {
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                kinectReader = kinectSensor.BodyFrameSource.OpenReader();

                if (!kinectSensor.IsOpen)
                    kinectSensor.Open();
            }
        }

        void Update()
        {
            if (kinectReader == null)
                return;

            var frame = kinectReader.AcquireLatestFrame();

            if (frame != null)
            {
                if (body == null)
                    body = new Body[kinectSensor.BodyFrameSource.BodyCount];

                frame.GetAndRefreshBodyData(body);

                frame.Dispose();
                frame = null;
            }
        }

        void OnApplicationQuit()
        {
            ShutdownKinect();
        }
    }
}
