namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Settings;

    public class GetPatients : MonoBehaviour
    {
        public Transform patientButton;

        // Use this for initialization
        void Start()
        {
            var patients = DBManager.Query("SELECT name from editor_player");

            foreach(var patient in patients)
            {
                var button = Instantiate(patientButton) as Transform;
                var text = button.GetComponentInChildren<Text>();
                var name = patient["name"].ToString();

                button.SetParent(this.transform, false);
                button.name = name;
                text.text = name;
            }

            RGSettings.ActivePatient = null;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}