namespace HSA.RehaGame.UI
{
    using User;
    using Scene;
    using InGame;
    using UnityEngine;
    using DB;

    public class SetActivePatient : MonoBehaviour
    {
        public void SetPatient()
        {
            if (DBManager.Exists("editor_patient", this.name))
            {
                GameState.PatientName = this.name;
                GameState.ActivePatient = Patient.Instance(this.name);
                LoadScene.LoadUser();
            }
        }
    }
}