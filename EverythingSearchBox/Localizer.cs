using QTPlugin;

namespace QuizoPlugins
{
    sealed class Localizer : LocalizedStringProvider
    {
        public override string Author
        {
            get { return "HamzaETTH"; }
        }

        public override string Description
        {
            get { return "Adds a search box to the toolbar to search using Everything."; }
        }

        public override string Name
        {
            get { return "Search Box Plugin"; }
        }

        public override void SetKey(int iKey)
        {
            // Not used in this plugin.
        }
    }
}
