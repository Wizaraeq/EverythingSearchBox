using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuizoPlugins
{
	public partial class Form_SelectionInfoSB : Form
	{
		public Form_SelectionInfoSB()
		{
			InitializeComponent();

			this.chbSize.Text = SelectionInfoToStatusBar.PropertyDisplayNames[0];
			this.chbMod.Text = SelectionInfoToStatusBar.PropertyDisplayNames[1];
			this.chbCrt.Text = SelectionInfoToStatusBar.PropertyDisplayNames[2];
			this.chbAtt.Text = SelectionInfoToStatusBar.PropertyDisplayNames[3];
			this.chbDrive.Text = SelectionInfoToStatusBar.PropertyDisplayNames[4];
			this.chbCurFolder.Text = SelectionInfoToStatusBar.PropertyDisplayNames[5];
			this.chbSizInByte.Text = SelectionInfoToStatusBar.PropertyDisplayNames[6];
			this.chbFolderSize.Text = SelectionInfoToStatusBar.PropertyDisplayNames[7];
			this.chbNetwork.Text = SelectionInfoToStatusBar.PropertyDisplayNames[8];
			

			var kinds = SelectionInfoToStatusBar.ReadSettings();
			this.chbSize.Checked = kinds.HasFlag( StatusInfoKind.Size );
			this.chbMod.Checked = kinds.HasFlag( StatusInfoKind.Modified );
			this.chbCrt.Checked = kinds.HasFlag( StatusInfoKind.Created );
			this.chbAtt.Checked = kinds.HasFlag( StatusInfoKind.Attributes );
			this.chbDrive.Checked = kinds.HasFlag( StatusInfoKind.DriveInfo );
			this.chbSizInByte.Checked = kinds.HasFlag( StatusInfoKind.SizeInByte );
			this.chbSizInByte.Enabled = this.chbSize.Checked;
			this.chbFolderSize.Checked = !kinds.HasFlag( StatusInfoKind.NoFolderSize );
			this.chbFolderSize.Enabled = this.chbSize.Checked;
			this.chbNetwork.Checked = kinds.HasFlag( StatusInfoKind.Network );
			this.chbCurFolder.Checked = kinds.HasFlag( StatusInfoKind.FolderInfo );
		}

		private void btnOK_Click( object sender, EventArgs e )
		{
			StatusInfoKind kinds = 0;
			if( this.chbSize.Checked )
			{
				kinds |= StatusInfoKind.Size;
			}
			if( this.chbMod.Checked )
			{
				kinds |= StatusInfoKind.Modified;
			}
			if( this.chbCrt.Checked )
			{
				kinds |= StatusInfoKind.Created;
			}
			if( this.chbAtt.Checked )
			{
				kinds |= StatusInfoKind.Attributes;
			}
			if( this.chbDrive.Checked )
			{
				kinds |= StatusInfoKind.DriveInfo;
			}
			if( this.chbNetwork.Checked )
			{
				kinds |= StatusInfoKind.Network;
			}
			if( this.chbSizInByte.Enabled && this.chbSizInByte.Checked )
			{
				kinds |= StatusInfoKind.SizeInByte;
			}
			if( this.chbFolderSize.Enabled && !this.chbFolderSize.Checked )
			{
				kinds |= StatusInfoKind.NoFolderSize;
			}
			if( this.chbCurFolder.Checked )
			{
				kinds |= StatusInfoKind.FolderInfo;
			}

			SelectionInfoToStatusBar.InfoKind = kinds;
			SelectionInfoToStatusBar.SaveSettings();

			this.DialogResult = DialogResult.OK;
		}

		private void chbFolSize_CheckedChanged( object sender, EventArgs e )
		{
			this.chbSizInByte.Enabled =
			this.chbFolderSize.Enabled = this.chbSize.Checked;
		}
	}
}