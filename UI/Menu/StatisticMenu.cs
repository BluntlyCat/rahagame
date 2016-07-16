namespace HSA.RehaGame.UI.Menu
{
    using DB.Models;
    using Manager.Audio;
    using UnityEngine;
    using UnityEngine.UI;

    public class StatisticMenu : MonoBehaviour
    {
        public GameObject gameManager;

        public GameObject menuTitleObject;

        private SoundManager soundManager;
        private MusicManager musicManager;

        private Menu menu;


        void Start()
        {
            menu = Model.GetModel<Menu>(this.name);

            soundManager = gameManager.GetComponentInChildren<SoundManager>();
            musicManager = gameManager.GetComponentInChildren<MusicManager>();

            menuTitleObject.GetComponent<Text>().text = menu.Name;

            soundManager.Enqueue(menu.AuditiveName);
            musicManager.AddMusic(menu.Music);
        }
    }
}
