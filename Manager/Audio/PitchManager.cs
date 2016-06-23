namespace HSA.RehaGame.Manager.Audio
{
    using DB.Models;
    using Logging;
    using System.Collections.Generic;
    using UI.AuditiveExercise;
    using UI.AuditiveExercise.PitchSounds;
    using UnityEngine;

    public class PitchManager : MonoBehaviour
    {
        private static Logger<PitchManager> logger = new Logger<PitchManager>();

        private Dictionary<PitchType, IPitchable> pitchSounds;

        private SettingsManager settingsManager;
        private SettingsKeyValue readingOn;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.settingsManager = this.GetComponentInParent<SettingsManager>();

            this.pitchSounds = new Dictionary<PitchType, IPitchable>()
            {
                { PitchType.pitchFullfilled, this.transform.FindChild("PitchFullfilled").GetComponent<PitchFullfilled>() },
                { PitchType.pitchAngleValue, this.transform.FindChild("PitchAngleValue").GetComponent<PitchAngleFeedback>() },
                { PitchType.pitchDepth, this.transform.FindChild("PitchDepth").GetComponent<PitchDepth>() },
                { PitchType.pitchHeight, this.transform.FindChild("PitchHeight").GetComponent<PitchHeight>() },
                { PitchType.pitchDistance, this.transform.FindChild("PitchDistance").GetComponent<PitchDistance>() },
            };

            this.readingOn = settingsManager.GetKeyValue("ingame", "reading");
        }

        public void Pitch(PitchType type, params object[] args)
        {
            if (this.pitchSounds.ContainsKey(type))
                this.pitchSounds[type].Replay(args);
        }
    }
}
