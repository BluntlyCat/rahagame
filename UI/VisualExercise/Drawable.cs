namespace HSA.RehaGame.UI.VisualExercise
{
    using UnityEngine;

    public abstract class Drawable : MonoBehaviour , IDrawable
    {
        protected bool active = true;
        protected GameObject parent;

        public void SetParent(GameObject parent)
        {
            this.parent = parent;
            this.transform.SetParent(parent.transform, true);
        }

        public bool Active
        {
            get
            {
                return active;
            }

            set
            {
                active = value;
            }
        }

        public virtual void SetActive(bool active)
        {
            if (!active)
                Clear();

            this.active = active;
        }

        public abstract void Clear();

        public abstract void Redraw(params object[] args);
    }
}
