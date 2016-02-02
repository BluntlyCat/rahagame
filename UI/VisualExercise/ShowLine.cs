namespace HSA.RehaGame.UI.VisualExercise
{
    using UnityEngine;
    using Kinect = Windows.Kinect;
    using rMath = Math;

    [RequireComponent(typeof(LineRenderer))]
    public class ShowLine : Drawable
    {
        public Color color;
        public float lineWidth;

        private LineRenderer lineRenderer;
        private Transform textObject;
        private TextMesh text;

        void Start()
        {
            lineRenderer = this.GetComponent<LineRenderer>();
            lineRenderer.material.color = color;
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetWidth(0, 0);

            textObject = this.transform.FindChild("distance");
            text = textObject.GetComponent<TextMesh>();
        }

        public override void Redraw(params object[] args)
        {
            if (active)
            {
                Kinect.Joint activeJoint = (Kinect.Joint)args[0];
                Kinect.Joint passiveJoint = (Kinect.Joint)args[1];

                float lw = (float)args[2];

                var distance = rMath.Calculations.GetDistance(activeJoint, passiveJoint);
                var textScale = text.transform.localScale;

                var activePosition = rMath.Calculations.GetVector3FromJoint(activeJoint);
                var passivePosition = rMath.Calculations.GetVector3FromJoint(passiveJoint);

                this.transform.position = activePosition;

                lineRenderer.SetWidth(lw, lw);
                lineRenderer.SetPosition(0, activePosition);
                lineRenderer.SetPosition(1, passivePosition);

                var cm = string.Format("{0} cm", distance.x.ToString("000"));

                if (cm[0] == '0')
                    cm = cm.Substring(1);

                text.text = cm;

                textObject.rotation = Quaternion.LookRotation(passivePosition, activePosition) * Quaternion.Euler(0, 0, -90);
                textObject.position = new Vector3(activePosition.x + 1f, activePosition.y + 1f, activePosition.z);
            }
        }

        public override void Clear()
        {
            lineRenderer.SetWidth(0, 0);
            text.text = "";
        }
    }
}
