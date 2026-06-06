namespace QuizoPlugins
{
    using System;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource
    {
        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        internal Resource()
        {
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    var temp = new global::System.Resources.ResourceManager("QuizoPlugins.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }

                return resourceMan;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get { return resourceCulture; }
            set { resourceCulture = value; }
        }

        internal static System.Drawing.Bitmap SearchBoxPlugin_large
        {
            get
            {
                object obj = ResourceManager.GetObject("SearchBoxPlugin_large", resourceCulture);
                return (System.Drawing.Bitmap)obj;
            }
        }

        internal static System.Drawing.Bitmap SearchBoxPlugin_small
        {
            get
            {
                object obj = ResourceManager.GetObject("SearchBoxPlugin_small", resourceCulture);
                return (System.Drawing.Bitmap)obj;
            }
        }
    }
}
