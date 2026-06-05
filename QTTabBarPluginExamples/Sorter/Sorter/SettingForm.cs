using System;
using System.Windows.Forms;

namespace Sorter
{
	public partial class SettingForm : Form
	{
		public SettingForm( int data )
		{
			InitializeComponent();

			this.checkedListBox1.Items.AddRange( StringResources.ColumnNames );
			this.label1.Text = StringResources.SettingForm[0];
			this.Font = Sorter.CreateDefaultFont();

			if( this.checkedListBox1.Items.Count > 32 )
			{
				throw new InvalidOperationException();
			}

			int bit = 1;
			for( int i = 0; i < this.checkedListBox1.Items.Count; i++ )
			{
				if( ( data & bit ) != 0 )
				{
					this.checkedListBox1.SetItemChecked( i, true );
				}
				bit <<= 1;
			}
		}

		public int[] CheckedIndices
		{
			get
			{
				int[] arr = new int[this.checkedListBox1.CheckedIndices.Count];
				if( arr.Length > 32 )
				{
					throw new InvalidOperationException();
				}

				this.checkedListBox1.CheckedIndices.CopyTo( arr, 0 );

				return arr;
			}
		}
	}
}
