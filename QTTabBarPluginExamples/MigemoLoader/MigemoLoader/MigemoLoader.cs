using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using QTPlugin;
using QTPlugin.Interop;

namespace QuizoPlugins
{
	[Plugin( PluginType.Background, Author = "Quizo", Name = "Migemo ローダー", Version = "1.0.0.3", Description = "Migemo dll のローダーです。\r\nフィルターボックスでMigemoの機能を使えるようになります。\r\nオプションからC/Migemo dllと辞書ファイルを指定してください。" )]
	public class MigemoLoader : IFilter
	{
		private IPluginServer pluginServer;
		private MigemoWrapper migemoWrapper;

		private static bool fPartialMatch = true, fDicIsUTF8;
		private static string pathDLL, pathDic;


		#region IFilter member

		public bool QueryRegex( string strQuery, out Regex re )
		{
			re = null;

			if( String.IsNullOrEmpty( strQuery ) )
			{
				return false;
			}

			if( this.migemoWrapper == null )
			{
				if( String.IsNullOrEmpty( MigemoLoader.pathDLL ) || String.IsNullOrEmpty( MigemoLoader.pathDic ) )
				{
					return false;
				}

				try
				{
					this.migemoWrapper = new MigemoWrapper( MigemoLoader.pathDLL, MigemoLoader.pathDic, MigemoLoader.fDicIsUTF8 );
				}
				catch
				{
					return false;
				}
			}

			if( this.migemoWrapper != null && this.migemoWrapper.IsEnable )
			{
				try
				{
					bool fStartWithNoPartial = strQuery.StartsWith( "^" );
					if( fStartWithNoPartial && strQuery.Length > 1 )
					{
						strQuery = strQuery.Substring( 1 );
					}

					string strPrefix = String.Empty;
					if( fStartWithNoPartial || !MigemoLoader.fPartialMatch )
					{
						strPrefix = "^";
					}

					re = new Regex( strPrefix + this.migemoWrapper.QueryRegexStr( strQuery ), RegexOptions.IgnoreCase );
					return true;
				}
				catch
				{
				}
			}
			return false;
		}

		#endregion


		#region IPluginClient member

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			MigemoLoader.ReadSettings();
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
		}

		public void Close( EndCode endCode )
		{
			if( this.migemoWrapper != null )
			{
				this.migemoWrapper.Dispose();
				this.migemoWrapper = null;
			}
		}

		public bool HasOption
		{
			get
			{
				return true;
			}
		}

		public void OnOption()
		{
			this.pluginServer.ExecuteCommand( Commands.SetModalState, true );
			try
			{
				using( MigemoOptionForm mof = new MigemoOptionForm( MigemoLoader.pathDLL, MigemoLoader.pathDic, MigemoLoader.fPartialMatch, MigemoLoader.fDicIsUTF8 ) )
				{
					if( DialogResult.OK == mof.ShowDialog() )
					{
						MigemoLoader.pathDLL = mof.pathDLL;
						MigemoLoader.pathDic = mof.pathDic;
						MigemoLoader.fPartialMatch = mof.PartialMatch;
						MigemoLoader.fDicIsUTF8 = mof.UTF8Dictionary;

						if( this.migemoWrapper != null )
						{
							this.migemoWrapper.Dispose();
							this.migemoWrapper = null;
						}

						using( RegistryKey rkMigemo = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + "\\MigemoLoader" ) )
						{
							if( rkMigemo != null )
							{
								rkMigemo.SetValue( "dll", MigemoLoader.pathDLL );
								rkMigemo.SetValue( "dic", MigemoLoader.pathDic );
								rkMigemo.SetValue( "PartialMatch", MigemoLoader.fPartialMatch ? 1 : 0 );
								rkMigemo.SetValue( "UTF-8", MigemoLoader.fDicIsUTF8 ? 1 : 0 );
							}
						}
					}
				}
			}
			finally
			{
				this.pluginServer.ExecuteCommand( Commands.SetModalState, false );
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public void OnShortcutKeyPressed( int index )
		{
		}

		#endregion


		public static void Uninstall()
		{
			using( RegistryKey rkPluginSetting = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS, true ) )
			{
				if( rkPluginSetting != null )
				{
					rkPluginSetting.DeleteSubKey( "MigemoLoader", false );
				}
			}
		}

		private static bool ReadSettings()
		{
			using( RegistryKey rkMigemo = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + "\\MigemoLoader", false ) )
			{
				if( rkMigemo != null )
				{
					try
					{
						string pathDLL = (string)rkMigemo.GetValue( "dll" );
						string pathDic = (string)rkMigemo.GetValue( "dic" );
						MigemoLoader.fPartialMatch = (int)rkMigemo.GetValue( "PartialMatch", 0 ) == 1;
						MigemoLoader.fDicIsUTF8 = (int)rkMigemo.GetValue( "UTF-8", 0 ) == 1;

						MigemoLoader.pathDLL = pathDLL;
						MigemoLoader.pathDic = pathDic;
					}
					catch
					{
						MessageBox.Show( "Invalid registry values.", "Migemo loader plugin" );
					}
				}
			}
			return false;
		}
	}
}
