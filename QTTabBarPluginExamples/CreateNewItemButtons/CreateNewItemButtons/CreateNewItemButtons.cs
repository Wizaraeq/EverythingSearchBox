using System;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using QTPlugin;
using QTPlugin.Interop;

namespace QuizoPlugin
{
	[Plugin( PluginType.Interactive, typeof( Localizer ), 0, Version = "1.3.0.0" )]
	public class CreateFolderButton : IBarButton
	{
		// 1.3.0.0		supports views

		private IPluginServer pluginServer;

		public void Open( IPluginServer pluginServer, QTPlugin.Interop.IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
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
			IntPtr hicon = ShellAPI.getHIcon( String.Empty, fLarge ? SHIL.EXTRALARGE : SHIL.SMALL, false );
			if( hicon != IntPtr.Zero )
			{
				Bitmap bmp = Icon.FromHandle( hicon ).ToBitmap();
				ShellAPI.DestroyIcon( hicon );
				if( fLarge )
				{
					var bmpTemp = ShellAPI.shrinkBitmap( bmp );
					bmp.Dispose();
					bmp = bmpTemp;
				}
				return bmp;
			}
			return null;
		}

		public void OnButtonClick()
		{
			var view = this.pluginServer.FocusedView;
			if( view != View.None )
			{
				this.pluginServer.InvokeCommand( QCommand.CreateNewFolder, null, false, view );
			}
		}

		public void InitializeItem()
		{
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return Localizer.resources[0];
			}
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		private void Update()
		{
			var fCanCreateFile = false;
			var tab = this.pluginServer.SelectedTabInFocusedView;
			if( tab != null )
			{
				fCanCreateFile = CanCreateFile( tab.Path );
			}
			this.pluginServer.UpdateItem( this, fCanCreateFile, false );
		}

		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}
		
		public static bool CanCreateFile( string path )
		{
			try
			{
				return Directory.Exists( path ) || path.EndsWith( ".library-ms" );
			}
			catch
			{
			}
			return false;
		}
	}

	[Plugin( PluginType.Interactive, typeof( Localizer ), 1, Version = "1.3.0.0" )]
	public class CreateTextButton : IBarButton
	{
		// 1.3.0.0		supports views

		private IPluginServer pluginServer;

		public void Open( IPluginServer pluginServer, QTPlugin.Interop.IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
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
			IntPtr hicon = ShellAPI.getHIcon( "*.txt", fLarge ? SHIL.EXTRALARGE : SHIL.SMALL, true );
			if( hicon != IntPtr.Zero )
			{
				Bitmap bmp = Icon.FromHandle( hicon ).ToBitmap();
				ShellAPI.DestroyIcon( hicon );
				if( fLarge )
				{
					var bmpTemp = ShellAPI.shrinkBitmap( bmp );
					bmp.Dispose();
					bmp = bmpTemp;
				}
				return bmp;
			}
			return null;
		}

		public void OnButtonClick()
		{
			var view = this.pluginServer.FocusedView;
			if( view != View.None )
			{
				this.pluginServer.InvokeCommand( QCommand.CreateNewFile, null, false, view );
			}
		}

		public void InitializeItem()
		{
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return Localizer.resources[1];
			}
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void Update()
		{
			var fCanCreateFile = false;
			var tab = this.pluginServer.SelectedTabInFocusedView;
			if( tab != null )
			{
				fCanCreateFile = CreateFolderButton.CanCreateFile( tab.Path );
			}
			this.pluginServer.UpdateItem( this, fCanCreateFile, false );
		}
	}

	[Plugin( PluginType.Background, typeof( Localizer ), 2, Version = "1.3.0.0" )]
	public class CreateConsecutiveFile : IBarButton
	{
		// 1.3.0.0		supports views

		private IPluginServer pluginServer;
		private IShellBrowser shellBrowser;
		internal static string REGNAME = @"CreateConsecutiveFile";

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.shellBrowser = shellBrowser;

			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.ActiveViewChanged += new PluginEventHandler( ViewEventSource_ActiveViewChanged );
			this.pluginServer.ShortcutKeyPressed += new PluginKeyEventHandler( pluginServer_ShortcutKeyPressed );
		}

		public void Close( EndCode endCode )
		{
		}

		public bool HasOption
		{
			get
			{
				return false;
			}
		}

		public void OnOption()
		{
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = new string[] { Localizer.resources[6] };
			return true;
		}

		public void OnShortcutKeyPressed( int index )
		{
		}
			
		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.CreateConsecutiveFile_large : Resource.CreateConsecutiveFile_small;
		}

		public void InitializeItem()
		{			
		}

		public void OnButtonClick()
		{
			this.CreateConsecutives();
		}

		public bool ShowTextLabel
		{
			get
			{
				return true;
			}
		}

		public string Text
		{
			get
			{
				return Localizer.resources[2];
			}
		}
		
		private void CreateConsecutives()
		{
			var tab = this.pluginServer.SelectedTabInFocusedView;
			if( tab != null )
			{
				string pathCurrent = tab.Path;
				if( Directory.Exists( pathCurrent ) )
				{
					var name = String.Empty;
					var sel = tab.SelectedPaths;
					if( sel.Count == 1 )
					{
						name = Path.GetFileName( sel[0] );
					}

					using( var ccf = new CreateConsecutivesForm( pathCurrent, this.pluginServer, name ) )
					{
						ccf.ShowDialog();
						return;
					}
				}				
			}
			System.Media.SystemSounds.Beep.Play();
		}

		private void Update()
		{
			bool fCancreate = false;
			try
			{
				var tab = this.pluginServer.SelectedTabInFocusedView;
				if( tab != null )
				{
					fCancreate = Directory.Exists( tab.Path );
				}
			}
			catch
			{
			}
			this.pluginServer.UpdateItem( this, fCancreate, false );
		}

		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void ViewEventSource_ActiveViewChanged( object sender, PluginEventArgs e )
		{
			this.Update();
		}

		private void pluginServer_ShortcutKeyPressed( object sender, PluginKeyEventArgs e )
		{
			if( !e.Repeat )
			{
				this.CreateConsecutives();
			}
		}

		public static void Uninstall()
		{
			using( var rkPlugin = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"Quizo", true ) )
			{
				if( rkPlugin != null )
				{
					rkPlugin.DeleteSubKey( REGNAME, false );
				}
			}
		}
	}
}
