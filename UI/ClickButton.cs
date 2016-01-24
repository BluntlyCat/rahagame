namespace HSA.RehaGame.UI
{
    using Scene;
    using UnityEngine;

    public class ClickButton : MonoBehaviour
    {
        public void LoadSceneByButtonName()
        {
            LoadScene.LoadExercise(this.name);
        }
    }
}