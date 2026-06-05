using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QTPlugin.Interop;

namespace QuizoPlugins
{
	public partial class SettingWindow : Form
	{
		private IntPtr hwndExplorer;
		private Dictionary<string, Rectangle> dicPresets;
		private Dictionary<string, int> dicPresetsStatus;
		private string startingPreset;
		const uint SWP_NOSIZE = 0x0001;
		const uint SWP_NOZORDER = 0x0004;
		const uint SWP_NOMOVE = 0x0002;

		public SettingWindow( Rectangle rctInitial, byte[] config, int delta_RESIZE, IntPtr hwnd, Dictionary<string, Rectangle> dicPresets, Dictionary<string, int> dicPresetsStatus, string startingPreset )
		{
			InitializeComponent();

			if( System.Globalization.CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				string[] strs = Resource.ResStrs_Options_ja.Split( new char[] { ';' } );

				this.chbInitialSize.Text = strs[0];
				this.chbInitialLoc.Text = strs[1];
				this.buttonRestoreSize.Text = strs[2];
				this.buttonRestoreLoc.Text = strs[3];
				this.checkBoxResizeMode.Text = strs[4];
				this.labelDELTARESIZE.Text = strs[5];
				this.groupBoxPresets.Text = strs[6];
				this.buttonSet.Text = strs[7];
				this.buttonDel.Text = strs[8];
				this.buttonOK.Text = strs[9];
				this.buttonCancel.Text = strs[10];
				this.buttonGetCurLoc.Text = strs[11];
				this.buttonGetCurSize.Text = strs[12];
				this.chbStartingPreset.Text = strs[13];
				this.buttonGetCurrentToPreset.Text = strs[14];
				this.btnApplyPreset.Text = strs[15];

				string[] strsFnt = Resource.DefaultFont_ja.Split( new char[] { ';' } );

				float f;
				if( !Single.TryParse( strsFnt[1], out f ) )
				{
					f = 9;
				}
				this.Font = new Font( strsFnt[0], f );
			}


			this.hwndExplorer = hwnd;	// can be NULL
			this.dicPresets = new Dictionary<string, Rectangle>( dicPresets );
			this.dicPresetsStatus = new Dictionary<string, int>( dicPresetsStatus );
			this.startingPreset = startingPreset;

			Rectangle rctScreen = Screen.FromHandle( hwnd ).Bounds;
			this.nudInitialW.Maximum = rctScreen.Width;
			this.nudInitialH.Maximum = rctScreen.Height;

			RECT rct;
			PInvoke_QTWM.GetWindowRect( hwnd, out rct );
			this.SetTitleText( rct );

			try
			{
				if( ( config[0] & 0x80 ) != 0 )
				{
					this.chbInitialSize.Checked = true;
				}

				if( ( config[0] & 0x40 ) != 0 )
				{
					this.checkBoxResizeMode.Checked = false;
				}

				if( ( config[0] & 0x20 ) != 0 )
				{
					this.chbInitialLoc.Checked = true;
				}

				if( ( config[0] & 0x10 ) != 0 )
				{
					this.chbStartingPreset.Checked = true;
				}

				//if( ( config[0] & 0x08 ) != 0 )
				//{
				//    this.chbRestoreClosedRct.Checked = true;
				//}


				this.nudInitialX.Value = rctInitial.X;
				this.nudInitialY.Value = rctInitial.Y;
				this.nudInitialW.Value = rctInitial.Width;
				this.nudInitialH.Value = rctInitial.Height;

				if( delta_RESIZE < 33 && delta_RESIZE > 0 )
					this.nudDelta.Value = delta_RESIZE;

				this.chbInitialLoc_CheckedChanged( null, EventArgs.Empty );
				this.chbInitialSize_CheckedChanged( null, EventArgs.Empty );

				if( this.chbStartingPreset.Checked )
				{
					this.nudInitialX.Enabled = this.nudInitialY.Enabled = this.chbInitialLoc.Enabled =
					this.buttonRestoreLoc.Enabled = this.buttonGetCurLoc.Enabled =
					this.buttonRestoreSize.Enabled = this.buttonGetCurSize.Enabled =
					this.nudInitialW.Enabled = this.nudInitialH.Enabled = this.chbInitialSize.Enabled = false;
				}
				else
				{
					this.cmbStartingPreset.Enabled = false;
				}
			}
			catch
			{
			}

			foreach( string name in this.dicPresets.Keys )
			{
				this.cmbPresets.Items.Add( name );
				this.cmbStartingPreset.Items.Add( name );
			}

			if( !String.IsNullOrEmpty( this.startingPreset ) )
			{
				int indexStartingPreset = this.cmbStartingPreset.Items.IndexOf( this.startingPreset );
				if( indexStartingPreset != -1 )
				{
					this.cmbStartingPreset.SelectedIndex = indexStartingPreset;
				}
			}

			if( this.cmbPresets.Items.Count > 0 )
			{
				this.cmbPresets.SelectedIndex = 0;
				int iFlag;
				this.dicPresetsStatus.TryGetValue( (string)this.cmbPresets.Items[0], out iFlag );

				this.SetPresetCheckBoxes( iFlag );
			}

			// server is desktop
			if( this.hwndExplorer == IntPtr.Zero )
			{
				this.btnApplyPreset.Enabled = this.buttonGetCurLoc.Enabled = 
				this.buttonGetCurrentToPreset.Enabled = this.buttonGetCurSize.Enabled = 
				this.buttonRestoreLoc.Enabled = this.buttonRestoreSize.Enabled = false;
			}
		}

		private void SetTitleText( RECT rct )
		{
			this.Text += " ( " + rct.left + ", " + rct.top + " ),  " + rct.Width + " x " + rct.Height;
		}

		private void SetPresetCheckBoxes( int iFlag )
		{
			if( iFlag == 1 )
			{
				this.chbPresetPosEnabled.Checked = true;
				this.chbPresetSizeEnabled.Checked = false;
			}
			else if( iFlag == 2 )
			{
				this.chbPresetPosEnabled.Checked = false;
				this.chbPresetSizeEnabled.Checked = true;
			}
			else
			{
				this.chbPresetPosEnabled.Checked = true;
				this.chbPresetSizeEnabled.Checked = true;
			}
		}

		public Point InitialLocation
		{
			get
			{
				return new Point( (int)this.nudInitialX.Value, (int)this.nudInitialY.Value );
			}
		}

		public Size InitialSize
		{
			get
			{
				return new Size( (int)this.nudInitialW.Value, (int)this.nudInitialH.Value );
			}
		}

		public int ResizeDelta
		{
			get
			{
				return (int)this.nudDelta.Value;
			}
		}

		public byte[] ConfigValues
		{
			get
			{
				byte[] config = new byte[] { 0, 0, 0, 0 };

				if( this.chbInitialSize.Checked )
					config[0] |= 0x80;

				if( !this.checkBoxResizeMode.Checked )
					config[0] |= 0x40;

				if( this.chbInitialLoc.Checked )
					config[0] |= 0x20;

				if( this.chbStartingPreset.Checked )
					config[0] |= 0x10;

				return config;
			}
		}

		public Dictionary<string, Rectangle> Presets
		{
			get
			{
				return this.dicPresets;
			}
		}

		public Dictionary<string, int> PresetsStatus
		{
			get
			{
				return this.dicPresetsStatus;
			}
		}

		public string StartingPreset
		{
			get
			{
				if( this.cmbStartingPreset.SelectedItem != null )
					return this.cmbStartingPreset.SelectedItem.ToString();
				else
					return String.Empty;
			}
		}



		private void chbInitialLoc_CheckedChanged( object sender, EventArgs e )
		{
			this.nudInitialX.Enabled = this.nudInitialY.Enabled =
			this.buttonRestoreLoc.Enabled = this.buttonGetCurLoc.Enabled =
			this.chbInitialLoc.Checked;
		}

		private void chbInitialSize_CheckedChanged( object sender, EventArgs e )
		{
			this.nudInitialW.Enabled = this.nudInitialH.Enabled = 
			this.buttonRestoreSize.Enabled = this.buttonGetCurSize.Enabled = 
			this.chbInitialSize.Checked;
		}

		private void chbStartingPreset_CheckedChanged( object sender, EventArgs e )
		{
			this.nudInitialX.Enabled = this.nudInitialY.Enabled = this.chbInitialLoc.Enabled =
			this.buttonRestoreLoc.Enabled = this.buttonGetCurLoc.Enabled =
			this.buttonRestoreSize.Enabled = this.buttonGetCurSize.Enabled =
			this.nudInitialW.Enabled = this.nudInitialH.Enabled = this.chbInitialSize.Enabled = !this.chbStartingPreset.Checked;

			this.cmbStartingPreset.Enabled = this.chbStartingPreset.Checked;
		}

		private void chbRestoreClosedRct_CheckedChanged( object sender, EventArgs e )
		{

		}


		private void buttonRestoreLoc_Click( object sender, EventArgs e )
		{
			if( this.hwndExplorer != IntPtr.Zero )
			{
				Point pnt = this.InitialLocation;

				PInvoke_QTWM.SetWindowPos( this.hwndExplorer, IntPtr.Zero, pnt.X, pnt.Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER );

				WindowManager.RemoveMAXIMIZE( this.hwndExplorer );
			}
		}

		private void buttonRestoreSize_Click( object sender, EventArgs e )
		{			
			if( this.hwndExplorer != IntPtr.Zero )
			{
				Size size = this.InitialSize;

				PInvoke_QTWM.SetWindowPos( this.hwndExplorer, IntPtr.Zero, 0, 0, size.Width, size.Height, SWP_NOMOVE | SWP_NOZORDER );

				WindowManager.RemoveMAXIMIZE( this.hwndExplorer );
			}
		}

		private void buttonGetCurLoc_Click( object sender, EventArgs e )
		{
			RECT rct;
			PInvoke_QTWM.GetWindowRect( this.hwndExplorer, out rct );

			this.nudInitialX.Value = rct.left;
			this.nudInitialY.Value = rct.top;

			this.SetTitleText( rct );
		}

		private void buttonGetCurSize_Click( object sender, EventArgs e )
		{
			RECT rct;
			PInvoke_QTWM.GetWindowRect( this.hwndExplorer, out rct );

			this.nudInitialW.Value = rct.Width;
			this.nudInitialH.Value = rct.Height;
		}

		private void buttonGetCurrentToPreset_Click( object sender, EventArgs e )
		{
			RECT rct;
			PInvoke_QTWM.GetWindowRect( this.hwndExplorer, out rct );

			this.nudPresets_X.Value = rct.left;
			this.nudPresets_Y.Value = rct.top;
			this.nudPresets_W.Value = rct.Width;
			this.nudPresets_H.Value = rct.Height;

			this.chbPresetPosEnabled.Checked = true;
			this.chbPresetSizeEnabled.Checked = true;
		}

		private void buttonSet_Click( object sender, EventArgs e )
		{
			if( this.cmbPresets.SelectedIndex != -1 )
			{
				if( this.cmbPresets.SelectedItem != null )
				{
					string name = this.cmbPresets.SelectedItem.ToString();

					this.dicPresets[name] = new Rectangle( (int)this.nudPresets_X.Value, (int)this.nudPresets_Y.Value, (int)this.nudPresets_W.Value, (int)this.nudPresets_H.Value );

					int iFlags = 0;
					if( !this.chbPresetPosEnabled.Checked )
					{
						iFlags = 2;
					}
					else if( !this.chbPresetSizeEnabled.Checked )
					{
						iFlags = 1;
					}
					this.dicPresetsStatus[name] = iFlags;

					if( !this.cmbStartingPreset.Items.Contains( name ) )
					{
						this.cmbStartingPreset.Items.Add( name );
					}
				}
			}
			else if( this.cmbPresets.Text.Length > 0 )
			{
				string name = this.cmbPresets.Text;

				this.dicPresets[name] = new Rectangle( (int)this.nudPresets_X.Value, (int)this.nudPresets_Y.Value, (int)this.nudPresets_W.Value, (int)this.nudPresets_H.Value );
				int iFlags = 0;
				if( !this.chbPresetPosEnabled.Checked )
				{
					iFlags = 2;
				}
				else if( !this.chbPresetSizeEnabled.Checked )
				{
					iFlags = 1;
				}
				this.dicPresetsStatus[name] = iFlags;

				this.cmbPresets.Items.Remove( name );
				this.cmbPresets.Items.Add( name );

				if( !this.cmbStartingPreset.Items.Contains( name ) )
				{
					this.cmbStartingPreset.Items.Add( name );
				}
			}
		}

		private void buttonDel_Click( object sender, EventArgs e )
		{
			if( this.cmbPresets.SelectedItem != null )
			{
				string name = this.cmbPresets.SelectedItem.ToString();
				this.dicPresets.Remove( name );
				this.dicPresetsStatus.Remove( name );

				this.cmbPresets.Items.Remove( this.cmbPresets.SelectedItem );
				this.cmbStartingPreset.Items.Remove( name );

				this.cmbPresets.Text = String.Empty;
			}
		}


		private void cmbPresets_SelectedIndexChanged( object sender, EventArgs e )
		{
			if( this.cmbPresets.SelectedItem != null )
			{
				string name = this.cmbPresets.SelectedItem.ToString();

				Rectangle rct;
				if( this.dicPresets.TryGetValue( name, out rct ) )
				{
					this.nudPresets_X.Value = rct.X;
					this.nudPresets_Y.Value = rct.Y;
					this.nudPresets_W.Value = rct.Width;
					this.nudPresets_H.Value = rct.Height;
				}

				int iFlags;
				this.dicPresetsStatus.TryGetValue( name, out iFlags );
				this.SetPresetCheckBoxes( iFlags );
			}
		}

		private void btnApplyPreset_Click( object sender, EventArgs e )
		{
			uint uFlags = ( this.chbPresetPosEnabled.Checked ? 0 : SWP_NOMOVE ) | ( this.chbPresetSizeEnabled.Checked ? 0 : SWP_NOSIZE ) | SWP_NOZORDER;

			PInvoke_QTWM.SetWindowPos( this.hwndExplorer, IntPtr.Zero, (int)this.nudPresets_X.Value, (int)this.nudPresets_Y.Value, (int)this.nudPresets_W.Value, (int)this.nudPresets_H.Value, uFlags );
		}

		private void chbPresetEnabled_CheckedChanged( object sender, EventArgs e )
		{
			if( !this.chbPresetPosEnabled.Checked && !this.chbPresetSizeEnabled.Checked )
			{
				if( sender == this.chbPresetPosEnabled )
				{
					this.chbPresetPosEnabled.Checked = true;		// this method will be called...
				}
				else
				{
					this.chbPresetSizeEnabled.Checked = true;		// this method will be called...
				}
			}
		}

	}
}