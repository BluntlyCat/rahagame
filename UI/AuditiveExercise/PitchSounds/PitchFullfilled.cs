namespace HSA.RehaGame.UI.AuditiveExercise.PitchSounds
{
    using HSA.RehaGame.Manager.Audio;

    public class PitchFullfilled : Pitchable
    {
        public override void Replay(params object[] args)
        {
            base.Pitch(2);
        }
    }
}
