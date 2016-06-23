namespace HSA.RehaGame.UI.AuditiveExercise.PitchSounds
{
    using Manager.Audio;
    using System;

    public class PitchAngleFeedback : Pitchable
    {
        private const double a = -726d / -11664000d;
        private const double b = 59d / 10800d;

        public override void Replay(params object[] args)
        {
            double initialAngle = (double)args[0];
            double currentAngle = (double)args[1];

            double move = initialAngle <= 180 ? -1 : 1;
            double y = a * Math.Pow(currentAngle - move * initialAngle, 2) + b * currentAngle;

            Pitch((float)y);
        }
    }
}
