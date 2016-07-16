namespace HSA.RehaGame.UI
{
    using Input;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(InputField))]
    public class ChangeIntValue : MonoBehaviour
    {
        public KeyboardEventHandler keyboardEventHandler;
        public int maxValue;
        
        private InputField input;
        private int value;
        private bool selected = false;

        void Start()
        {
            input = this.GetComponent<InputField>();
            input.characterLimit = GetCharacterLimit();
            keyboardEventHandler.KeyPressed += KeyboardEventHandler_KeyPressed;
            ValueChanged();
        }

        private int GetCharacterLimit()
        {
            int pos = 1;
            float tmp = maxValue;

            while ((tmp /= 10) >= 1)
                pos++;

            return pos;
        }

        private void KeyboardEventHandler_KeyPressed(KeyCode keyCode)
        {
            if (selected)
            {
                switch(keyCode)
                {
                    case KeyCode.UpArrow:
                        if (value < maxValue)
                            value = ++value;
                        else
                            value = 1;

                        SetValue();

                        break;

                    case KeyCode.DownArrow:
                        if (value > 1)
                            value = --value;
                        else
                            value = maxValue;

                        SetValue();

                        break;

                    case KeyCode.LeftArrow:
                        if (value - 10 >= 1)
                            value -= 10;
                        else
                            value = (maxValue - 10) + value;

                        SetValue();

                        break;

                    case KeyCode.RightArrow:
                        if (value + 10 <= maxValue)
                            value += 10;
                        else
                            value = value - (maxValue - 10);

                        SetValue();

                        break;
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
                if(input == null)
                    input = this.GetComponent<InputField>();

                int tmp = int.Parse(input.text);

                if (tmp > maxValue)
                {
                    value = maxValue;
                    SetValue();
                }
                else
                    value = tmp;
            }
            catch
            {
                value = 1;
            }
        }
    }
}