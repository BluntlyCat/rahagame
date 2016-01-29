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
                Patient patient = new Patient(this.name);
                GameState.ActivePatient = patient.Select() as Patient;
                LoadScene.MainMenu();
            }
        }
    }
}