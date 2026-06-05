using System;
using System.Windows.Forms;
using Microsoft.Win32;
using QTPlugin;
using QTPlugin.Interop;
using System.Globalization;

namespace QuizoPlugin
{
	[Plugin( PluginType.Background, typeof( Localizer ), Version = "1.3.0.0" )]
	public class ActivateByMouseHover : IPluginClient
	{
		// 1.3.0.0			pluginlib 1.3.0.0, supports tabs in extra views

		private IPluginServer pluginServer;
		private IShellBrowser shellBrowser;
		private System.Windows.Forms.Timer timer;

		private ITab previousTab;
		private static int mouseHoverTime = 700;

		private const string REGNAME = "ActivateByMouseHover";


		static ActivateByMouseHover()
		{
			ReadSetting();
		}

		public void Open( IPluginServer pluginServer, QTPlugin.Interop.IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.shellBrowser = shellBrowser;

			this.pluginServer.ViewEventSource.PointedTabChanged += new PluginEventHandler( pluginServer_PointedTabChanged );
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
		}

		public void Close( EndCode endCode )
		{
			if( this.timer != null )
			{
				this.timer.Dispose();
				this.timer = null;
			}
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
		}

		public void OnOption()
		{
			using( var sf = new SettingForm( mouseHoverTime ) )
			{
				if( DialogResult.OK == sf.ShowDialog() )
				{
					mouseHoverTime = sf.Value;
					SaveSetting();
					if( this.timer != null )
					{
						this.timer.Interval = mouseHoverTime;
					}
				}
			}
		}

		public void OnShortcutKeyPressed( int index )
		{
		}
		
		public bool HasOption
		{
			get
			{
				return true;
			}
		}



		private void pluginServer_PointedTabChanged( object sender, PluginEventArgs e )
		{
			if( this.timer == null )
			{
				this.timer = new Timer();
				this.timer.Interval = mouseHoverTime;
				this.timer.Tick += new EventHandler( timer_Tick );
			}

			this.timer.Enabled = false;

			//var tab = this.pluginServer.HitTest( Control.MousePosition );

			var tabs = this.pluginServer.GetTabs( e.View );
			if( -1 < e.Index && e.Index < tabs.Length )
			{
				this.previousTab = tabs[e.Index];
				this.timer.Enabled = true;
			}
			else
			{
				this.previousTab = null;
			}
		}

		private void timer_Tick( object sender, EventArgs e )
		{
			try
			{
				if( this.previousTab != null )
				{
					this.previousTab.Selected = true;
				}
				this.timer.Enabled = false;
			}
			catch
			{
			}
		}

		private static void ReadSetting()
		{
			using( var rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\" + REGNAME ) )
			{
				if( rkPlugin != null )
				{
					var obj  =  rkPlugin.GetValue( "MouseHoverTime", 700 );
					if( obj is int )
					{
						mouseHoverTime = (int)obj;
					}
				}
			}
		}

		private static void SaveSetting()
		{
			using( var rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\" + REGNAME ) )
			{
				if( rkPlugin != null )
				{
					rkPlugin.SetValue( "MouseHoverTime", mouseHoverTime );
				}
			}
		}

	}

	sealed class Localizer : LocalizedStringProvider2
	{
		private bool fJa;

		public Localizer()
		{
			fJa = CultureInfo.CurrentCulture.Name == "ja-JP";
		}

		public override string Name
		{
			get
			{
				return fJa ? "マウスホバーでタブを選択" : "Activate By MouseHover";
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
				return fJa ? "マウスカーソルをタブの上に置くだけで、そのタブを選択できるようになります。ウェイト時間設定可。" : "Activate a tab by mouse-hover. To set delay time, press Option. ";
			}
		}

		public override DateTime LastUpdate
		{
			get
			{
				return new DateTime( 2014, 12, 14 );
			}
		}

		public override string SupportURL
		{
			get
			{
				return "https://qttabbar.backlog.jp/projects/Q";
			}
		}

		public override void SetKey( int iKey )
		{
		}
	}
}
