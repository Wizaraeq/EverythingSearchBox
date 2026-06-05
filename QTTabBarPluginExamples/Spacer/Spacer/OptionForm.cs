using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuizoPlugins
{
	public partial class OptionForm : Form
	{
		public OptionForm( bool fTransparent, string strRes )
		{
			InitializeComponent();

			this.checkBox1.Checked = fTransparent;
			this.checkBox1.Text = strRes;
		}

		public bool  DoubleClickTransparent
		{
			get
			{
				return this.checkBox1.Checked;
			}
		}
	}
}
