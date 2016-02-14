namespace HSA.RehaGame.UI
{
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]

    public class SetPatientName : MonoBehaviour
    {
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            if (GameManager.ActivePatient != null)
            {
                Text textComponent = this.GetComponent<Text>();
                textComponent.text = GameManager.ActivePatient.Name;
            }
        }
    }
}