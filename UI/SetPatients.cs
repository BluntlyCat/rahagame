namespace HSA.RehaGame.UI
{
    using DB.Models;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class SetPatients : MonoBehaviour
    {
        public GameObject dbManager;
        public Transform patientButton;

        // Use this for initialization
        void Start()
        {
            var patients = Model.All<Patient>();

            if (patients.Count > 0)
            {
                bool isFirst = true;

                foreach (var patient in patients.Values)
                {
                    var button = Instantiate(patientButton) as Transform;
                    var text = button.GetComponentInChildren<Text>();
                    var name = patient.Name;

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
        }
    }
}