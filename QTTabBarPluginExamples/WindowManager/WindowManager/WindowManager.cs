using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

using QTPlugin;
using QTPlugin.Interop;
using System.Text;

namespace QuizoPlugins
{
	[Plugin( PluginType.Background, typeof( Localizer ) )]
	public class WindowManager : IBarDropButton
	{
		private IPluginServer pluginServer;
		private Localizer localizer;

		private byte[] ConfigValues = { 0, 0, 0, 0 };
		private Size sizeInitial = new Size( 800, 600 );
		private Point pntInitial = new Point( 0, 0 );
		private int ResizeDelta = 8;
		private Dictionary<string, Rectangle> dicPresets = new Dictionary<string, Rectangle>();
		private Dictionary<string, int> dicPresetsStatus = new Dictionary<string, int>();
		private string startingPreset = String.Empty;

		private bool fServerIsDesktopTool;
		private bool fNowOptionShowing;
		private static bool fNowTiled;
		private static Dictionary<IntPtr, RECT> dicTiledRectangle;

		private const uint SWP_NOSIZE = 0x0001;
		private const uint SWP_NOMOVE = 0x0002;
		private const uint SWP_NOZORDER = 0x0004;
		private const uint SWP_NOACTIVATE = 0x0010;
		private const uint WS_MAXIMIZE = 0x01000000;

		private const string CN_CabinetWClass = "CabinetWClass";
		private const string CN_Explorer = "ExploreWClass";
		const string PRODUCTNAME = "QTWindowManager";

		private int[] xPartitions = { 0, 0, 2, 2, 2, 2, 2, 3 };
		private int[] yPartitions = { 0, 0, 1, 2, 2, 3, 3, 3 };


		public WindowManager()
		{
			this.ReadSettings();
			this.localizer = new Localizer();
		}


		#region IPluginClient Members

		public void Open( IPluginServer pluginServer, IShellBrowser shellBrowser )
		{
			this.pluginServer = pluginServer;
			this.fServerIsDesktopTool = this.pluginServer.ExplorerHandle == IntPtr.Zero;

			this.RestoreInitialSize();
		}

		public bool QueryShortcutKeys( out string[] actions )
		{
			if( this.fServerIsDesktopTool )
			{
				actions = new string[] { this.localizer.KeysboardShortcuts[0], this.localizer.KeysboardShortcuts[15] };	// option and tile
			}
			else
			{
				List<string> lstActions = new List<string>( this.localizer.KeysboardShortcuts );
				lstActions.AddRange( this.dicPresets.Keys );
				actions = lstActions.ToArray();
			}
			return true;
		}

		public void Close( EndCode code )
		{
		}

		public void OnMenuItemClick( MenuType menuType, string menuText, ITab tab )
		{
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
			this.ShowSettingWindow( true );
		}

		public void OnShortcutKeyPressed( int index )
		{
			if( this.fServerIsDesktopTool )
			{
				if( index == 0 )
				{
					// option
					this.ShowSettingWindow( true );
				}
				else if( index == 1 )
				{
					this.TileExplorers();
				}
			}
			else
			{
				if( index == 0 )
				{
					// option
					this.ShowSettingWindow( true );
				}
				else if( index < 8 )
				{
					this.ResizeWindow( index );
				}
				else if( index < 12 )
				{
					this.MoveWindow( index );
				}
				else if( index < 15 )
				{
					this.MaxMinWindow( index );
				}
				else if( index == 15 )
				{
					this.TileExplorers();
				}
				else
				{
					this.DoPresets( index );
				}
			}
		}

		#endregion


		#region IBarButton Members

		public void InitializeItem()
		{
		}

		public Image GetImage( bool fLarge )
		{
			return fLarge ? Resource.QTWindowManager_large : Resource.QTWindowManager_small;
		}

		public void OnButtonClick()
		{
			this.ShowSettingWindow( true );
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
				return this.localizer.Strs[0];
			}
		}

		#endregion


		#region IBarDropButton Members

		public bool IsSplitButton
		{
			get
			{
				return true;
			}
		}

		public void OnDropDownOpening( ToolStripDropDownMenu menu )
		{
			if( menu.Items.Count != this.dicPresets.Count + 5 )
			{
				menu.Items.Clear();

				menu.Items.Add( this.localizer.KeysboardShortcuts[7] );
				menu.Items.Add( this.localizer.KeysboardShortcuts[12] );
				menu.Items.Add( this.localizer.KeysboardShortcuts[13] );
				menu.Items.Add( this.localizer.KeysboardShortcuts[14] );
				menu.Items.Add( fNowTiled ? this.localizer.Strs[2] : this.localizer.KeysboardShortcuts[15] );	// tile / untile

				foreach( string name in this.dicPresets.Keys )
				{
					ToolStripMenuItem tsmi = new ToolStripMenuItem( name );
					tsmi.Tag = true;
					menu.Items.Add( tsmi );
				}
			}
			else
			{
				menu.Items[4].Text = fNowTiled ? this.localizer.Strs[2] : this.localizer.KeysboardShortcuts[15];
			}
		}

		public void OnDropDownItemClick( ToolStripItem item, MouseButtons mouseButton )
		{
			( (ToolStripDropDown)item.Owner ).Close( ToolStripDropDownCloseReason.ItemClicked );

			if( item.Tag != null && item.Tag is bool )
			{
				Rectangle rct ;
				if( this.dicPresets.TryGetValue( item.Text, out rct ) )
				{
					int iFlags = 0;
					this.dicPresetsStatus.TryGetValue( item.Text, out iFlags );

					this.DoPresetsCore( rct, iFlags );
				}
				return;
			}

			if( item.Text == this.localizer.KeysboardShortcuts[7] )
			{
				this.ResizeWindow( 7);
			}
			else if( item.Text == this.localizer.KeysboardShortcuts[12] )
			{
				this.MaxMinWindow( 12 );
			}
			else if( item.Text == this.localizer.KeysboardShortcuts[13] )
			{
				this.MaxMinWindow( 13 );
			}
			else if( item.Text == this.localizer.KeysboardShortcuts[14] )
			{
				this.MaxMinWindow( 14 );
			}
			else
			{
				this.TileExplorers();
			}
		}


		#endregion



		public static void Uninstall()
		{
			//using( RegistryKey rkPlugin = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS, true ) )
			//{
			//    if( rkPlugin != null )
			//    {
			//        try
			//        {
			//            rkPlugin.DeleteSubKeyTree( PRODUCTNAME );
			//        }
			//        catch
			//        {
			//        }
			//    }
			//}
		}

		private static int ReadInt( RegistryKey rk, string valName, int defvalue )
		{
			object o = rk.GetValue( valName, defvalue );
			if( o is int )
			{
				return (int)o;
			}
			return defvalue;
		}

		private void ReadSettings()
		{
			using( RegistryKey rkPluginQTWM = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + PRODUCTNAME ) )
			{
				if( rkPluginQTWM != null )
				{
					this.ConfigValues = rkPluginQTWM.GetValue( "Config", new byte[] { 0, 0, 0, 0 } ) as byte[];
					if( this.ConfigValues == null )
					{
						this.ConfigValues = new byte[] { 0, 0, 0, 0 };
					}

					int w = ReadInt( rkPluginQTWM, "InitialWidth", 800 );
					int h = ReadInt( rkPluginQTWM, "InitialHeight", 600 );
					int x = ReadInt( rkPluginQTWM, "InitialX", 0 );
					int y = ReadInt( rkPluginQTWM, "InitialY", 0 );

					this.sizeInitial = new Size( w, h );
					this.pntInitial = new Point( x, y );
					this.ResizeDelta = ReadInt( rkPluginQTWM, "ResizeDelta", 3 );
					this.startingPreset = rkPluginQTWM.GetValue( "StartingPreset", String.Empty) as string;

					using( RegistryKey rkPresets = rkPluginQTWM.OpenSubKey( "Presets" ) )
					{
						if( rkPresets != null )
						{
							foreach( string name in rkPresets.GetValueNames() )
							{
								string val = rkPresets.GetValue( name ) as string;
								if( !String.IsNullOrEmpty( val ) )
								{
									string[] vals = val.Split( new char[] { ',' } );
									if( vals.Length > 3 )
									{
										int[] nums = new int[4];
										bool fFail = false;

										for( int i = 0; i < 4; i++ )
										{
											string strNum = vals[i].Trim();

											if( !int.TryParse( strNum, out nums[i] ) )
											{
												fFail = true;
												break;
											}
										}

										if( !fFail )
										{
											this.dicPresets[name] = new Rectangle( nums[0], nums[1], nums[2], nums[3] );

											if( vals.Length == 5 )
											{
												int iFlags;
												if( Int32.TryParse( vals[4], out iFlags ) )
												{
													this.dicPresetsStatus[name] = iFlags;
												}
											}
										}
									}
								}
							}
						}
					}

					if( this.startingPreset == null )
					{
						this.startingPreset = String.Empty;
					}
					else if( this.startingPreset.Length > 0 )
					{
						if( !this.dicPresets.ContainsKey( this.startingPreset ) )
						{
							this.startingPreset = String.Empty;
						}
					}
				}
			}
		}

		private void SaveSettings()
		{
			using( RegistryKey rkPluginQTWM = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\" + PRODUCTNAME ) )
			{
				if( rkPluginQTWM != null )
				{
					rkPluginQTWM.SetValue( "Config", this.ConfigValues );
					rkPluginQTWM.SetValue( "InitialWidth", this.sizeInitial.Width );
					rkPluginQTWM.SetValue( "InitialHeight", this.sizeInitial.Height );
					rkPluginQTWM.SetValue( "InitialX", this.pntInitial.X );
					rkPluginQTWM.SetValue( "InitialY", this.pntInitial.Y );
					rkPluginQTWM.SetValue( "ResizeDelta", this.ResizeDelta );

					rkPluginQTWM.DeleteSubKey( "Presets", false );
					if( this.dicPresets.Count > 0 )
					{
						using( RegistryKey rkPresets = rkPluginQTWM.CreateSubKey( "Presets" ) )
						{
							foreach( string name in this.dicPresets.Keys )
							{
								Rectangle rct = this.dicPresets[name];

								string val = rct.X + "," + rct.Y + "," + rct.Width + "," + rct.Height;

								int iFlags;
								if( this.dicPresetsStatus.TryGetValue( name, out iFlags ) )
								{
									val += "," + iFlags;
								}

								rkPresets.SetValue( name, val );
							}
						}
					}

					if( !String.IsNullOrEmpty( this.startingPreset ) )
					{
						if( this.dicPresets.ContainsKey( this.startingPreset ) )
						{
							rkPluginQTWM.SetValue( "StartingPreset", this.startingPreset );
						}
						else
						{
							rkPluginQTWM.SetValue( "StartingPreset", String.Empty );
							this.startingPreset = String.Empty;
						}
					}
					else
					{
						rkPluginQTWM.SetValue( "StartingPreset", String.Empty );
						this.startingPreset = String.Empty;
					}
				}
			}
		}



		private void RestoreInitialSize()
		{
			bool fLoc = ( this.ConfigValues[0] & 0x20 ) != 0;
			bool fSiz = ( this.ConfigValues[0] & 0x80 ) != 0;
			bool fPreset = ( this.ConfigValues[0] & 0x10 ) != 0;

			if( fLoc || fSiz || fPreset )
			{
				IntPtr hwnd = this.pluginServer.ExplorerHandle;
				if( hwnd != IntPtr.Zero )
				{
					if( PInvoke_QTWM.IsZoomed( hwnd ) )
					{
						const int SW_RESTORE = 9;
						PInvoke_QTWM.ShowWindow( hwnd, SW_RESTORE );
					}

					if( fPreset )
					{
						if( !String.IsNullOrEmpty( this.startingPreset ) && this.dicPresets.ContainsKey( this.startingPreset ) )
						{
							Rectangle rctPreset = this.dicPresets[this.startingPreset];
							int iFlags;
							this.dicPresetsStatus.TryGetValue( this.startingPreset, out iFlags );

							PInvoke_QTWM.SetWindowPos( hwnd, IntPtr.Zero, rctPreset.X, rctPreset.Y, rctPreset.Width, rctPreset.Height, StatusFlagsToSWP( iFlags ) );
							WindowManager.RemoveMAXIMIZE( hwnd );
						}
					}
					else
					{
						uint uFlags = SWP_NOZORDER | ( fLoc ? 0 : SWP_NOMOVE ) | ( fSiz ? 0 : SWP_NOSIZE );
						PInvoke_QTWM.SetWindowPos( hwnd, IntPtr.Zero, this.pntInitial.X, this.pntInitial.Y, this.sizeInitial.Width, this.sizeInitial.Height, uFlags );
						WindowManager.RemoveMAXIMIZE( hwnd );
					}
				}
			}
		}

		private void ResizeWindow( int index )
		{
			IntPtr hwnd = this.pluginServer.ExplorerHandle;

			if( hwnd == IntPtr.Zero )
				return;

			RECT rct;
			PInvoke_QTWM.GetWindowRect( hwnd, out rct );

			bool fAuto = ( this.ConfigValues[0] & 0x40 ) == 0;

			int x = rct.left;
			int y = rct.top;
			int w = rct.Width;
			int h = rct.Height;
			uint uFlags = SWP_NOZORDER;

			switch( index )
			{
				case 1:
					//Enlarge window
					if( fAuto )
					{
						x -= this.ResizeDelta;
						y -= this.ResizeDelta;
						w += this.ResizeDelta * 2;
						h += this.ResizeDelta * 2;
					}
					else
					{
						w += this.ResizeDelta;
						h += this.ResizeDelta;
						uFlags |= SWP_NOMOVE;
					}
					break;

				case 2:
					//Shrink window
					if( fAuto )
					{
						x += this.ResizeDelta;
						y += this.ResizeDelta;
						w -= this.ResizeDelta * 2;
						h -= this.ResizeDelta * 2;
					}
					else
					{
						w -= this.ResizeDelta;
						h -= this.ResizeDelta;
						uFlags |= SWP_NOMOVE;
					}
					break;

				case 3:
					//Widen window
					if( fAuto )
					{
						x -= this.ResizeDelta;
						w += this.ResizeDelta * 2;
					}
					else
					{
						w += this.ResizeDelta;
						uFlags |= SWP_NOMOVE;
					}
					break;

				case 4:
					//Narrow widnow
					if( fAuto )
					{
						x += this.ResizeDelta;
						w -= this.ResizeDelta * 2;
					}
					else
					{
						w -= this.ResizeDelta;
						uFlags |= SWP_NOMOVE;
					}
					break;

				case 5:
					//Heighten window
					if( fAuto )
					{
						y -= this.ResizeDelta;
						h += this.ResizeDelta * 2;
					}
					else
					{
						h += this.ResizeDelta;
						uFlags |= SWP_NOMOVE;
					}
					break;
				case 6:
					//Lower window
					if( fAuto )
					{
						y += this.ResizeDelta;
						h -= this.ResizeDelta * 2;
					}
					else
					{
						h -= this.ResizeDelta;
						uFlags |= SWP_NOMOVE;
					}
					break;

				case 7:
					//Restore size
					w = this.sizeInitial.Width;
					h = this.sizeInitial.Height;
					uFlags |= SWP_NOMOVE;
					break;
			}


			if( fAuto )
			{
				Rectangle rctScreen = Screen.FromHandle( hwnd ).Bounds;

				if( x < rctScreen.X )
				{
					x = rctScreen.X;

					if( index == 7 )
						uFlags &= ~SWP_NOMOVE;
				}
				if( y < rctScreen.Y )
				{
					y = rctScreen.Y;

					if( index == 7 )
						uFlags &= ~SWP_NOMOVE;
				}
				if( x + w > rctScreen.Right )
				{
					if( index == 7 )
					{
						x = rctScreen.Right - w;
						uFlags &= ~SWP_NOMOVE;
					}
					else
					{
						w = rctScreen.Right - x;
					}
				}
				if( y + h > rctScreen.Bottom )
				{
					if( index == 7 )
					{
						y = rctScreen.Bottom - h;
						uFlags &= ~SWP_NOMOVE;
					}
					else
					{
						h = rctScreen.Bottom - y;
					}
				}
			}

			if( h > 150 && w > 122 )
			{
				PInvoke_QTWM.SetWindowPos( hwnd, IntPtr.Zero, x, y, w, h, uFlags );

				WindowManager.RemoveMAXIMIZE( hwnd );
			}
		}

		private void MoveWindow( int index )
		{
			IntPtr hwnd = this.pluginServer.ExplorerHandle;

			if( hwnd == IntPtr.Zero )
				return;

			RECT rct;
			PInvoke_QTWM.GetWindowRect( hwnd, out rct );

			int x = rct.left;
			int y = rct.top;
			
			switch( index )
			{
				case 8:
					// left
					x -= this.ResizeDelta;
					break;

				case 9:
					// right
					x += this.ResizeDelta;
					break;

				case 10:
					// up
					y -= this.ResizeDelta;
					break;

				case 11:
					// down
					y += this.ResizeDelta;
					break;
			}

			PInvoke_QTWM.SetWindowPos( hwnd, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER );

			WindowManager.RemoveMAXIMIZE( hwnd );
		}

		internal static void RemoveMAXIMIZE( IntPtr hwnd )
		{
			PInvoke_QTWM.SetWindowLongPtr( hwnd, -16, PInvoke_QTWM.Ptr_OP_AND( PInvoke_QTWM.GetWindowLongPtr( hwnd, -16 ), ~WS_MAXIMIZE ) );
		}

		private void MaxMinWindow( int index )
		{
			//const int SW_MAXIMIZE = 3;
			//const int SW_MINIMIZE = 6;
			const int SW_RESTORE = 9;
			const int SW_SHOWMINIMIZED = 2;
			const int SW_SHOWMAXIMIZED = 3;

			IntPtr hwnd = this.pluginServer.ExplorerHandle;

			if( hwnd != IntPtr.Zero )
			{
				int nCmdShow;
				switch( index )
				{
					case 13:
						// Minimize
						nCmdShow = SW_SHOWMINIMIZED;
						break;

					case 14:
						// Restore
						nCmdShow = SW_RESTORE;
						break;

					default:	//12
						// Maximize
						nCmdShow = SW_SHOWMAXIMIZED;
						break;
				}

				PInvoke_QTWM.ShowWindow( hwnd, nCmdShow );
			}
		}

		private void DoPresets( int index )
		{
			index -= this.localizer.KeysboardShortcuts.Length;
			int i = 0;
			foreach( string name in this.dicPresets.Keys )
			{
				if( i == index )
				{
					int iFlags = 0;
					this.dicPresetsStatus.TryGetValue( name, out iFlags );
					this.DoPresetsCore( this.dicPresets[name], iFlags );
					return;
				}
				i++;
			}
		}

		private void DoPresetsCore( Rectangle rct, int iFlag )
		{
			IntPtr hwnd = this.pluginServer.ExplorerHandle;
			if( hwnd != IntPtr.Zero )
			{
				PInvoke_QTWM.SetWindowPos( hwnd, IntPtr.Zero, rct.X, rct.Y, rct.Width, rct.Height, StatusFlagsToSWP( iFlag ) );
				WindowManager.RemoveMAXIMIZE( hwnd );
			}
		}

		private void ShowSettingWindow( bool fSetModal )
		{
			try
			{
				if( fSetModal && this.pluginServer != null )
				{
					this.pluginServer.ExecuteCommand( Commands.SetModalState, true );
				}

				if( !this.fNowOptionShowing )
				{
					using( SettingWindow sw = new SettingWindow( new Rectangle( this.pntInitial, this.sizeInitial ), this.ConfigValues, ResizeDelta, this.pluginServer.ExplorerHandle, this.dicPresets, this.dicPresetsStatus, this.startingPreset ) )
					{
						this.fNowOptionShowing = true;
						if( sw.ShowDialog() == DialogResult.OK )
						{
							this.sizeInitial = sw.InitialSize;
							this.pntInitial = sw.InitialLocation;
							this.ConfigValues = sw.ConfigValues;
							this.ResizeDelta = sw.ResizeDelta;
							this.dicPresets = sw.Presets;
							this.dicPresetsStatus = sw.PresetsStatus;
							this.startingPreset = sw.StartingPreset;
							if( this.startingPreset == null )
							{
								this.startingPreset = String.Empty;
							}

							this.SaveSettings();
						}
						this.fNowOptionShowing = false;
					}
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
			finally
			{
				if( fSetModal && this.pluginServer != null )
				{
					this.pluginServer.ExecuteCommand( Commands.SetModalState, false );
				}
			}
		}

		private void TileExplorers()
		{
			IntPtr hwndCurrent = this.pluginServer.ExplorerHandle;
			List<IntPtr> lstHWNDs = this.EnumExplorer( false );
			int c = lstHWNDs.Count;

			if( c > 1 )
			{
				if( !fNowTiled )
				{
					int xPartition = c > this.xPartitions.Length - 1 ? this.xPartitions[this.xPartitions.Length - 1] : this.xPartitions[c];
					int yPartition = c > this.yPartitions.Length - 1 ? this.yPartitions[this.yPartitions.Length - 1] : this.yPartitions[c];

					Rectangle rctCurrentScreen;
					if( hwndCurrent == IntPtr.Zero )
					{
						rctCurrentScreen = Screen.PrimaryScreen.WorkingArea;
					}
					else
					{
						RECT rctCurrentWindow = GetWindowRect( hwndCurrent );
						rctCurrentScreen = Screen.FromPoint( new Point( rctCurrentWindow.left, rctCurrentWindow.top ) ).WorkingArea;
					}

					List<Rectangle> lstRectangles = new List<Rectangle>();

					int w = rctCurrentScreen.Width / xPartition;
					int h = rctCurrentScreen.Height / yPartition;

					for( int x = 0; x < xPartition; x++ )
					{
						for( int y = 0; y < yPartition; y++ )
						{
							lstRectangles.Add( new Rectangle( rctCurrentScreen.X + ( x * w ), rctCurrentScreen.Y + ( y * h ), w, h ) );

							if( lstRectangles.Count == c )
							{
								goto RECTSCREATED;
							}
						}
					}
					RECTSCREATED:

					dicTiledRectangle = new Dictionary<IntPtr, RECT>();

					if( lstHWNDs.Remove( hwndCurrent ) )
					{
						// bring current window to first
						lstHWNDs.Insert( 0, hwndCurrent );
					}
					int indexOfRect = 0;
					for( int i = 0; i < lstHWNDs.Count; i++ )
					{
						IntPtr hwnd = lstHWNDs[i];
						dicTiledRectangle[hwnd] = GetWindowRect( hwnd );
						Rectangle rct = lstRectangles[indexOfRect];

						PInvoke_QTWM.SetWindowPos( hwnd, IntPtr.Zero, rct.X, rct.Y, rct.Width, rct.Height, hwnd == hwndCurrent ? SWP_NOZORDER : SWP_NOZORDER | SWP_NOACTIVATE );

						indexOfRect++;
						if( indexOfRect > lstRectangles.Count - 1 )
						{
							indexOfRect = 0;
						}
					}

					fNowTiled = true;
				}
				else
				{
					if( dicTiledRectangle != null )
					{
						foreach( IntPtr hwnd in dicTiledRectangle.Keys )
						{
							RECT rct = dicTiledRectangle[hwnd];

							PInvoke_QTWM.SetWindowPos( hwnd, IntPtr.Zero, rct.left, rct.top, rct.Width, rct.Height, hwnd == hwndCurrent ? SWP_NOZORDER : SWP_NOZORDER | SWP_NOACTIVATE );
						}

						dicTiledRectangle.Clear();
						fNowTiled = false;
					}
				}
			}
		}

		private List<IntPtr> lstExploererHwnd;
		private List<IntPtr> EnumExplorer( bool fExcludeCurrent )
		{
			this.lstExploererHwnd = new List<IntPtr>();

			PInvoke_QTWM.EnumWindows( this.EnumExplorerCallback, fExcludeCurrent ? this.pluginServer.ExplorerHandle : IntPtr.Zero );

			return this.lstExploererHwnd;
		}

		private bool EnumExplorerCallback( IntPtr hwnd, IntPtr lParam )
		{
			StringBuilder sb = new StringBuilder( 260 );
			PInvoke_QTWM.GetClassName( hwnd, sb, sb.MaxCapacity );

			string className = sb.ToString();
			if( lParam != hwnd && ( className == CN_CabinetWClass || className == CN_Explorer ) )
			{
				this.lstExploererHwnd.Add( hwnd );
			}

			return true;
		}



		private static uint StatusFlagsToSWP( int iFlags )
		{
			switch( iFlags )
			{
				case 1:
					return SWP_NOSIZE | SWP_NOZORDER;

				case 2:
					return SWP_NOMOVE | SWP_NOZORDER;

				default:
					return SWP_NOZORDER;
			}
		}

		private static RECT GetWindowRect( IntPtr hwnd )
		{
			RECT rct;
			PInvoke_QTWM.GetWindowRect( hwnd, out rct );
			return rct;
		}


		/*
		 * config	0x80	if on, initial sizing
		 *			0x40	if on, enlarge/shrink window at fixed pos. if off, preserve relative positon to screen
		 *			0x20	if on, initial location
		 *			0x10	if on, starting preset
		 *			0x08	if on, restore closed rct	<- ????
		 */
	}

	sealed class Localizer : LocalizedStringProvider
	{
		string[] strs;
		string[] strsKeys;

		public Localizer()
		{
			if( System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				strs = Resource.ResStrs_ja.Split( new char[] { ';' } );
				strsKeys = Resource.KeyboardShortcutNames_ja.Split( new char[] { ';' } );
			}
			else
			{
				strs = Resource.ResStrs.Split( new char[] { ';' } );
				strsKeys = Resource.KeyboardShortcutNames.Split( new char[] { ';' } );
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

		public string[] KeysboardShortcuts
		{
			get
			{
				return this.strsKeys;
			}
		}
	}

}
