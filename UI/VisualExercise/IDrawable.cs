namespace HSA.RehaGame.UI.VisualExercise
{
    public interface IDrawable
    {
        void Redraw(params object[] args);

        void Clear();

        void SetActive(bool active);
    }
}
