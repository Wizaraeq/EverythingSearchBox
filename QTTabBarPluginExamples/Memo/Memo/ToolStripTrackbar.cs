using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuizoPlugin
{
	sealed class ToolStripTrackBar : ToolStripControlHost
	{
		public event EventHandler ValueChanged;
		private bool fSuppressEvent;

		public ToolStripTrackBar()
			: base( new TrackBar() )
		{
			TrackBar t = (TrackBar)base.Control;
			t.MaximumSize = new Size( 100, 24 );

			t.Maximum = 255;
			t.Minimum = 20;
			t.Value = 255;
			t.SmallChange = 15;
			t.LargeChange = 26;
			t.TickFrequency = 26;

			// set background transparent ( Vista - )
			PInvoke.SetWindowLongPtr( t.Handle, GWL.STYLE, PInvoke.Ptr_OP_OR( PInvoke.GetWindowLongPtr( t.Handle, GWL.STYLE ), W32.TBS_TRANSPARENTBKGND ) );
		}

		public int Value
		{
			get
			{
				return ( (TrackBar)base.Control ).Value;
			}
		}

		public void SetValueWithoutEvent( int value )
		{
			TrackBar t = (TrackBar)base.Control;
			if( t.Minimum <= value && value <= t.Maximum )
			{
				this.fSuppressEvent = true;
				t.Value = value;
				this.fSuppressEvent = false;
			}
		}

		protected override void OnSubscribeControlEvents( Control control )
		{
			var trackBar = control as TrackBar;
			if( trackBar != null )
			{
				trackBar.ValueChanged += new EventHandler( OnValueChange );
				//trackBar.MouseMove += new MouseEventHandler( trackBar_MouseMove );
			}
			base.OnSubscribeControlEvents( control );
		}

		protected override void OnUnsubscribeControlEvents( Control control )
		{
			var trackBar = control as TrackBar;
			if( trackBar != null )
			{
				trackBar.ValueChanged -= new EventHandler( OnValueChange );
				//trackBar.MouseMove -= new MouseEventHandler( trackBar_MouseMove );
			}
			base.OnUnsubscribeControlEvents( control );
		}

		private void OnValueChange( object sender, EventArgs e )
		{
			if( !this.fSuppressEvent && this.ValueChanged != null )
			{
				this.ValueChanged( this, e );
			}
		}

		protected override bool ProcessCmdKey( ref Message m, Keys keyData )
		{
			if( this.Focused && ( keyData == Keys.Down || keyData == Keys.Up ) )
			{
				ToolStripItem item = this.Parent.GetNextItem( this, keyData == Keys.Down ? ArrowDirection.Down : ArrowDirection.Up );
				if( item != null )
				{
					// give focus to parent.
					this.Parent.Focus();

					// select next or previous item
					item.Select();
					return true;
				}
			}
			return base.ProcessCmdKey( ref m, keyData );
		}
	}
}