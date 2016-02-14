namespace HSA.RehaGame.UI
{
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    public class SetPatientAge : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            var input = this.GetComponent<Text>();
            input.text = GameManager.ActivePatient == null ? "1" : GameManager.ActivePatient.Age.ToString();
        }
    }
}