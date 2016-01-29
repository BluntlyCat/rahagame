namespace HSA.RehaGame.User
{
    using UnityEngine;
    using System.Collections;
    using InGame;
    using System.Collections.Generic;
    using UnityEngine.UI;

    public class PatientJointManager : MonoBehaviour
    {
        private Button button;
        private PatientJoint joint;
        private ColorBlock buttonColors;

        private enum states { inactive, active, calibrating, specialInactive, specialActive }

        private Dictionary<states, Color> colors = new Dictionary<states, Color>() {
            { states.inactive, new Color(255, 0, 0, 255) },
            { states.specialInactive, new Color(255, 255, 0, 255) },
            { states.active, new Color(0, 196, 255, 255) },
            { states.specialActive, new Color(255, 196, 255, 255) },
            { states.calibrating, new Color(255, 255, 0, 255) }
        };

        void Start()
        {
            this.button = GetComponent<Button>();
            this.buttonColors = this.button.colors;
        }

        void OnEnable()
        {
            if (GameState.ActivePatient != null)
            {
                this.button = GetComponent<Button>();
                this.buttonColors = this.button.colors;

                this.joint = GameState.ActivePatient.GetJoint(this.name);
                states state = joint.Active ? states.active : states.inactive;
                this.buttonColors.normalColor = colors[state];
                this.buttonColors.pressedColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                this.buttonColors.highlightedColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                this.buttonColors.disabledColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                this.button.colors = this.buttonColors;
            }
        }

        public void Activate()
        {
            if (joint != null)
            {
                bool active = !joint.Active;
                var children = joint.Activate(active);
                var list = GameObject.Find("jointList");

                states state = active ? states.active : states.inactive;

                foreach (var child in children)
                {
                    foreach(Transform transform in list.transform)
                    {
                        if (transform.name == child.JointType.ToString())
                        {
                            var childButton = transform.GetComponent<Button>();
                            var childColors = childButton.colors;

                            childColors.normalColor = colors[state];
                            childColors.pressedColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                            childColors.highlightedColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                            childColors.disabledColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                            childButton.colors = childColors;
                        }
                    }
                }

                this.buttonColors.normalColor = colors[state];
                this.buttonColors.pressedColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                this.buttonColors.highlightedColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                this.buttonColors.disabledColor = state == states.active ? colors[states.specialActive] : colors[states.specialInactive];
                this.button.colors = this.buttonColors;
            }
        }
    }
}