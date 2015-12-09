using UnityEngine;

public class Development : MonoBehaviour
{
    private static bool DEVEL = false;
    private static GameObject[] developmentObjects;

    // Use this for initialization
    void Start()
    {
        developmentObjects = GameObject.FindGameObjectsWithTag("development");
        Hide();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetActive(bool active)
    {
        DEVEL = active;

        if (DEVEL)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public static void Reset()
    {
        DEVEL = false;
        Hide();
    }

    public static void Hide()
    {
        foreach (var o in developmentObjects)
        {
            o.SetActive(false);
        }
    }

    public static void Show()
    {
        foreach (var o in developmentObjects)
        {
            o.SetActive(true);
        }
    }

    public static bool IsDevelMode
    {
        get
        {
            return DEVEL;
        }
    }
}
