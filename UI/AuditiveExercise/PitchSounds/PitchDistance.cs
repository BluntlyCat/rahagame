namespace HSA.RehaGame.UI.AuditiveExercise.PitchSounds
{
    using HSA.RehaGame.Manager.Audio;
    using Math;
    using UnityEngine;

    public class PitchDistance : Pitchable
    {
        public override void Replay(params object[] args)
        {
            Vector3 maxDistance = (Vector3)args[0];
            Vector3 distance = (Vector3)args[1];

            double maxDistanceLength = Calculations.CalculateVectorLength(maxDistance);
            double distanceLength = Calculations.CalculateVectorLength(distance);
            
            double pitchVal = (3 / maxDistanceLength) * distanceLength;

            base.Pitch((float)pitchVal);
        }
    }
}
