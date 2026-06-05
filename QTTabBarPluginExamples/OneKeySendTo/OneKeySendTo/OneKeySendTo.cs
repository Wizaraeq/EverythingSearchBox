using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using QTPlugin;
using QTPlugin.Interop;

namespace QuizoPlugins
{
	[Plugin( PluginType.Background, typeof( Localizer ), Version = "1.3.0.0" )]
	public class OneKeySendTo : IPluginClient
	{
		private IPluginServer pluginServer;

		const string PLUGIN_NAME = "OneKeySendTo";
		const string REGKEY_RESERVED = "Reserved";

		private static List<string> lstPathTargets = new List<string>();
		private static List<string> lstReservedPaths = new List<string>();

		#region IPluginClient members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.pluginServer.ShortcutKeyPressed += new PluginKeyEventHandler( pluginServer_ShortcutKeyPressed );

			ReadSettings();
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			if( lstPathTargets.Count == 0 )
			{
				actions = new string[] { "Option" };
			}
			else
			{
				string[] strs = new Localizer().Strs;

				List<string> lstActions = new List<string>();
				foreach( string path in lstPathTargets )
				{
					lstActions.Add( String.Format( strs[7], path ) );
					lstActions.Add( String.Format( strs[8], path ) );
				}
				lstActions.Add( strs[9] );

				actions = lstActions.ToArray();
			}
			return true;
		}

		public void Close( EndCode endCode )
		{
			if( endCode == EndCode.Removed )
			{
				DeleteSettings();
			}
		}

		public bool HasOption
		{
			get
			{
				return true;
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public void OnOption()
		{
			this.ShowOptionDialog( false );
		}

		private void ShowOptionDialog( bool fByKey )
		{
			using( OptionForm of = new OptionForm( lstPathTargets, lstReservedPaths ) )
			{
				if( DialogResult.OK == of.ShowDialog() )
				{
					lstPathTargets = of.Paths;
					lstReservedPaths = of.ReservedPaths;
					SaveSettings();
				}
			}
		}

		public void OnShortcutKeyPressed( int index )
		{
		}

		#endregion

		#region Settings 

		private static void ReadSettings()
		{
			if( lstPathTargets.Count == 0 )
			{
				using( RegistryKey rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\" + PLUGIN_NAME ) )
				{
					if( rkPlugin != null )
					{
						foreach( string valName in rkPlugin.GetValueNames() )
						{
							string path = rkPlugin.GetValue( valName, null ) as string;
							if( !String.IsNullOrEmpty( path ) )
							{
								lstPathTargets.Add( path );
							}
						}

						using( RegistryKey rkPlugin_Rsvd = rkPlugin.OpenSubKey( REGKEY_RESERVED ) )
						{
							if( rkPlugin_Rsvd != null )
							{
								foreach( string valName in rkPlugin_Rsvd.GetValueNames() )
								{
									string path = rkPlugin_Rsvd.GetValue( valName, null ) as string;
									if( !String.IsNullOrEmpty( path ) )
									{
										lstReservedPaths.Add( path );
									}
								}
							}
						}
					}
				}
			}
		}

		private static void SaveSettings()
		{
			using( RegistryKey rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\" + PLUGIN_NAME ) )
			{
				if( rkPlugin != null )
				{
					foreach( string val in rkPlugin.GetValueNames() )
					{
						rkPlugin.DeleteValue( val, false );						
					}

					int i = 0;
					foreach( string path in lstPathTargets )
					{
						if( !String.IsNullOrEmpty( path ) )
						{
							rkPlugin.SetValue( ( i++ ).ToString(), path );
						}
					}

					rkPlugin.DeleteSubKeyTree( REGKEY_RESERVED, false );
					if( lstReservedPaths.Count > 0 )
					{
						using( RegistryKey rkPlugin_Rsvd = rkPlugin.CreateSubKey( REGKEY_RESERVED ) )
						{
							if( rkPlugin_Rsvd != null )
							{
								int j = 0;
								foreach( string path in lstReservedPaths )
								{
									if( !String.IsNullOrEmpty( path ) )
									{
										rkPlugin_Rsvd.SetValue( ( j++ ).ToString(), path );
									}
								}
							}
						}
					}
				}
			}
		}

		private static void DeleteSettings()
		{
		}

		#endregion

		private void pluginServer_ShortcutKeyPressed( object sender, PluginKeyEventArgs e )
		{
			if( !e.Repeat )
			{
				if( e.Index < lstPathTargets.Count * 2 )
				{
					this.SendTo( e.Index );
				}
				else
				{
					this.ShowOptionDialog( true );
				}
			}
		}
		
		private void SendTo( int index )
		{
			// weird code!!
			bool fMove = ( index % 2 ) == 1;
			string strPathTarget = lstPathTargets[index / 2];

			if( String.IsNullOrEmpty( strPathTarget ) )
			{
				System.Media.SystemSounds.Hand.Play();
				return;
			}

			var tab = this.pluginServer.SelectedTabInFocusedView;
			if( tab != null )
			{
				this.pluginServer.InvokeCommand( fMove ? QCommand.MoveFile : QCommand.CopyFile, tab.SelectedPaths.ToArray(), strPathTarget );
			}
		}
	}

	sealed class Localizer : LocalizedStringProvider
	{
		string[] strs;

		public Localizer()
		{
			if( System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				strs = Resource.ja.Split( new char[] { ';' } );
			}
			else
			{
				strs = Resource.en.Split( new char[] { ';' } );
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
				return this.strs[1];
			}
		}

		public override string Name
		{
			get
			{
				return this.strs[0];
			}
		}

		public override void SetKey( int iKey )
		{
		}

		public string[] Strs
		{
			get
			{
				return this.strs;
			}
		}
	}
}
