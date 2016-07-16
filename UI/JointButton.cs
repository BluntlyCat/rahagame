namespace HSA.RehaGame.UI
{
    using System.Collections.Generic;
    using DB.Models;
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    public class JointButton : MonoBehaviour
    {
        public GameManager gameManager;

        private Button button;
        private PatientJoint joint;
        private ColorBlock buttonColors;

        private enum States { inactive, active, calibrating, specialInactive, specialActive }

        private Dictionary<States, Color> colors = new Dictionary<States, Color>() {
            { States.inactive, new Color(255, 0, 0, 255) },
            { States.specialInactive, new Color(255, 255, 0, 255) },
            { States.active, new Color(0, 196, 255, 255) },
            { States.specialActive, new Color(255, 196, 255, 255) },
            { States.calibrating, new Color(255, 255, 0, 255) }
        };

        void Start()
        {
            this.button = GetComponent<Button>();
            this.buttonColors = this.button.colors;
        }

        void OnEnable()
        {
            if (GameManager.ActivePatient != null)
            {
                this.button = GetComponent<Button>();
                this.buttonColors = this.button.colors;

                this.joint = GameManager.ActivePatient.GetJointByName(this.name);
                States state = joint.Active ? States.active : States.inactive;
                this.buttonColors.normalColor = colors[state];
                this.buttonColors.pressedColor = state == States.active ? colors[States.specialActive] : colors[States.specialInactive];
                this.buttonColors.highlightedColor = state == States.active ? colors[States.specialActive] : colors[States.specialInactive];
                this.buttonColors.disabledColor = state == States.active ? colors[States.specialActive] : colors[States.specialInactive];
                this.button.colors = this.buttonColors;
            }
        }

        public void Activate()
        {
            if (joint != null)
            {
                bool active = !joint.Active;

                joint.Active = active;
                SetActive(this.transform, active);
            }
        }

        private void SetActive(Transform root, bool active)
        {
            SetColor(root.GetComponent<Button>(), active);
            root.gameObject.SetActive(active);

            foreach (Transform child in root)
            {
                if (child.childCount > 0)
                    SetActive(child, active);

                SetColor(child.GetComponent<Button>(), active);
                child.gameObject.SetActive(active);
            }
        }

        private void SetColor(Button button, bool active)
        {
            var buttonColors = button.colors;
            States state = active ? States.active : States.inactive;

            buttonColors.normalColor = colors[state];
            buttonColors.pressedColor = state == States.active ? colors[States.specialActive] : colors[States.specialInactive];
            buttonColors.highlightedColor = state == States.active ? colors[States.specialActive] : colors[States.specialInactive];
            buttonColors.disabledColor = state == States.active ? colors[States.specialActive] : colors[States.specialInactive];
            button.colors = buttonColors;
        }
    }
}