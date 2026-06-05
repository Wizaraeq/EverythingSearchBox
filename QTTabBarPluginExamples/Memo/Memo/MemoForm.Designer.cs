namespace QuizoPlugin
{
	partial class MemoForm
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing )
		{
			this.owner = null;

			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiCut = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiColor = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiChooseColor = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDefaultColor = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiFont = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiFontLarger = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiFontSmaller = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiChooseFont = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDefaultFont = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiSearch = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiTopMost = new System.Windows.Forms.ToolStripMenuItem();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tbSearch = new System.Windows.Forms.TextBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox1.AcceptsTab = true;
			this.richTextBox1.AutoWordSelection = true;
			this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.HideSelection = false;
			this.richTextBox1.ImeMode = System.Windows.Forms.ImeMode.On;
			this.richTextBox1.Location = new System.Drawing.Point(8, 8);
			this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.richTextBox1.Size = new System.Drawing.Size(224, 203);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.TabStop = false;
			this.richTextBox1.Text = "";
			this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
			this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCut,
            this.tsmiCopy,
            this.tsmiPaste,
            this.tsmiDelete,
            this.toolStripSeparator2,
            this.tsmiColor,
            this.tsmiFont,
            this.toolStripSeparator1,
            this.tsmiSearch,
            this.tsmiTopMost});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.ShowCheckMargin = true;
			this.contextMenuStrip1.ShowImageMargin = false;
			this.contextMenuStrip1.Size = new System.Drawing.Size(200, 192);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
			// 
			// tsmiCut
			// 
			this.tsmiCut.Name = "tsmiCut";
			this.tsmiCut.Size = new System.Drawing.Size(199, 22);
			this.tsmiCut.Text = "Cu&t";
			// 
			// tsmiCopy
			// 
			this.tsmiCopy.Name = "tsmiCopy";
			this.tsmiCopy.Size = new System.Drawing.Size(199, 22);
			this.tsmiCopy.Text = "&Copy";
			// 
			// tsmiPaste
			// 
			this.tsmiPaste.Name = "tsmiPaste";
			this.tsmiPaste.Size = new System.Drawing.Size(199, 22);
			this.tsmiPaste.Text = "&Paste";
			// 
			// tsmiDelete
			// 
			this.tsmiDelete.Name = "tsmiDelete";
			this.tsmiDelete.Size = new System.Drawing.Size(199, 22);
			this.tsmiDelete.Text = "&Delete";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(196, 6);
			// 
			// tsmiColor
			// 
			this.tsmiColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChooseColor,
            this.tsmiDefaultColor});
			this.tsmiColor.Name = "tsmiColor";
			this.tsmiColor.Size = new System.Drawing.Size(199, 22);
			this.tsmiColor.Text = "Co&lor";
			// 
			// tsmiChooseColor
			// 
			this.tsmiChooseColor.Name = "tsmiChooseColor";
			this.tsmiChooseColor.Size = new System.Drawing.Size(160, 22);
			this.tsmiChooseColor.Text = "&Choose color...";
			// 
			// tsmiDefaultColor
			// 
			this.tsmiDefaultColor.Name = "tsmiDefaultColor";
			this.tsmiDefaultColor.Size = new System.Drawing.Size(160, 22);
			this.tsmiDefaultColor.Text = "&Default color";
			// 
			// tsmiFont
			// 
			this.tsmiFont.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFontLarger,
            this.tsmiFontSmaller,
            this.tsmiChooseFont,
            this.tsmiDefaultFont});
			this.tsmiFont.Name = "tsmiFont";
			this.tsmiFont.Size = new System.Drawing.Size(199, 22);
			this.tsmiFont.Text = "&Font";
			// 
			// tsmiFontLarger
			// 
			this.tsmiFontLarger.Name = "tsmiFontLarger";
			this.tsmiFontLarger.ShortcutKeyDisplayString = "Ctrl + \'+\'";
			this.tsmiFontLarger.Size = new System.Drawing.Size(206, 22);
			this.tsmiFontLarger.Text = "&Larger font";
			// 
			// tsmiFontSmaller
			// 
			this.tsmiFontSmaller.Name = "tsmiFontSmaller";
			this.tsmiFontSmaller.ShortcutKeyDisplayString = "Ctrl + \'-\'";
			this.tsmiFontSmaller.Size = new System.Drawing.Size(206, 22);
			this.tsmiFontSmaller.Text = "&Smaller font";
			// 
			// tsmiChooseFont
			// 
			this.tsmiChooseFont.Name = "tsmiChooseFont";
			this.tsmiChooseFont.Size = new System.Drawing.Size(206, 22);
			this.tsmiChooseFont.Text = "Choose font...";
			// 
			// tsmiDefaultFont
			// 
			this.tsmiDefaultFont.Name = "tsmiDefaultFont";
			this.tsmiDefaultFont.Size = new System.Drawing.Size(206, 22);
			this.tsmiDefaultFont.Text = "&Default font";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(196, 6);
			// 
			// tsmiSearch
			// 
			this.tsmiSearch.Name = "tsmiSearch";
			this.tsmiSearch.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
			this.tsmiSearch.Size = new System.Drawing.Size(199, 22);
			this.tsmiSearch.Text = "&Search";
			// 
			// tsmiTopMost
			// 
			this.tsmiTopMost.Name = "tsmiTopMost";
			this.tsmiTopMost.Size = new System.Drawing.Size(199, 22);
			this.tsmiTopMost.Text = "Al&ways on top";
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listView1.FullRowSelect = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.Location = new System.Drawing.Point(11, 56);
			this.listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.ShowGroups = false;
			this.listView1.ShowItemToolTips = true;
			this.listView1.Size = new System.Drawing.Size(218, 151);
			this.listView1.TabIndex = 1;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Path";
			this.columnHeader1.Width = 93;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Text";
			this.columnHeader2.Width = 500;
			// 
			// tbSearch
			// 
			this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbSearch.Location = new System.Drawing.Point(11, 24);
			this.tbSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tbSearch.Name = "tbSearch";
			this.tbSearch.Size = new System.Drawing.Size(137, 23);
			this.tbSearch.TabIndex = 2;
			this.tbSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearch_KeyPress);
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(154, 22);
			this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 26);
			this.btnSearch.TabIndex = 3;
			this.btnSearch.Text = "Search";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// MemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(240, 219);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.tbSearch);
			this.Controls.Add(this.btnSearch);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MemoForm";
			this.Padding = new System.Windows.Forms.Padding(8);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Memo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MemoForm_FormClosing);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}


		#endregion

		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.TextBox tbSearch;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ToolStripMenuItem tsmiCut;
		private System.Windows.Forms.ToolStripMenuItem tsmiPaste;
		private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
		private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem tsmiSearch;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem tsmiTopMost;
		private System.Windows.Forms.ToolStripMenuItem tsmiColor;
		private System.Windows.Forms.ToolStripMenuItem tsmiChooseColor;
		private System.Windows.Forms.ToolStripMenuItem tsmiDefaultColor;
		private System.Windows.Forms.ToolStripMenuItem tsmiFont;
		private System.Windows.Forms.ToolStripMenuItem tsmiFontLarger;
		private System.Windows.Forms.ToolStripMenuItem tsmiFontSmaller;
		private System.Windows.Forms.ToolStripMenuItem tsmiChooseFont;
		private System.Windows.Forms.ToolStripMenuItem tsmiDefaultFont;
	}
}