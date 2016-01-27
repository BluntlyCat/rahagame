namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using InGame;
    using Scene;

    public class GetPatients : MonoBehaviour
    {
        public Transform patientButton;

        // Use this for initialization
        void Start()
        {
            var patients = DBManager.Query("SELECT name from editor_patient");

            if (patients.Count > 0)
            {
                foreach (var patient in patients)
                {
                    var button = Instantiate(patientButton) as Transform;
                    var text = button.GetComponentInChildren<Text>();
                    var name = patient["name"].ToString();

                    button.SetParent(this.transform, false);
                    button.name = name;
                    text.text = name;
                }
            }
            else
                LoadScene.LoadNewUser();

            GameState.PatientName = null;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}