namespace HSA.RehaGame.UI.VisualExercise
{
    using UnityEngine;

    public class Circle
    {
        private LineRenderer lineRenderer;
        
        private int segments;
        private float xRadius;
        private float yRadius;

        public Circle(int segments, float xRadius, float yRadius, GameObject gameObject, Color color)
        {
            this.lineRenderer = gameObject.AddComponent<LineRenderer>();

            this.lineRenderer.SetVertexCount(segments + 1);
            this.lineRenderer.useWorldSpace = false;

            this.segments = segments;
            this.xRadius = xRadius;
            this.yRadius = yRadius;

            this.lineRenderer.material.color = color;

            Update(0, 0);
        }

        public void Update(float jointAngle, float zLayer)
        {
            float x;
            float y;
            float z = zLayer;

            float angle = 0;

            for (int i = 0; i < (segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;

                this.lineRenderer.SetPosition(i, new Vector3(x, y, z));

                angle += (jointAngle / segments);
            }
        }
    }
}
