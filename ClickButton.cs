namespace HSA.RehaGame.UI
{
    using Scenes;
    using UnityEngine;

    public class ClickButton : MonoBehaviour
    {
        public void LoadSceneByButtonName()
        {
            LoadScene.LoadExercise(this.name);
        }
    }
}