using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuizoPlugins
{
	public partial class OptionForm : Form
	{
		public OptionForm( List<string> lstTargetPaths, List<string> lstReservedPaths )
		{
			InitializeComponent();

			string[] strs = new Localizer().Strs;
			//One Key Send-To;Provides key shortcuts to send selected files to pre-defined directories.;Add;Remove selections;OK;Cancel
			this.buttonAdd.Text = strs[2];
			this.buttonRemove.Text = strs[3];
			this.btnOk.Text = strs[4];
			this.btnCancel.Text = strs[5];
			this.label1.Text = strs[6];

			this.listView1.BeginUpdate();
			foreach( string path in lstTargetPaths )
			{
				ListViewItem lvi = new ListViewItem( path );
				lvi.Checked = true;
				this.listView1.Items.Add( lvi );
			}
			foreach( string path in lstReservedPaths )
			{
				ListViewItem lvi = new ListViewItem( path );
				lvi.Checked = false;
				this.listView1.Items.Add( lvi );
			}
			this.listView1.EndUpdate();
		}

		private void btnBrowse_Click( object sender, EventArgs e )
		{
			using( FolderBrowserDialog fbd = new FolderBrowserDialog() )
			{
				if( !String.IsNullOrEmpty( this.tbTarget.Text ) )
				{
					fbd.SelectedPath = this.tbTarget.Text;
				}
				fbd.ShowNewFolderButton = true;

				if( DialogResult.OK == fbd.ShowDialog() )
				{
					this.tbTarget.Text = fbd.SelectedPath;
				}
			}
		}

		private void buttonAdd_Click( object sender, EventArgs e )
		{
			if( String.IsNullOrEmpty( this.tbTarget.Text ) )
			{
				this.btnBrowse.PerformClick();
			}

			if( !String.IsNullOrEmpty( this.tbTarget.Text ) )
			{
				ListViewItem lvi = new ListViewItem( this.tbTarget.Text );
				lvi.Checked = true;
				this.listView1.Items.Add( lvi );
			}
		}

		private void buttonRemove_Click( object sender, EventArgs e )
		{
			List<ListViewItem> lst = new List<ListViewItem>();
			foreach( ListViewItem lvi in this.listView1.SelectedItems )
			{
				lst.Add( lvi );
			}
			this.listView1.BeginUpdate();
			foreach( ListViewItem lvi in lst )
			{
				this.listView1.Items.Remove( lvi );
			}
			this.listView1.EndUpdate();
		}

		public List<string> Paths
		{
			get
			{
				List<string> lst = new List<string>();
				foreach( ListViewItem lvi in this.listView1.Items )
				{
					if( lvi.Checked )
					{
						lst.Add( lvi.Text );
					}	
				}
				return lst;
			}
		}

		public List<string> ReservedPaths
		{
			get
			{
				List<string> lst = new List<string>();
				foreach( ListViewItem lvi in this.listView1.Items )
				{
					if( !lvi.Checked )
					{
						lst.Add( lvi.Text );
					}
				}
				return lst;
			}
		}

		private void listView1_KeyDown( object sender, KeyEventArgs e )
		{
			switch( e.KeyData )
			{
				case Keys.Delete:

					this.listView1.BeginUpdate();
					while( this.listView1.SelectedItems.Count > 0 )
					{
						this.listView1.Items.Remove( this.listView1.SelectedItems[0] );
					}
						this.listView1.EndUpdate();
					break;
			}
		}

	}
}