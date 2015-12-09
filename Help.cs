using UnityEngine;

public class Help : MonoBehaviour
{
    private static bool help = false;

    private static GameObject helpInfo;
    private static GameObject helpText;

    // Use this for initialization
    void Start()
    {
        helpInfo = GameObject.FindGameObjectWithTag("helpInfo");
        helpText = GameObject.FindGameObjectWithTag("helpText");

        helpText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void ShowHelp()
    {
        if (help)
        {
            helpInfo.SetActive(true);
            helpText.SetActive(false);

            if (Development.IsDevelMode)
            {
                Development.Show();
            }
        }
        else
        {
            helpInfo.SetActive(false);
            helpText.SetActive(true);

            if (Development.IsDevelMode)
            {
                Development.Hide();
            }
        }

        help = !help;
    }

    public static void Reset()
    {
        helpInfo.SetActive(true);
        helpText.SetActive(false);
        help = false;
    }

    public static void Show()
    {
        helpInfo.SetActive(false);
        helpText.SetActive(true);
    }

    public static void Hide()
    {
        helpInfo.SetActive(false);
        helpText.SetActive(false);
    }

    public static bool IsHelp
    {
        get
        {
            return help;
        }
    }
}
