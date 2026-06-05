using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace QuizoPlugin
{
	sealed partial class MemoForm : Form
	{
		private const string VER = "128.0.0.2";	// update this on new release...

		private static string PATH_DAT = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\Quizo\QTTabBar\Plugins\Memo\memodata";
		private static string PATH_DAT_VER = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\Quizo\QTTabBar\Plugins\Memo\memodata" + VER;

		private dynamic memoStore;
		private bool fFirstLoadComplete;
		private string currentPath = String.Empty;
		private ToolStripTrackBar toolStripTrackBar;
		private List<Font> fontList = new List<Font>();
		private const int NUM_LVTEXTWIDTH = 100;
		private Memo owner;


		public MemoForm( Memo memo )
		{
			this.owner = memo;
			this.InitializeComponent();
			this.InitializeAfterDesigner();
			this.Localize();

			this.LoadData();
		}

		private void InitializeAfterDesigner()
		{
			this.contextMenuStrip1.Renderer = this.owner.menuRenderer;
			this.tsmiFontLarger.ShortcutKeyDisplayString = "Ctrl + '+'";
			this.tsmiFontSmaller.ShortcutKeyDisplayString = "Ctrl + '-'";
			
			this.toolStripTrackBar = new ToolStripTrackBar();
			this.toolStripTrackBar.AutoSize = true;
			this.toolStripTrackBar.ValueChanged += new EventHandler( toolStripTrackBar_ValueChanged );

			this.tsmiFont.DropDownItemClicked += new ToolStripItemClickedEventHandler( tsmiFont_DropDownItemClicked );
			this.tsmiColor.DropDownItemClicked += new ToolStripItemClickedEventHandler( tsmiColor_DropDownItemClicked );
		}

		private void Localize()
		{
			this.richTextBox1.Font = Localizer.CreateFont();
			this.richTextBox1.LanguageOption &= ~RichTextBoxLanguageOptions.DualFont;

			if( Localizer.fJa )
			{
				this.tsmiCut.Text = Localizer.MenuStrings[0];
				this.tsmiCopy.Text = Localizer.MenuStrings[1];
				this.tsmiPaste.Text = Localizer.MenuStrings[2];
				this.tsmiDelete.Text = Localizer.MenuStrings[3];
				this.tsmiColor.Text = Localizer.MenuStrings[4];
				this.tsmiFont.Text = Localizer.MenuStrings[5];
				this.tsmiChooseColor.Text = Localizer.MenuStrings[6];
				this.tsmiDefaultColor.Text = Localizer.MenuStrings[7];
				this.tsmiFontLarger.Text = Localizer.MenuStrings[8];
				this.tsmiFontSmaller.Text = Localizer.MenuStrings[9];
				this.tsmiChooseFont.Text = Localizer.MenuStrings[10];				
				this.tsmiDefaultFont.Text = Localizer.MenuStrings[11];
				this.tsmiSearch.Text = Localizer.MenuStrings[12];
				this.tsmiTopMost.Text = Localizer.MenuStrings[13];
			}
		}

		protected override bool ShowWithoutActivation
		{
			get
			{
				return true;
			}
		}

		
		public void ShowMemo( string path, string displayName )
		{
			try
			{
				if( !this.fFirstLoadComplete )
				{
					this.Bounds = Memo.Bounds;
					this.Opacity = Memo.Opacity;
					this.tsmiTopMost.Checked = Memo.AlwaysTopMost;
					this.fFirstLoadComplete = true;
				}

				if( this.Visible && this.richTextBox1.CanUndo )
				{
					this.UpdateDataStore();
				}

				this.currentPath = path;
				if( !String.IsNullOrEmpty( path ) )
				{
					this.Text = Localizer.StringResources[4] + " - " + displayName;

					var hash = EncryptionUtil.PathToHash( path );
					string rtf = String.Empty;

					// Set richTextBox data.
					this.richTextBox1.SuspendLayout();
					this.richTextBox1.Clear();
					if( this.memoStore.RichTextData.ContainsKey( hash ) && EncryptionUtil.TryDecrypt( this.memoStore.RichTextData[hash], hash, out rtf ) )
					{
						this.richTextBox1.Rtf = rtf;
					}
					this.richTextBox1.Visible = true;
					this.richTextBox1.ResumeLayout();

					PInvoke.SetWindowPos( this.Handle, (IntPtr)( this.tsmiTopMost.Checked ? -1 : 0 ), 0, 0, 0, 0, 0x13 );		// SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE
					this.Show();
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
		}

		public void HideMemo()
		{
			try
			{
				if( this.Visible )
				{
					Memo.Bounds = this.WindowState == FormWindowState.Minimized ? this.RestoreBounds : this.Bounds;
					Memo.Opacity = this.Opacity;

					this.Hide();
					if( this.richTextBox1.CanUndo )
					{
						this.UpdateDataStore();
					}
				}

				foreach( Font fnt in this.fontList )
				{
					fnt.Dispose();
				}
				this.fontList.Clear();
			}
			catch( Exception ex )
			{				
				MessageBox.Show( ex.ToString() );				
			}
		}

		public bool ContainsPath( string path )
		{
			return this.memoStore.RichTextData.ContainsKey( EncryptionUtil.PathToHash( path ) );
		}

		public void GiveFocus()
		{
			this.richTextBox1.Visible = true;
			this.richTextBox1.Focus();
		}

		private void EnlargeFont( bool fLarge, int step )
		{
			Font fntSel = this.richTextBox1.SelectionFont;
			if( fntSel == null )
			{
				fntSel = this.Font;
			}

			if( fLarge )
			{

				if( fntSel.Size < 45 )
				{
					Font fnt = new Font( this.Font.FontFamily, fntSel.Size + 0.75f * step );
					this.richTextBox1.SelectionFont = fnt;
					this.fontList.Add( fnt );
				}
			}
			else
			{
				if( fntSel.Size > 6 )
				{
					Font fnt = new Font( this.Font.FontFamily, fntSel.Size - 0.75f * step );
					this.richTextBox1.SelectionFont = fnt;
					this.fontList.Add( fnt );
				}
			}
		}


		private void LoadData()
		{
			// 128.0.0.2		now loads version dependent data file. and using dynamic type, can load old data...

			string path = PATH_DAT_VER;
			if( !File.Exists( path ) )
			{
				path = PATH_DAT;
				if( !File.Exists( path ) )
				{
					path = null;
				}
			}

			if( path != null )
			{
				AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler( CurrentDomain_AssemblyResolve );
				try
				{
					using( var fs = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ) )
					{
						this.memoStore = new BinaryFormatter().Deserialize( fs ) as dynamic;
					}
				}
				catch( Exception ex )
				{
					MessageBox.Show( ex.ToString() );
				}
				finally
				{
					AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler( CurrentDomain_AssemblyResolve );
				}
			}

			if( this.memoStore == null )
			{
				this.memoStore = new MemoStore();
			}
		}

		private void SaveData()
		{
			// 128.0.0.2		now saves version dependent data file. so old data will not be overwritten by new version...

			if( !Directory.Exists( Path.GetDirectoryName( PATH_DAT_VER ) ) )
			{
				Directory.CreateDirectory( Path.GetDirectoryName( PATH_DAT_VER ) );
			}

			using( var fs = new FileStream( MemoForm.PATH_DAT_VER, FileMode.Create, FileAccess.Write, FileShare.Read ) )
			{
				new BinaryFormatter().Serialize( fs, this.memoStore );
			}
		}

		private void UpdateDataStore()
		{
			var hash = EncryptionUtil.PathToHash( this.currentPath );
			if( this.richTextBox1.TextLength > 0 )
			{
				this.memoStore.RichTextData[hash] = EncryptionUtil.Encrypt( this.richTextBox1.Rtf, hash );
				this.memoStore.TextData[hash]	  = EncryptionUtil.Encrypt( this.richTextBox1.Text, hash );
				this.memoStore.Paths[hash]		  = EncryptionUtil.Encrypt( this.currentPath, hash );
			}
			else
			{
				this.memoStore.RichTextData.Remove( hash );
				this.memoStore.TextData.Remove( hash );
				this.memoStore.Paths.Remove( hash );
			}
			this.SaveData();
		}

		private void InitializeList( string str )
		{
			this.listView1.BeginUpdate();
			this.listView1.Items.Clear();

			foreach( var hash in this.memoStore.TextData.Keys )
			{
				string text, path = String.Empty;
				if( EncryptionUtil.TryDecrypt( this.memoStore.TextData[hash], hash, out text ) && ( String.IsNullOrEmpty( str ) || text.IndexOf( str, StringComparison.OrdinalIgnoreCase ) != -1 ) )
				{
					byte[] buffer;
					if( !this.memoStore.Paths.TryGetValue( hash, out buffer ) || !EncryptionUtil.TryDecrypt( buffer, hash, out path ) )
					{
						path = "Unknown path";
					}

					if( text.Length > NUM_LVTEXTWIDTH )
					{
						text = text.Substring( 0, NUM_LVTEXTWIDTH - 3 ) + "...";
					}
					ListViewItem lvi = new ListViewItem( new string[] { path, text } );
					lvi.ToolTipText = path;
					this.listView1.Items.Add( lvi );
				}
			}
			this.listView1.EndUpdate();
		}
		

		private void MemoForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			if( e.CloseReason == CloseReason.WindowsShutDown )
			{
				return;
			}

			e.Cancel = true;

			if( this.richTextBox1.Visible )
			{
				this.HideMemo();
			}
			else
			{
				this.richTextBox1.Visible = true;
				this.richTextBox1.Focus();
			}
		}
		
		private void contextMenuStrip1_Opening( object sender, CancelEventArgs e )
		{
			this.contextMenuStrip1.SuspendLayout();
			this.contextMenuStrip1.Items.Clear();

			if( this.richTextBox1.SelectionLength < 1 )
			{
				this.contextMenuStrip1.Items.AddRange( new ToolStripItem[] { this.tsmiCut, this.tsmiCopy, this.tsmiPaste, this.tsmiDelete, this.toolStripSeparator1, this.tsmiSearch, this.tsmiTopMost, this.toolStripTrackBar } );
				this.tsmiCut.Enabled = this.tsmiCopy.Enabled = this.tsmiDelete.Enabled = false;
				this.toolStripTrackBar.SetValueWithoutEvent( (int)( this.Opacity * 255 ) );
			}
			else
			{
				this.contextMenuStrip1.Items.AddRange( new ToolStripItem[] { this.tsmiCut, this.tsmiCopy, this.tsmiPaste, this.tsmiDelete, this.toolStripSeparator1, this.tsmiColor, this.tsmiFont, this.toolStripSeparator2, this.tsmiSearch, this.tsmiTopMost } );
				this.tsmiCut.Enabled = this.tsmiCopy.Enabled = this.tsmiDelete.Enabled = true;
			}
			
			this.contextMenuStrip1.ResumeLayout();
		}

		private void contextMenuStrip1_ItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			if( e.ClickedItem == this.tsmiCut )
			{
				this.richTextBox1.Cut();
			}
			else if( e.ClickedItem == this.tsmiCopy )
			{
				this.richTextBox1.Copy();
			}
			else if( e.ClickedItem == this.tsmiPaste )
			{
				this.richTextBox1.Paste();
			}
			else if( e.ClickedItem == this.tsmiDelete )
			{
				this.richTextBox1.SelectedText = String.Empty;
			}
			else if( e.ClickedItem == this.tsmiSearch )
			{
				if( this.richTextBox1.CanUndo )
				{
					this.UpdateDataStore();
				}

				this.InitializeList( null );
				this.richTextBox1.Visible = false;
				this.Refresh();
				this.tbSearch.Focus();
			}
			else if( e.ClickedItem == this.tsmiTopMost )
			{
				this.tsmiTopMost.Checked = !this.tsmiTopMost.Checked;
				Memo.AlwaysTopMost = this.tsmiTopMost.Checked;

				PInvoke.SetWindowPos( this.Handle, this.tsmiTopMost.Checked ? (IntPtr)( -1 ) : (IntPtr)(-2), 0, 0, 0, 0, 0x13 );
			}
		}

		private void tsmiColor_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			if( e.ClickedItem == this.tsmiChooseColor )
			{
				using( var cd = new ColorDialog() )
				{
					if( this.richTextBox1.SelectionColor != Color.Empty )
					{
						cd.Color = this.richTextBox1.SelectionColor;
					}
					cd.FullOpen = true;

					if( DialogResult.OK == cd.ShowDialog( this ) )
					{
						this.richTextBox1.SelectionColor = cd.Color;
					}
				}
			}
			else if( e.ClickedItem == this.tsmiDefaultColor )
			{
				this.richTextBox1.SelectionColor = SystemColors.WindowText;
			}
		}

		private void tsmiFont_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
		{
			if( e.ClickedItem == this.tsmiFontLarger )
			{
				this.EnlargeFont( true, 2 );
			}
			else if( e.ClickedItem == this.tsmiFontSmaller )
			{
				this.EnlargeFont( false, 2 );
			}
			else if( e.ClickedItem == this.tsmiDefaultFont )
			{
				this.richTextBox1.SelectionFont = this.richTextBox1.Font;
			}
			else if( e.ClickedItem == this.tsmiChooseFont )
			{
				using( var fd = new FontDialog() )
				{
					fd.Font = this.richTextBox1.SelectionFont;
					fd.Color = this.richTextBox1.SelectionColor;

					if( DialogResult.OK == fd.ShowDialog( this ) )
					{
						this.richTextBox1.SelectionFont = fd.Font;
						this.richTextBox1.SelectionColor = fd.Color;
					}
				}
			}
		}

		private void toolStripTrackBar_ValueChanged( object sender, EventArgs e )
		{
			this.Opacity = ( (double)this.toolStripTrackBar.Value ) / 255;
		}

		private void richTextBox1_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.Modifiers != Keys.Control )
			{
				return;
			}

			if( e.KeyCode == Keys.Add  )
			{
				this.EnlargeFont( true, 1 );
			}
			else if( e.KeyCode == Keys.Subtract )
			{
				this.EnlargeFont( false, 1 );
			}
			else if( e.KeyCode == Keys.F )
			{
				if( this.richTextBox1.CanUndo )
				{
					this.UpdateDataStore();
				}

				// Set listview's tooltip to topmost.
				const int LVM_FIRST = 0x1000;
				const int LVM_GETTOOLTIPS = ( LVM_FIRST + 78 );

				IntPtr hwndToolTip = PInvoke.SendMessage( this.listView1.Handle, LVM_GETTOOLTIPS, IntPtr.Zero, IntPtr.Zero );				
				if( hwndToolTip != IntPtr.Zero )
				{
					//HWND_TOPMOST    ((HWND)-1)
					//SWP_NOSIZE          0x0001
					//SWP_NOMOVE          0x0002
					//SWP_NOACTIVATE      0x0010
					PInvoke.SetWindowPos( hwndToolTip, new IntPtr( -1 ), 0, 0, 0, 0, 0x13u );
				}

				this.InitializeList( null );
				this.richTextBox1.Visible = false;
				this.Refresh();
				this.tbSearch.Focus();
			}
		}

		private void richTextBox1_LinkClicked( object sender, LinkClickedEventArgs e )
		{
			try
			{
				Process.Start( e.LinkText );
			}
			catch
			{
			}
		}

		private void listView1_ItemActivate( object sender, EventArgs e )
		{
			string path = this.listView1.SelectedItems[0].Text;
			if( Directory.Exists( path ) )
			{
				this.owner.OpenDirectory( path );
			}
		}

		private void tbSearch_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( e.KeyChar == (char)Keys.Enter )
			{
				e.Handled = true;
				this.InitializeList( this.tbSearch.Text );
			}
		}

		private void btnSearch_Click( object sender, EventArgs e )
		{
			this.InitializeList( this.tbSearch.Text );
		}

		private static System.Reflection.Assembly CurrentDomain_AssemblyResolve( object sender, ResolveEventArgs args )
		{
			return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault( ( asm ) => asm.FullName == "Memo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" );
		}
	}

	[Serializable]
	sealed class MemoStore
	{
		/// <summary>
		/// Rich Text Data.
		/// </summary>
		public Dictionary<string, byte[]> RichTextData = new Dictionary<string, byte[]>();
		/// <summary>
		/// Raw Text Data to seach strings.
		/// </summary>
		public Dictionary<string, byte[]> TextData = new Dictionary<string, byte[]>();
		/// <summary>
		/// Folder paths.
		/// </summary>
		public Dictionary<string, byte[]> Paths = new Dictionary<string, byte[]>();

		public MemoStore()
		{
		}
	}
}