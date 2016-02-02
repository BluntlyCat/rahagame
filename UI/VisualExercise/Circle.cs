namespace HSA.RehaGame.UI.VisualExercise
{
    using System;
    using Logging;
    using Math;
    using UnityEngine;
    using Kinect = Windows.Kinect;

    [RequireComponent(typeof(LineRenderer))]
    public class Circle : Drawable
    {
        private static Logger<Circle> logger = new Logger<Circle>();

        private LineRenderer lineRenderer;
        
        public int segments;
        public float lineWidth;
        public float xRadius;
        public float yRadius;
        public Color color;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            this.lineRenderer = this.GetComponent<LineRenderer>();

            this.lineRenderer.SetVertexCount(segments + 1);
            this.lineRenderer.material.color = color;
            this.lineRenderer.useWorldSpace = false;

            Clear();
        }

        private void CreatePoints(float z, float toAngle)
        {
            float x;
            float y;

            float angle = 0f;

            for (int i = 0; i < (segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * this.xRadius;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * this.yRadius;

                lineRenderer.SetPosition(i, new Vector3(x, y, z));

                angle += (toAngle / segments);
            }
        }

        public override void Redraw(params object[] args)
        {
            if (active)
            {
                float toAngle = Convert.ToSingle(args[0]);
                Kinect.Joint joint = (Kinect.Joint)args[1];
                float z = Convert.ToSingle(args[2]);

                this.transform.position = Calculations.GetVector3FromJoint(joint);
                CreatePoints(z, toAngle);
            }
        }

        public override void Clear()
        {
            this.lineRenderer.SetWidth(0, 0);
            CreatePoints(1f, 0);
            this.lineRenderer.SetWidth(lineWidth, lineWidth);
        }
    }
}
