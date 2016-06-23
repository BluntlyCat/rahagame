namespace HSA.RehaGame.UI.Menu
{
    using DB.Models;
    using Manager.Audio;
    using UnityEngine;
    using UnityEngine.UI;

    public class MainMenu : MonoBehaviour
    {
        public GameObject gameManager;

        public GameObject menuTitleObject;
        public GameObject[] menuEntries;

        private SoundManager soundManager;
        private MusicManager musicManager;

        private Menu menu;


        void Start()
        {
            menu = Model.GetModel<Menu>(this.name);

            soundManager = gameManager.GetComponentInChildren<SoundManager>();
            musicManager = gameManager.GetComponentInChildren<MusicManager>();

            menuTitleObject.GetComponent<Text>().text = menu.Name;

            foreach(var menuEntry in this.menuEntries)
            {
                menuEntry.GetComponent<Text>().text = menu.Entries[menuEntry.name].Entry;
            }

            soundManager.Enqueue(menu.AuditiveName);
            musicManager.AddMusic(menu.Music);
        }

        public void ReadMenu(GameObject menuEntry)
        {
            soundManager.Enqueue(menu.Entries[menuEntry.name].AuditiveEntry, true);   
        }
    }
}
