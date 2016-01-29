namespace HSA.RehaGame.UI
{
    using DB;
    using InGame;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]

    public class SetPatientName : MonoBehaviour
    {
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            if (GameState.ActivePatient != null)
            {
                Text textComponent = this.GetComponent<Text>();
                textComponent.text = GameState.ActivePatient.Name;
            }
        }
    }
}