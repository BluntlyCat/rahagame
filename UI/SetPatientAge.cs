namespace HSA.RehaGame.UI
{
    using InGame;
    using UnityEngine;
    using UnityEngine.UI;

    public class SetPatientAge : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            var input = this.GetComponent<Text>();
            input.text = GameState.ActivePatient == null ? "1" : GameState.ActivePatient.Age.ToString();
        }
    }
}