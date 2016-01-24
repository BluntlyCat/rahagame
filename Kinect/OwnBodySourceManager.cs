namespace HSA.RehaGame.Kinect
{
    using UnityEngine;
    using System.Collections;
    using Windows.Kinect;
    using Scene;

    public class OwnBodySourceManager : MonoBehaviour
    {
        private static KinectSensor _Sensor;
        private static BodyFrameReader _Reader;
        private Body[] _Data = null;

        public Body[] GetData()
        {
            return _Data;
        }

        public static void ShutdownKinect()
        {
            if (_Reader != null)
            {
                _Reader.Dispose();
                _Reader = null;
            }

            if (_Sensor != null)
            {
                if (_Sensor.IsOpen)
                {
                    _Sensor.Close();
                }

                _Sensor = null;
            }
        }

        void Start()
        {
            _Sensor = KinectSensor.GetDefault();

            if (_Sensor != null)
            {
                _Reader = _Sensor.BodyFrameSource.OpenReader();

                if (!_Sensor.IsOpen)
                {
                    _Sensor.Open();
                }
            }
        }

        void Update()
        {
            if (Pause.Paused != true && _Reader != null)
            {
                var frame = _Reader.AcquireLatestFrame();
                if (frame != null)
                {
                    if (_Data == null)
                    {
                        _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                    }

                    frame.GetAndRefreshBodyData(_Data);

                    frame.Dispose();
                    frame = null;
                }
            }
        }

        void OnApplicationQuit()
        {
            ShutdownKinect();
        }
    }
}
