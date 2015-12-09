namespace HSA.RehaGame.KinectManager
{
    using System;
    using Logging;
    using UnityEngine;
    using Windows.Kinect;

    public class KinectManager : MonoBehaviour
    {
        private static Logger<KinectManager> logger = new Logger<KinectManager>();

        private KinectSensor kinectSensor;
        private BodyFrameReader kinectReader;
        private Body[] kinectBodies = null;

        private GameObject player;
        private Bones bones;

        public void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            logger.AddLogAppender<FileAppender>();

            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                this.kinectReader = this.kinectSensor.BodyFrameSource.OpenReader();

                if (!this.kinectSensor.IsOpen)
                    this.kinectSensor.Open();

                this.bones = new Bones();
                this.player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        public void Update()
        {
            try
            {
                if (this.kinectReader != null)
                {
                    var frame = this.kinectReader.AcquireLatestFrame();

                    if (frame != null)
                    {
                        if (kinectBodies == null)
                            kinectBodies = new Body[kinectSensor.BodyFrameSource.BodyCount];

                        frame.GetAndRefreshBodyData(kinectBodies);
                        frame.Dispose();

                        foreach (var body in kinectBodies)
                        {
                            if (body == null)
                                continue;

                            else if (body.IsTracked)
                                bones.SetBonePosition(body, player);
                        }

                        kinectBodies = null;
                        frame = null;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal(e);
            }
        }

        public void OnApplicationQuit()
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
    }
}