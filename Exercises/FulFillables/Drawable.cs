namespace HSA.RehaGame.Exercises.FulFillables
{
    using UI.VisualExercise;
    using Windows.Kinect;

    public abstract class Drawable : Informable
    {
        protected Drawing drawing;

        public Drawable(Drawing drawing, FulFillable previous) : base (previous)
        {
            this.drawing = drawing;
        }

        public abstract void Draw(Body body);

        public abstract void Clear();
    }
}
