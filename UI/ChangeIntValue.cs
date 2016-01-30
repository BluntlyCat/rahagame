namespace HSA.RehaGame.UI
{
    using InGame;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChangeIntValue : MonoBehaviour
    {
        private InputField input;
        private int value;
        private bool selected = false;

        void Start()
        {
            input = this.GetComponent<InputField>();
            input.text = GameState.ActivePatient == null ? "1" : GameState.ActivePatient.Age.ToString();
            ValueChanged();
        }

        void Update()
        {
            if (selected)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (value < 99)
                        value = ++value;
                    else
                        value = 1;

                    SetValue();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (value + 10 <= 99)
                        value += 10;
                    else
                        value = value - 90
                            ;

                    SetValue();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (value > 1)
                        value = --value;
                    else
                        value = 99;

                    SetValue();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (value - 10 >= 1)
                        value -= 10;
                    else
                        value = 90 + value;

                    SetValue();
                }
            }
        }

        public void Select()
        {
            selected = true;
        }

        public void DeSelect()
        {
            selected = false;
        }

        private void SetValue()
        {
            input.text = value.ToString();
        }

        public void ValueChanged()
        {
            try
            {
                value = int.Parse(input.text);
            }
            catch
            {
                value = 1;
            }
        }
    }
}