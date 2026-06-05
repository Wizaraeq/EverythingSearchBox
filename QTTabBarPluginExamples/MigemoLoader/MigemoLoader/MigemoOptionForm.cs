using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuizoPlugins
{
	public partial class MigemoOptionForm : Form
	{
		public string pathDLL, pathDic;

		public MigemoOptionForm( string pathDLL, string pathDic, bool fPartialMatch, bool fDicIsUTF8 )
		{
			this.pathDLL = pathDLL;
			this.pathDic = pathDic;

			InitializeComponent();

			this.textBoxDLL.Text = this.pathDLL;
			this.textBoxDic.Text = this.pathDic;
			this.chbPerfectMatch.Checked = !fPartialMatch;
			this.chbUTF8.Checked = fDicIsUTF8;
		}

		private void button_Browse_Click( object sender, EventArgs e )
		{
			using( OpenFileDialog ofd = new OpenFileDialog() )
			{
				bool fDLL = sender == this.buttonBrowseDll;

				if( fDLL )
				{
					ofd.Filter = "Migemo dll file ( migemo.dll )|*.dll";
					ofd.FileName = this.pathDLL;
				}
				else
				{
					ofd.Filter = "Dictionary file ( migemo-dict )|*.*";
					ofd.FileName = this.pathDic;
				}

				if( DialogResult.OK == ofd.ShowDialog() )
				{
					if( fDLL )
					{
						this.textBoxDLL.Text = ofd.FileName;
					}
					else
					{
						this.textBoxDic.Text = ofd.FileName;
					}
				}
			}
		}

		private void buttonOK_Click( object sender, EventArgs e )
		{
			this.DialogResult = DialogResult.OK;
		}

		private void textBoxes_TextChanged( object sender, EventArgs e )
		{
			try
			{
				if( sender == this.textBoxDLL )
				{
					this.pathDLL = this.textBoxDLL.Text;
					this.textBoxDLL.ForeColor = File.Exists( this.pathDLL ) ? this.ForeColor : Color.Red;
				}
				else
				{
					this.pathDic = this.textBoxDic.Text;
					this.textBoxDic.ForeColor = File.Exists( this.pathDic ) ? this.ForeColor : Color.Red;
				}
			}
			catch
			{
			}
		}

		public bool PartialMatch
		{
			get
			{
				return !this.chbPerfectMatch.Checked;
			}
		}

		public bool UTF8Dictionary
		{
			get
			{
				return this.chbUTF8.Checked;
			}
		}
	}
}