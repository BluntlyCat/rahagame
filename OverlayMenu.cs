using UnityEngine;
using System.Collections;

public class OverlayMenu : MonoBehaviour
{
    public static bool PAUSE = true;
    private static GameObject overlayMenu;

    public void Start()
    {
        overlayMenu = GameObject.FindGameObjectWithTag("OverlayMenu");
        Time.timeScale = 0;
    }

    public static void HideMenu()
    {
        PAUSE = !PAUSE;
        overlayMenu.SetActive(PAUSE);
        Time.timeScale = PAUSE ? 0 : 1;
    }

    public void buttonClickAction()
    {
        HideMenu();
    }
}
