using QTPlugin;

sealed class Localizer : LocalizedStringProvider
{
    public override string Author
    {
        get
        {
            return "UnderPL";
        }
    }

    public override string Description
    {
        get
        {
            return "Adds a search box to the toolbar to search using Everything.";
        }
    }

    public override string Name
    {
        get
        {
            return "Search Box Plugin";
        }
    }

    public override void SetKey(int iKey)
    {
        // No action needed since we're not using multiple keys
    }
}
