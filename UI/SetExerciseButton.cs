namespace HSA.RehaGame.UI
{
    using DB.Models;
    using Manager;
    using Manager.Audio;
    using UnityEngine;

    public class SetExerciseButton : MonoBehaviour
    {
        private GameObject gameManager;
        private SoundManager soundManager;
        private SceneManager sceneManager;
        private Exercise exercise;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            soundManager = gameManager.GetComponentInChildren<SoundManager>();
            sceneManager = gameManager.GetComponent<SceneManager>();

            exercise = Model.GetModel<Exercise>(this.name);
        }

        public void ReadExercise()
        {
            this.soundManager.Enqueue(this.exercise.AuditiveName);
        }

        public void LoadExrcise()
        {
            GameManager.ActiveExercise = exercise;
            sceneManager.GetComponent<SceneManager>().LoadExercise();
        }
    }
}