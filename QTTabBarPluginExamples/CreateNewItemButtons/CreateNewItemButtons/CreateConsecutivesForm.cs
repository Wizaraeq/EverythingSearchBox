using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using QTPlugin;

namespace QuizoPlugin
{
	partial class CreateConsecutivesForm : Form
	{
		private int iStart, iEnd;
		private bool fNowUpdatingNUD;
		private bool fNowUpdating = true;
		private string pathCurrent;
		private volatile bool fCancelPending;

		private const string PLACEHOLDER = "%n%";
		private const int COUNT_CONFIRM_MIN = 64;

		private IPluginServer pluginServer;
		private ITab tab;


		public CreateConsecutivesForm( string pathCurrent, IPluginServer pluginServer, string sel )
		{
			this.pluginServer = pluginServer;
			this.tab = pluginServer.SelectedTabInFocusedView;
			this.pathCurrent = pathCurrent.EndsWith( @"\" ) ? pathCurrent : pathCurrent + @"\";

			InitializeComponent();

			this.Text = this.pathCurrent;
			this.textBoxBaseName.Text = sel;

			if( CultureInfo.CurrentCulture.Parent.Name == "ja" )
			{
				string[] dialogStrs = Resource.dialog_ja.Split( new char[] { ';' } );

				this.labelBaseName.Text			= dialogStrs[0];
				this.labelStartVal.Text			= dialogStrs[1];
				this.labelEnd.Text				= dialogStrs[2];
				this.labelExt.Text				= dialogStrs[3];
				this.checkBoxAddZero.Text		= dialogStrs[4];
				this.checkBoxConfirm.Text		= dialogStrs[5];
				this.buttonSave.Text			= dialogStrs[6];
				this.buttonCancel.Text			= dialogStrs[7];
				this.textBoxBaseName.Text		= dialogStrs[8];
				this.checkBoxCloseOnCreate.Text = dialogStrs[9];
				this.chbSaveOnClose.Text		= dialogStrs[10];
			}

			this.toolTip1.SetToolTip( this.textBoxBaseName, Localizer.Tooltips[0] );
			this.toolTip1.SetToolTip( this.buttonCreateFolder, Localizer.Tooltips[1] );
			this.toolTip1.SetToolTip( this.buttonCreateEmptyFile, Localizer.Tooltips[2] );
			
			this.ReadSettings();
			this.fNowUpdating = false;
			this.CreateSampleFileName();
		}


		private void CreateConsecutivesForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			using( var rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\" + CreateConsecutiveFile.REGNAME ) )
			{
				if( rkPlugin != null )
				{
					var rct = this.WindowState != FormWindowState.Normal ? this.RestoreBounds : this.Bounds;

					rkPlugin.SetValue( "x", rct.X );
					rkPlugin.SetValue( "y", rct.Y );
				}
			}

			if( this.chbSaveOnClose.Checked )
			{
				this.SaveSettings();
			}
		}
		
		private void textBoxes_TextChanged( object sender, EventArgs e )
		{
			this.CreateSampleFileName();
		}

		private void numericUpDowns_ValueChanged( object sender, EventArgs e )
		{
			if( this.fNowUpdatingNUD )
			{
				return;
			}
			this.fNowUpdatingNUD = true;

			this.iStart = (int)this.numericUpDownStart.Value;
			this.iEnd = (int)this.numericUpDownEnd.Value;

			if( this.iEnd < this.iStart )
			{
				this.numericUpDownEnd.Value = this.iEnd = this.iStart;
			}
			else
			{
				this.numericUpDownStart.Value = this.iStart;
				this.numericUpDownEnd.Value = this.iEnd;
			}

			this.fNowUpdatingNUD = false;
			this.CreateSampleFileName();
		}

		private void checkBoxAddZero_CheckedChanged( object sender, EventArgs e )
		{
			this.CreateSampleFileName();
		}

		private void buttonCreateFolder_Click( object sender, EventArgs e )
		{
			if( this.iEnd - this.iStart + 1 < COUNT_CONFIRM_MIN || !this.checkBoxConfirm.Checked || ConfirmTooManyFile( this.iEnd - this.iStart + 1 ) )
			{
				this.CreateFiles( false );
			}
		}

		private void buttonCreateEmptyFile_Click( object sender, EventArgs e )
		{
			if( this.iEnd - this.iStart + 1 < COUNT_CONFIRM_MIN || !this.checkBoxConfirm.Checked || ConfirmTooManyFile( this.iEnd - this.iStart + 1 ) )
			{
				this.CreateFiles( true );
			}
		}

		private void buttonSave_Click( object sender, EventArgs e )
		{
			this.SaveSettings();
		}

		private void buttonCancel_Click( object sender, EventArgs e )
		{
			this.Close();
		}

		private void textBoxes_KeyPress( object sender, KeyPressEventArgs e )
		{
			if( !IsValidFileNameChar( e.KeyChar ) )//&& e.KeyChar != (char)Keys.Back )
			{
				e.Handled = true;
				System.Media.SystemSounds.Hand.Play();
			}
		}


		private static T GetValueSafe<T>( RegistryKey rk, string valName, T defaultVal )
		{
			object val = rk.GetValue( valName, defaultVal );

			if( val != null && val is T )
			{
				return (T)val;
			}
			else
			{
				return defaultVal;
			}
		}



		private void ReadSettings()
		{
			using( var rkPlugin = Registry.CurrentUser.OpenSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\" + CreateConsecutiveFile.REGNAME ) )
			{
				if( rkPlugin != null )
				{					
					string strBaseName = GetValueSafe<string>( rkPlugin, "BaseName", String.Empty );
					string strExt =		 GetValueSafe<string>( rkPlugin, "Ext", String.Empty );
					int x = GetValueSafe<int>( rkPlugin, "x", -1 );
					int y = GetValueSafe<int>( rkPlugin, "y", -1 );


					this.textBoxBaseName.Text = strBaseName;
					this.textBoxExt.Text = strExt;

					this.iStart = GetValueSafe<int>( rkPlugin, "iStart", 0 );
					this.iEnd	= GetValueSafe<int>( rkPlugin, "iEnd", 9 );

					this.fNowUpdatingNUD = true;
					this.numericUpDownEnd.Value = this.iEnd;
					this.numericUpDownStart.Value = this.iStart;
					this.fNowUpdatingNUD = false;

					this.checkBoxAddZero.Checked = GetValueSafe<int>( rkPlugin, "AddZero", 1 ) == 1;
					this.checkBoxConfirm.Checked = GetValueSafe<int>( rkPlugin, "Confirm", 1 ) == 1;
					this.checkBoxCloseOnCreate.Checked = GetValueSafe<int>( rkPlugin, "CloseOnCreate", 0 ) == 1;
					this.chbSaveOnClose.Checked = GetValueSafe<int>( rkPlugin, "SaveOnClose", 0 ) == 1;
					


					if( x != -1 && y != -1 )
					{
						this.Location = new Point( x, y );
					}
					else
					{
						this.StartPosition = FormStartPosition.CenterScreen;
					}

					using( var rkACSource = rkPlugin.OpenSubKey( "AutoComplete", false ) )
					{
						if( rkACSource != null )
						{
							var lst = new List<string>();

							foreach( var valName in rkACSource.GetValueNames() )
							{
								var val = GetValueSafe<string>( rkACSource, valName, String.Empty );
								if( !val.Contains( val ) )
								{
									lst.Add( val );
								}
							}

							var source = new AutoCompleteStringCollection();
							source.AddRange( lst.ToArray() );
							this.textBoxBaseName.AutoCompleteCustomSource = source;
						}
					}
				}
				else
				{
					this.StartPosition = FormStartPosition.CenterScreen;
				}
			}
		}

		private void SaveSettings()
		{
			using( var rkPlugin = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\" + CreateConsecutiveFile.REGNAME ) )
			{
				if( rkPlugin != null )
				{
					rkPlugin.SetValue( "AddZero", this.checkBoxAddZero.Checked ? 1 : 0 );
					rkPlugin.SetValue( "BaseName", this.textBoxBaseName.Text );
					rkPlugin.SetValue( "Confirm", this.checkBoxConfirm.Checked ? 1 : 0 );
					rkPlugin.SetValue( "CloseOnCreate", this.checkBoxCloseOnCreate.Checked ? 1 : 0 );
					rkPlugin.SetValue( "SaveOnClose", this.chbSaveOnClose.Checked ? 1 : 0 );
					rkPlugin.SetValue( "Ext", this.textBoxExt.Text );
					rkPlugin.SetValue( "iStart", this.iStart );
					rkPlugin.SetValue( "iEnd", this.iEnd );
				}
			}

			this.SaveAutoCompleteSource();
		}

		private void SaveAutoCompleteSource()
		{
			using( var rkACSource = Registry.CurrentUser.CreateSubKey( CONSTANTS.REGISTRY_PLUGINSETTINGS + @"\Quizo\" + CreateConsecutiveFile.REGNAME + @"\AutoComplete" ) )
			{
				if( rkACSource != null )
				{
					foreach( var valName in rkACSource.GetValueNames() )
					{
						rkACSource.DeleteValue( valName );
					}
					int i = 0;
					foreach( string item in this.textBoxBaseName.AutoCompleteCustomSource )
					{
						rkACSource.SetValue( ( i++ ).ToString(), item );
					}
				}
			}
		}


		private void CreateFiles( bool fFile )
		{
			try
			{
				if( Directory.Exists( this.pathCurrent ) )
				{
					List<string> lstExisted = new List<string>();
					List<string> lstError = new List<string>();
					List<string> lstSuccess = new List<string>();

					string strBaseName = this.MakeFileNameFormat();

					for( int i = this.iStart; i < this.iEnd + 1; i++ )
					{
						string strFileName = String.Format( strBaseName, i );
						if( fFile )
						{
							strFileName += this.MakeExt();
						}

						string strNewPath = this.pathCurrent + strFileName;
						try
						{
							if( ( fFile && File.Exists( strNewPath ) ) || ( !fFile && Directory.Exists( strNewPath ) ) )
							{
								lstExisted.Add( strFileName );
								continue;
							}

							if( fFile )
							{
								using( File.Create( strNewPath ) )
								{
								}
							}
							else
							{
								Directory.CreateDirectory( strNewPath );
							}
						}
						catch
						{
							lstError.Add( strNewPath );
							continue;
						}

						lstSuccess.Add( strNewPath );
					}

					if( lstSuccess.Count > 0 )
					{
						this.pluginServer.ExecuteCommand( Commands.RefreshBrowser, null );
						//this.pluginServer.TrySetSelection( lstSuccess.ToArray(), true );
						if( this.tab != null )
						{
							this.tab.SelectedPaths = lstSuccess;
						}

						this.textBoxBaseName.AutoCompleteCustomSource.Add( this.textBoxBaseName.Text );
						this.SaveAutoCompleteSource();
					}

					if( lstExisted.Count > 0 )
					{
						string strMsg = String.Empty;
						foreach( string s in lstExisted )
						{
							strMsg += s + "\r\n";
						}

						MessageBox.Show( Localizer.messages[0] + strMsg, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information );
					}
					if( lstError.Count > 0 )
					{
						string strMsg = String.Empty;
						foreach( string s in lstError )
						{
							strMsg += s + "\r\n";
						}

						MessageBox.Show( Localizer.messages[1] + strMsg, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error );
					}

					if( lstSuccess.Count > 0 && this.checkBoxCloseOnCreate.Checked )
					{
						this.Close();
					}
				}
				else
				{
					MessageBox.Show( Localizer.messages[3], String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error );
				}
			}
			catch
			{
			}
		}

		private string MakeFileNameFormat()
		{
			string strBaseName = this.textBoxBaseName.Text;
			strBaseName = strBaseName.Replace( "{", "{{" );
			strBaseName = strBaseName.Replace( "}", "}}" );

			string strFormat = String.Empty;

			if( this.checkBoxAddZero.Checked )
			{
				for( int i = 0; i < this.iEnd.ToString().Length; i++ )
				{
					strFormat += "0";
				}
				strFormat = "{0:" + strFormat + "}";
			}
			else
			{
				strFormat = "{0}";
			}

			if( strBaseName.Contains( PLACEHOLDER ) )
			{
				strBaseName = strBaseName.Replace( PLACEHOLDER, strFormat );
			}
			else
			{
				strBaseName += strFormat;
			}
			return strBaseName;
		}

		private string MakeExt()
		{
			var ext = this.textBoxExt.Text;
			if( !String.IsNullOrEmpty( ext ) && !ext.StartsWith( "." ) )
			{
				ext = "." + ext;
			}
			return ext;
		}

		private static bool ConfirmTooManyFile( int c )
		{
			return DialogResult.OK == MessageBox.Show( String.Format( Localizer.messages[2], c ), "", MessageBoxButtons.OKCancel );
		}

		private void CreateSampleFileName()
		{
			if( this.fNowUpdating )
			{
				return;
			}

			if( this.textBoxBaseName.Text.Length > 0 )
			{
				string strBaseName = this.MakeFileNameFormat();
				this.textBoxInfo.Text = String.Format( strBaseName, this.iStart ) + MakeExt() + "  ~ \r\n" + String.Format( strBaseName, this.iEnd ) + MakeExt() + "\r\n\t" + ( this.iEnd - this.iStart + 1 ) + " items.";
			}
			else
			{
				this.textBoxInfo.Text = this.iStart + MakeExt() + "  ~ \r\n" + this.iEnd + MakeExt() + "\r\n\t" + ( this.iEnd - this.iStart + 1 ) + " items.";
			}
		}


		private static string SanitizeFileNameString( string path )
		{
			if( path == null )
				return null;

			path = path.Trim();

			StringBuilder sb = new StringBuilder( path.Length );
			for( int i = 0; i < path.Length; i++ )
			{
				if( IsValidFileNameChar( path[i] ) )
				{
					sb.Append( path[i] );
				}
			}
			return sb.ToString();
		}

		internal static bool IsValidFileNameChar( char ch )
		{
			// ", <, >, |, \, /, :, *, ? 

			return ( ch != '"' ) && ( ch != '<' ) && ( ch != '>' ) && ( ch != '|' ) && ( ch != '\\' ) && ( ch != '/' ) && ( ch != ':' ) && ( ch != '*' ) && ( ch != '?' );	//&& ( ch > 0x1f )
		}

	}
}