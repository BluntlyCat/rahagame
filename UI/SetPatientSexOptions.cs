namespace HSA.RehaGame.UI
{
    using System.Collections.Generic;
    using DB;
    using InGame;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]

    public class SetPatientSexOptions : MonoBehaviour
    {
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            Dropdown dropdown = this.GetComponent<Dropdown>();
            List<string> options = new List<string>();

            var table = DBManager.Query("editor_valuetranslation", "SELECT * FROM editor_valuetranslation WHERE unityObjectName = 'male' OR unityObjectName = 'female' ORDER BY unityObjectName desc");

            foreach(var row in table.Rows)
            {
                options.Add(row.GetValueFromLanguage("translation"));
            }

            dropdown.AddOptions(options);

            if (RGSettings.reading)
                audioSource.Play();
        }
    }
}