namespace HSA.RehaGame.UI
{
    using UnityEngine;
    using Scene;

    public class OverlayMenu : MonoBehaviour
    {
        private static GameObject overlayMenu;

        public void Start()
        {
            overlayMenu = GameObject.FindGameObjectWithTag("OverlayMenu");
            Time.timeScale = 0;
        }

        public static void HideMenu()
        {
            overlayMenu.SetActive(Pause.SetPause());
        }

        public void buttonClickAction()
        {
            HideMenu();
        }
    }
}