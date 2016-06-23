namespace HSA.RehaGame.UI.AuditiveExercise.PitchSounds
{
    using HSA.RehaGame.Manager.Audio;

    public class PitchDepth : Pitchable
    {
        public override void Replay(params object[] args)
        {
            double maxDistance = (double)args[0];
            double activeJointY = (double)args[1];
            double passiveJointY = (double)args[2];

            double jointDistance = activeJointY - passiveJointY;

            double pitchVal = (3 / maxDistance) * jointDistance;

            base.Pitch((float)pitchVal);
        }
    }
}
