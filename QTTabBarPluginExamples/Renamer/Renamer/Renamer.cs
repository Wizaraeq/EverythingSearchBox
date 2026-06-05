using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QTPlugin;
using QTPlugin.Interop;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace QuizoPlugins
{
	[Plugin( PluginType.Interactive, typeof( Localizer ) )]
	public class Renamer : IBarButton
	{
		private const int COUNT_OF_STRING_RESOURCES = 5;

		private IPluginServer pluginServer;
		private IShellBrowser shellBrowser;
		private Localizer localizer = new Localizer();


		public void Open( IPluginServer pluginServer, QTPlugin.Interop.IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.shellBrowser = shellBrowser;
			this.pluginServer.ShortcutKeyPressed += new PluginKeyEventHandler( pluginServer_ShortcutKeyPressed );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = localizer.Shortcuts;
			return true;
		}

		public void Close( EndCode endCode )
		{
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public void OnOption()
		{
		}

		public void OnShortcutKeyPressed( int index )
		{
		}




		public System.Drawing.Image GetImage( bool fLarge )
		{
			return null;
		}

		public void OnButtonClick()
		{
			this.RenameSelectionToClipboardString();
		}

		public void InitializeItem()
		{
		}

		public bool ShowTextLabel
		{
			get
			{
				return false;
			}
		}

		public string Text
		{
			get
			{
				return this.localizer.Name;
			}
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		private void pluginServer_ShortcutKeyPressed( object sender, PluginKeyEventArgs e )
		{
			this.RenameSelectionToClipboardString();
		}

		private void RenameSelectionToClipboardString()
		{
			try
			{
				string str = SanitizeNameString( Clipboard.GetText() );
				if( !String.IsNullOrEmpty( str ) )
				{
					Address[] adrs;
					if( this.pluginServer.TryGetSelection( out adrs ) && adrs.Length == 1 && !String.IsNullOrEmpty( adrs[0].Path ) )
					{
						string path = adrs[0].Path;
						string pathTarget;

						FileInfo fi = new FileInfo( path );
						if( fi.Exists )
						{
							int index = str.LastIndexOf( "." );
							if( index == -1 || index == str.Length - 1 )
							{
								// no extension in clipboard, append source extension
								str += Path.GetExtension( path );
							}

							pathTarget = Path.GetDirectoryName( path ) + "\\" + str;

							if( !File.Exists( pathTarget ) )
							{
								fi.MoveTo( pathTarget );
								return;
							}
						}
						else
						{
							DirectoryInfo di = new DirectoryInfo( path );
							if( di.Exists )
							{
								pathTarget = Path.GetDirectoryName( path ) + "\\" + str;
								if( !Directory.Exists( pathTarget ) )
								{
									di.MoveTo( pathTarget );
									return;
								}
							}
						}

					}
				}
			}
			catch
			{
			}

			System.Media.SystemSounds.Beep.Play();
		}

		private static string SanitizeNameString( string name )
		{
			// eliminate invalid name char from file name

			if( name == null )
			{
				return null;
			}

			name = name.Trim();

			StringBuilder sb = new StringBuilder( name.Length );
			for( int i = 0; i < name.Length; i++ )
			{
				if( IsValidFileNameChar( name[i] ) && name[i] > 0x1f )
				{
					// ", <, >, |, \, /, :, *, ? 

					sb.Append( name[i] );
				}
			}
			return sb.ToString();
		}

		private static bool IsValidFileNameChar( char ch )
		{
			// ", <, >, |, \, /, :, *, ? 

			return ( ch != '"' ) && ( ch != '<' ) && ( ch != '>' ) && ( ch != '|' ) && ( ch != '\\' ) && ( ch != '/' ) && ( ch != ':' ) && ( ch != '*' ) && ( ch != '?' ) && ( ch > 0x1f );
		}


	}

	sealed class Localizer : LocalizedStringProvider2
	{
		string[] res;

		public Localizer()
		{
			if( CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				res = Resource.ja.Split( new char[] { ';' } );
			}
			else
			{
				res = Resource.en.Split( new char[] { ';' } );
			}
		}

		public override DateTime LastUpdate
		{
			get
			{
				return new DateTime( 2012, 4, 1 );
			}
		}

		public override string SupportURL
		{
			get
			{
				return String.Empty;
			}
		}

		public override string Author
		{
			get
			{
				return "Quizo";
			}
		}

		public override string Description
		{
			get
			{
				return res[0];
			}
		}

		public override string Name
		{
			get
			{
				return "Renamer";
			}
		}

		public override void SetKey( int iKey )
		{
			
		}

		public string[] Shortcuts
		{
			get
			{
				return res;
			}
		}
	}
}
