namespace HSA.RehaGame.UI.VisualExercise
{
    using Logging;
    using System;
    using UnityEngine.UI;

    public class Circle : Drawable
    {
        private static Logger<Circle> logger = new Logger<Circle>();

        private Image image;

        void Awake()
        {
            logger.AddLogAppender<ConsoleAppender>();

            image = GetComponent<Image>();
            Clear();
        }

        

        public override void Redraw(params object[] args)
        {
            if (active)
            {
                float angle = Convert.ToSingle(args[0]);
                float fillAmount = angle / 360;

                this.image.fillAmount = fillAmount;
            }
        }

        public override void Clear()
        {
        }
    }
}
