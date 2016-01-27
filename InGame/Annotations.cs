[System.AttributeUsage(System.AttributeTargets.Property)]
public class Setting : System.Attribute
{
    private bool save;

    public Setting(bool save)
    {
        this.save = save;
    }
}