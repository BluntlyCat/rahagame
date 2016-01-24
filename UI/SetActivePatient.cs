namespace HSA.RehaGame.UI
{
    using User;
    using Scene;
    using Settings;
    using UnityEngine;

    public class SetActivePatient : MonoBehaviour
    {
        public void SetPatient()
        {
            RGSettings.ActivePatient = Patient.Instance(this.name);
            LoadScene.LoadUser();
        }
    }
}