using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using QTPlugin;
using QTPlugin.Interop;
using System.Runtime.InteropServices;

namespace QuizoPlugins
{
	[Plugin( PluginType.BackgroundMultiple, typeof(Localizer), Version = "1.1.0.2" )]
	public class Spacer : IBarMultipleCustomItems
	{
		private List<ToolStripSpacerItem> lst = new List<ToolStripSpacerItem>();
		private List<int> lstWidths = new List<int>();
		private List<bool> lstSpring = new List<bool>();
		private Localizer localizer = new Localizer();

		private static bool fDoubleClickTransparency;


		#region IPluginClient menmbers

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.ReadSetting();
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			actions = null;
			return false;
		}

		public void Close( EndCode endCode )
		{
			if( endCode == EndCode.Hidden || endCode == EndCode.Unloaded )
			{
				foreach( ToolStripSpacerItem tssi in this.lst )
				{
					tssi.Dispose();
				}
				this.lst.Clear();

				this.SaveSetting();
			}
			else if( endCode == EndCode.WindowClosed )
			{
				this.SaveSetting();
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
			using( OptionForm of = new OptionForm( fDoubleClickTransparency, this.localizer.strClickTransparency ) )
			{
				of.ShowDialog();

				fDoubleClickTransparency = of.DoubleClickTransparent;
			}

			using( RegistryKey rk = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\Spacer" ) )
			{
				rk.SetValue( String.Empty, fDoubleClickTransparency ? "transparent" : String.Empty );
			}
		}

		public void OnShortcutKeyPressed( int index )
		{
		}

		#endregion

		#region IBarMultipleCustomItems menmbers

		public void Initialize( int[] order )
		{
			foreach( ToolStripSpacerItem tssi in this.lst )
			{
				tssi.Dispose();
			}
			lst.Clear();
		}

		public ToolStripItem CreateItem( bool fLarge, DisplayStyle displayStyle, int index )
		{
			int w = 48;
			bool fSpring = false;
			if( this.lstWidths.Count > index )
			{
				w = this.lstWidths[index];
				fSpring = this.lstSpring[index];
			}
			else
			{
				this.lstWidths.Add( w );
				this.lstSpring.Add( false );
			}

			ToolStripSpacerItem tssi = new ToolStripSpacerItem( fLarge, w );
			tssi.Spring = fSpring;
			tssi.ResizeComplete += new EventHandler( tssis_ResizeComplete );
			tssi.DoubleClick += new EventHandler( tssi_DoubleClick );

			this.lst.Add( tssi );

			return tssi;
		}

		public int Count
		{
			get
			{
				return -1;
			}
		}

		public Image GetImage( bool fLarge, int index )
		{
			return fLarge ? Resource.Spacer_large : Resource.Spacer_small;
		}

		public string GetName( int index )
		{
			return this.localizer.Name;
		}

		#endregion

		private void tssi_DoubleClick( object sender, EventArgs e )
		{
			ToolStripSpacerItem tssi = (ToolStripSpacerItem)sender;
			tssi.Spring = !tssi.Spring;
			this.SaveSetting();

			if( fDoubleClickTransparency )
			{
				// redirect double clicked event to parent toolstrip
				ToolStrip toolStrip = tssi.Owner;
				if( toolStrip != null && toolStrip.IsHandleCreated )
				{
					const int MK_LBUTTON = 0x0001;
					const int WM_APP = 0x8000;
					const int QTBB_FIRST = WM_APP;
					const int QTBB_RAISE_MOUSEDBLCLICK = ( QTBB_FIRST + 2 );	// wParam MK_xx value, lParam pont in client coordinate, fixed.

					Point pnt = toolStrip.PointToClient( Control.MousePosition );
					Win32.SendMessage( Win32.GetParent( toolStrip.Handle ), QTBB_RAISE_MOUSEDBLCLICK, (IntPtr)MK_LBUTTON, Win32.Make_LPARAM( pnt.X, pnt.Y ) );
				}
			}
		}

		private void tssis_ResizeComplete( object sender, EventArgs e )
		{
			for( int i = this.lst.Count - 1; i > -1; i-- )
			{
				ToolStripSpacerItem tssi = this.lst[i];
				if( tssi.Spring )
				{
					tssi.RefreshWidth();
				}
			}
			this.SaveSetting();
		}

		private void ReadSetting()
		{
			this.lstWidths.Clear();
			this.lstSpring.Clear();

			try
			{
				using( RegistryKey rk = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\Spacer" ) )
				{
					if( rk != null )
					{
						foreach( string vn in rk.GetValueNames() )
						{
							if( vn.Length == 0 )
							{
								fDoubleClickTransparency = !String.IsNullOrEmpty( rk.GetValue( String.Empty ) as string );
							}
							else
							{
								int i = (int)rk.GetValue( vn, 0 );
								this.lstSpring.Add( i < 0 );
								this.lstWidths.Add( Math.Abs( i ) );
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		private void SaveSetting()
		{
			this.lstWidths.Clear();
			this.lstSpring.Clear();
			foreach( ToolStripSpacerItem tssi in this.lst )
			{
				if( tssi != null /*&& !tssi.IsDisposed */)
				{
					this.lstWidths.Add( tssi.Width );
					this.lstSpring.Add( tssi.Spring );
				}
			}

			try
			{
				using( RegistryKey rk = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\Spacer" ) )
				{
					if( rk != null )
					{
						foreach( string vn in rk.GetValueNames() )
						{
							rk.DeleteValue( vn );
						}

						for( int i = 0; i < this.lstWidths.Count; i++ )
						{
							int iFactor = this.lstSpring[i] ? -1 : 1;

							rk.SetValue( i.ToString(), this.lstWidths[i] * iFactor );
						}

						rk.SetValue( String.Empty, fDoubleClickTransparency ? "transparent" : String.Empty );
					}
				}
			}
			catch
			{
			}

		}

		public static void Uninstall()
		{
			try
			{
				using( RegistryKey rk = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGIN + @"\Quizo", true ) )
				{
					if( rk != null )
					{
						rk.DeleteSubKeyTree( "Spacer" );
					}
				}
			}
			catch
			{
			}

		}
	}


	sealed class Localizer : LocalizedStringProvider
	{
		private string parent;

		public Localizer()
		{
			this.parent = CultureInfo.CurrentCulture.Parent.Name;
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
				if( parent == "ja" )
				{
					return "ボタンバーに伸縮可能なスペーサーを複数追加できます。ダブルクリックするとスプリングモードと交互に切り替わります。";
				}
				else
				{
					return "Add expandable spacers to ButtonBar. Double-click to toggle spring mode.";
				}
			}
		}

		public override string Name
		{
			get
			{
				if( parent == "ja" )
				{
					return "スペーサー";
				}
				else
				{
					return "Spacer";
				}
			}
		}

		public override void SetKey( int iKey )
		{
		}

		public string strClickTransparency
		{
			get
			{
				if( parent == "ja" )
				{
					return "ダブルクリックを透過させる";
				}
				else
				{
					return "double click transparent";
				}
			}
		}
	}

	static class Win32
	{
		[DllImport( "user32.dll", CharSet = CharSet.Unicode )]
		public static extern IntPtr SendMessage( IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam );

		[DllImport( "user32.dll" )]
		public static extern IntPtr GetParent( IntPtr hWnd );

		public static IntPtr Make_LPARAM( int x, int y )
		{
			//#define MAKELPARAM(l, h)      ((LPARAM)(DWORD)MAKELONG(l, h))
			//#define MAKELONG(a, b)		((LONG)(((WORD)(((DWORD_PTR)(a)) & 0xffff)) | ((DWORD)((WORD)(((DWORD_PTR)(b)) & 0xffff))) << 16))
			//DWORD_PTR -> unsigned long or unsigned int64

			return (IntPtr)(int)( ( x & 0xFFFF ) | ( ( y & 0xFFFF ) << 16 ) );
		}


	}
}
