using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace QuizoPlugins
{
	sealed class ToolStripSpacerItem : ToolStripControlHost
	{
		private const int MAXWIDTH = 1024;
		private const int GRIPWIDTH = 12;
		private const int LBLSIZE_LARGE = 32;
		private const int LBLSIZE_SMALL = 24;
		private const int SPRING_MARGIN_FAR = 6;

		private LabelEx lbl;
		private ToolStrip toolStrip;
		private bool fNowDragging;
		private bool fSpring;
		private int MINWIDTH;

		public event EventHandler ResizeComplete;


		public ToolStripSpacerItem( bool fLarge, int width )
			: base( CreateControlInstance( fLarge, width ) )
		{
			this.MINWIDTH = fLarge ? 18 : 12;
			this.AutoSize = false;
			this.lbl = (LabelEx)base.Control;
		}


		private static Control CreateControlInstance( bool fLarge, int width )
		{
			LabelEx lbl = new LabelEx();
			lbl.AutoSize = false;
			lbl.BackColor = Color.Transparent;
			lbl.Size = new Size( width, fLarge ? LBLSIZE_LARGE : LBLSIZE_SMALL );
			return lbl;
		}

		protected override void OnMouseMove( MouseEventArgs e )
		{
			if( this.IsMouseOnTheEdge( e.Location ) )
			{
				this.lbl.Cursor = Cursors.VSplit;

				if( e.Button == MouseButtons.Left )
					this.StartDrag( true );
			}
			else
			{
				this.lbl.Cursor = Cursors.Default;
			}

			if( this.fNowDragging )
			{
				if( MINWIDTH <= e.X && e.X <= MAXWIDTH )
				{
					int w = MINWIDTH;
					ToolStrip ts = base.Owner;
					if( ts != null && !ts.Disposing && !( ts is ToolStripOverflow ) )
					{
						w = ts.DisplayRectangle.Width - this.Bounds.X - 24;
					}

					this.Width = Math.Min( e.X + 12, w );
				}
			}
			base.OnMouseMove( e );
		}

		protected override void OnMouseDown( MouseEventArgs e )
		{
			if( this.IsMouseOnTheEdge( e.Location ) && e.Button == MouseButtons.Left )
			{
				this.StartDrag( true );
			}
			base.OnMouseDown( e );
		}

		protected override void OnMouseUp( MouseEventArgs mevent )
		{
			this.StartDrag( false );
			base.OnMouseUp( mevent );
		}

		protected override void OnMouseLeave( EventArgs e )
		{
			this.fNowDragging = false;
			base.OnMouseLeave( e );
		}

		protected override void OnBoundsChanged()
		{
			base.OnBoundsChanged();

			if( base.Parent != null && !base.Parent.Disposing )
				base.Parent.Refresh();
		}

		protected override void OnOwnerChanged( EventArgs e )
		{
			base.OnOwnerChanged( e );

			if( this.toolStrip == null )
			{
				if( this.Owner != null && !( this.Owner is ToolStripOverflow ) && !this.Owner.Disposing )
				{
					// Here we start to listen to Resize event.
					// Resizing SearchBox of ButtonBar raises parent ToolStrip Resize event.

					this.toolStrip = this.Owner;
					this.toolStrip.Resize += new EventHandler( toolStrip_Resize );
				}
			}
		}

		protected override void Dispose( bool disposing )
		{
			if( this.toolStrip != null )
			{
				this.toolStrip.Resize -= this.toolStrip_Resize;
				this.toolStrip = null;
			}
			base.Dispose( disposing );
		}


		private void StartDrag( bool fStart )
		{
			this.fNowDragging = fStart;

			if( !fStart && this.ResizeComplete != null )
				this.ResizeComplete( this, EventArgs.Empty );
		}

		private bool IsMouseOnTheEdge( Point pnt )
		{
			return pnt.X > this.lbl.Width - GRIPWIDTH;
		}

		public void RefreshWidth()
		{
			if( this.toolStrip != null && !this.toolStrip.Disposing )
			{
				// this must be done when this is in overflow.

				int w = 0;

				foreach( ToolStripItem item in this.toolStrip.Items )
				{
					if( item != this )
					{
						w += item.Width;
					}
				}

				int widthIdeal = this.toolStrip.ClientSize.Width - w - SPRING_MARGIN_FAR;
				this.Width = Math.Max( widthIdeal, MINWIDTH );
				this.toolStrip.PerformLayout();
			}
		}

		private void toolStrip_Resize( object sender, EventArgs e )
		{
			if( this.fSpring )
			{
				this.RefreshWidth();
			}
		}

		public bool Spring
		{
			get
			{
				return this.fSpring;
			}
			set
			{
				bool fChanged = value != this.fSpring;
				this.fSpring = value;
				if( fChanged && this.fSpring )
				{
					this.RefreshWidth();
				}
			}
		}


		sealed class LabelEx : Label
		{
			private bool fMouseEntered;

			protected override void OnMouseEnter( EventArgs e )
			{
				base.OnMouseEnter( e );
				this.fMouseEntered = true;
				this.Invalidate();
			}

			protected override void OnMouseLeave( EventArgs e )
			{
				base.OnMouseLeave( e );
				this.fMouseEntered = false;
				this.Invalidate();
			}

			protected override void OnPaint( PaintEventArgs e )
			{
				base.OnPaint( e );

				if( this.fMouseEntered )
				{
					Rectangle rct = new Rectangle( this.Width - 12, 1, 6, this.Height - 2 );

					if( VisualStyleRenderer.IsSupported )
					{
						new VisualStyleRenderer( VisualStyleElement.Rebar.Gripper.Normal ).DrawBackground( e.Graphics, rct );
					}
					else
					{
						ControlPaint.DrawSizeGrip( e.Graphics, Color.Transparent, rct );
					}
				}
			}
		}
	}

}