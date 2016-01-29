namespace HSA.RehaGame.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ChangeIntValue : MonoBehaviour
    {
        private InputField input;
        private int value;

        void Start()
        {
            input = this.GetComponent<InputField>();
            input.text = "1";
            ValueChanged();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (value < 99)
                    value = ++value;
                else
                    value = 1;

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