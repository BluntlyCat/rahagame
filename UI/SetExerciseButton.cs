namespace HSA.RehaGame.UI
{
    using DB.Models;
    using Manager;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]

    public class SetExerciseButton : MonoBehaviour
    {
        public GameObject sceneManager;
        public GameObject dbManager;
        private AudioSource audioSource;

        void Start()
        {
            var exercise = Model.GetModel<Exercise>(this.name);

            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = exercise.AuditiveName;
        }

        public void LoadExrcise()
        {
            // ToDO GameManager.SetActiveExercise(this.name);
            sceneManager.GetComponent<SceneManager>().LoadExercise();
        }
    }
}