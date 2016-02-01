namespace HSA.RehaGame.UI.VisualExercise
{
    using UnityEngine;

    [RequireComponent(typeof(LineRenderer))]
    public class Circle : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        
        public int segments;
        public float xRadius;
        public float yRadius;
        public Color color;

        void Start()
        {
            this.lineRenderer = this.GetComponent<LineRenderer>();

            this.lineRenderer.SetVertexCount(segments + 1);
            this.lineRenderer.useWorldSpace = false;
            this.lineRenderer.material.color = color;

            UpdateCircle(0, 0);
        }

        public void UpdateCircle(float jointAngle, float zLayer)
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
