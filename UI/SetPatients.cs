namespace HSA.RehaGame.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using InGame;
    using Scene;
    using DB;
    using UnityEngine.EventSystems;

    public class SetPatients : MonoBehaviour
    {
        public Transform patientButton;

        // Use this for initialization
        void Start()
        {
            var patients = DBManager.Query("editor_patient", "SELECT name FROM editor_patient ORDER BY name");

            if (patients.Rows.Count > 0)
            {
                bool isFirst = true;

                foreach (var patient in patients.Rows)
                {
                    var button = Instantiate(patientButton) as Transform;
                    var text = button.GetComponentInChildren<Text>();
                    var name = patient.GetValue("name");

                    button.SetParent(this.transform, false);
                    button.name = name;
                    text.text = name;

                    if (isFirst)
                    {
                        var eventSystem = GameObject.Find("EventSystem");
                        eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = button.gameObject;
                        isFirst = false;
                    }
                }
            }
            else
            {
                LoadScene.LoadNewUser();
            }

            GameState.ActivePatient = null;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}