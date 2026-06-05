using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QuizoPlugin
{
	partial class OptionForm : Form
	{
		public OptionForm( bool fEnabled, bool fShowOnHover )
		{
			InitializeComponent();

			this.rbShowIfExists.Text = Localizer.StringResources[1];
			this.rbShowAlways.Text = Localizer.StringResources[2];
			this.checkBox1.Text = Localizer.StringResources[6];

			if( fEnabled )
			{
				this.rbShowIfExists.Checked = true;
			}
			else
			{
				this.rbShowAlways.Checked = true;
			}

			this.checkBox1.Checked = fShowOnHover;
		}

		public MemoMode MemoMode
		{
			get
			{
				return this.rbShowIfExists.Checked ? MemoMode.Enabled : MemoMode.ShowAlways;
			}
		}

		public bool ShowOnHover
		{
			get
			{
				return this.checkBox1.Checked;
			}			
		}
	}
}