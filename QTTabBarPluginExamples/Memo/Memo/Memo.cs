using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using QTPlugin;
using QTPlugin.Interop;

namespace QuizoPlugin
{
	// Do not edit the version and file version of this assembly.
	// Do not change building mode from 'Release'.

	[Plugin( PluginType.Background, typeof( Localizer ), Version = "128.0.0.2" )]
	public class Memo : IPluginClient
	{
		// 128.0.0.2		supports views.

		private IPluginServer pluginServer;		
		private MemoForm memoForm;
		internal ToolStripRenderer menuRenderer;
		private static MemoMode currentMode = ReadSettings();
		private static bool fShowOnHover;
		private const string REG_MEMO = @"Quizo\Memo";
		private System.Windows.Forms.Timer timerHover;
		private int HoveredTabID;

		#region IPluginClient members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.pluginServer.RegisterMenu( this, MenuType.Tab, Localizer.StringResources[0], true );

			this.pluginServer.ViewEventSource.NavigationComplete += new PluginEventHandler( pluginServer_NavigationComplete );
			this.pluginServer.ViewEventSource.TabChanged += new PluginEventHandler( ViewEventSource_TabChanged );
			this.pluginServer.ViewEventSource.PointedTabChanged += new PluginEventHandler( pluginServer_PointedTabChanged );

			this.menuRenderer = this.pluginServer.GetMenuRenderer();
		}

		public void Close( EndCode code )
		{
			if( this.memoForm != null )
			{
				if( this.memoForm.Visible )
				{
					Bounds = this.memoForm.WindowState == FormWindowState.Minimized ? this.memoForm.RestoreBounds : this.memoForm.Bounds;
				}
				Opacity = this.memoForm.Opacity;
				this.memoForm.Dispose();
				this.memoForm = null;
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
			this.ShowMemoForm( tab.Path, tab.Text, true );
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
			using( var of = new OptionForm( currentMode == MemoMode.Enabled, fShowOnHover ) )
			{
				if( DialogResult.OK == of.ShowDialog() )
				{
					currentMode = of.MemoMode;
					fShowOnHover = of.ShowOnHover;
				}
			}

			using( var rk = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + REG_MEMO ) )
			{
				if( rk != null )
				{
					rk.SetValue( "ShowAlways", currentMode == MemoMode.ShowAlways ? 1 : 0 );
					rk.SetValue( "ShowOnHover", fShowOnHover ? 1 : 0 );
				}
			}
		}

		public void OnShortcutKeyPressed( int index )
		{
			if( index == 0 )
			{
				var tab = this.pluginServer.SelectedTabInFocusedView;
				if( tab != null )
				{
					this.ShowMemoForm( tab.Path, tab.Text, true );
				}
			}
			else
			{
				if( this.memoForm != null )
				{
					this.memoForm.GiveFocus();
				}
			}
		}

		public bool QueryShortcutKeys( out string[] descriptions )
		{
			descriptions = new string[] { Localizer.StringResources[0], Localizer.StringResources[5] };
			return true;
		}

		#endregion

		private static MemoMode ReadSettings()
		{
			using( var rkPlugin = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + REG_MEMO, false ) )
			{
				if( rkPlugin != null )
				{
					object oShowOnHover = rkPlugin.GetValue( "ShowOnHover", 0 );
					if( oShowOnHover is int )
					{
						fShowOnHover = (int)oShowOnHover != 0;
					}

					object o = rkPlugin.GetValue( "ShowAlways", 0 );
					if( o is int )
					{
						if( (int)o != 0 )
						{
							return MemoMode.ShowAlways;
						}
					}
				}
			}
			return MemoMode.Enabled;
		}

		internal static Rectangle Bounds
		{
			get
			{
				using( var rkPlugin = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + REG_MEMO, false ) )
				{
					if( rkPlugin != null )
					{
						var o = rkPlugin.GetValue( "Bounds", null ) as byte[];
						if( o != null && o.Length == 16 )
						{
							var arr = ByteToInt32( o );
							return new Rectangle( arr[0], arr[1], arr[2], arr[3] );
						}
					}
				}
				var screen = Screen.PrimaryScreen.WorkingArea;
				return new Rectangle( screen.Right - 256, screen.Bottom - 256, 256, 256 );
			}
			set
			{
				using( var rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + REG_MEMO ) )
				{
					if( rkPlugin != null )
					{
						rkPlugin.SetValue( "Bounds", Int32ToByte( new int[] { value.Left, value.Top, value.Width, value.Height } ) );
					}
				}
			}
		}

		private static byte[] Int32ToByte( int[] input )
		{
			if( input != null )
			{
				if( input.Length > 0 )
				{
					List<byte> lst = new List<byte>();
					for( int i = 0; i < input.Length; i++ )
					{
						lst.AddRange( BitConverter.GetBytes( input[i] ) );
					}
					return lst.ToArray();
				}
				else
				{
					return new byte[0];
				}
			}
			return null;
		}

		private static int[] ByteToInt32( byte[] input )
		{
			if( input != null && input.Length > 3 && input.Length % 4 == 0 )
			{
				List<int> lst = new List<int>();
				for( int i = 0; i < input.Length; i += 4 )
				{
					lst.Add( BitConverter.ToInt32( input, i ) );
				}
				return lst.ToArray();
			}
			return null;
		}

		
		internal static double Opacity
		{
			get
			{
				using( var rkPlugin = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + REG_MEMO, false ) )
				{
					if( rkPlugin != null )
					{
						double d;
						var o = rkPlugin.GetValue( "Opacity", "1.0" ) as string;
						if( o != null && Double.TryParse( o, out d ) )
						{
							return d;
						}
					}
				}
				return 1d;
			}
			set
			{
				using( var rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + REG_MEMO ) )
				{
					if( rkPlugin != null )
					{
						rkPlugin.SetValue( "Opacity", Math.Min( 1, Math.Max( 0.1, value ) ).ToString() );
					}
				}
			}
		}

		internal static bool AlwaysTopMost
		{
			get
			{
				using( var rkPlugin = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + REG_MEMO, false ) )
				{
					if( rkPlugin != null )
					{
						var o = rkPlugin.GetValue( "AlwaysOnTop", 0 );
						if( o is int )
						{
							return (int)o != 0;
						}
					}
				}
				return false;
			}
			set
			{
				using( var rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + REG_MEMO ) )
				{
					if( rkPlugin != null )
					{
						rkPlugin.SetValue( "AlwaysOnTop", value ? 1 : 0 );
					}
				}
			}
		}


		public static void Uninstall()
		{
			using( var rk = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS, true ) )
			{
				if( rk != null )
				{
					rk.DeleteSubKey( REG_MEMO, false );
				}
			}
		}

		private void pluginServer_NavigationComplete( object sender, PluginEventArgs e )
		{
			this.UpdateMemoWindow();
		}

		private void UpdateMemoWindow()
		{
			try
			{
				var tab = this.pluginServer.SelectedTabInFocusedView;
				if( tab != null )
				{
					string path = tab.Path;

					if( !String.IsNullOrEmpty( path ) )
					{
						this.ShowMemoForm( path, tab.Text, false );
					}
					else
					{
						if( this.memoForm != null )
						{
							this.memoForm.HideMemo();
						}
					}
				}
			}
			catch
			{
			}
		}

		private void pluginServer_PointedTabChanged( object sender, PluginEventArgs e )
		{
			if( fShowOnHover && e.Index > -1 )
			{
				var id = -1;

				var tabs = this.pluginServer.GetTabs( e.View );
				if( tabs != null && tabs.Length > e.Index )
				{
					id = tabs[e.Index].ID;
				}

				if( id != this.HoveredTabID )
				{
					this.HoveredTabID = id;

					if( this.timerHover == null )
					{
						this.timerHover = new Timer();
						this.timerHover.Interval = SystemInformation.MouseHoverTime;
						this.timerHover.Tick += new EventHandler( timerHover_Tick );
					}
					this.timerHover.Stop();
					if( this.HoveredTabID != -1 )
					{
						this.timerHover.Start();
					}
				}
			}
		}

		private void ViewEventSource_TabChanged( object sender, PluginEventArgs e )
		{
			this.UpdateMemoWindow();
		}

		private void timerHover_Tick( object sender, EventArgs e )
		{
			this.timerHover.Stop();

			var tab = this.pluginServer.HitTest( Control.MousePosition );
			if( tab != null && tab.ID == this.HoveredTabID )
			{
				if( this.memoForm == null )
				{
					this.memoForm = new MemoForm( this );
				}
				if( this.memoForm.ContainsPath( tab.Path ) )
				{
					this.memoForm.ShowMemo( tab.Path, tab.Text );
				}
				else
				{
					this.memoForm.HideMemo();
				}
			}
		}

		private void ShowMemoForm( string path, string displayName, bool fForce )
		{
			try
			{
				if( this.memoForm == null )
				{
					this.memoForm = new MemoForm( this );
				}

				if( fForce || currentMode == MemoMode.ShowAlways || this.memoForm.ContainsPath( path ) )
				{
					this.memoForm.ShowMemo( path, displayName );
				}
				else
				{
					this.memoForm.HideMemo();
				}
			}
			catch
			{
			}
		}

		internal void OpenDirectory( string path )
		{
			this.pluginServer.CreateTab( new Address( path ), -1, false, true );
		}
	}

	enum MemoMode
	{
		Enabled,
		ShowAlways,
		Disabled, 
	}
}
