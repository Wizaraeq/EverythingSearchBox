namespace QuizoPlugin
{
	partial class CreateConsecutivesForm
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
			this.textBoxBaseName = new System.Windows.Forms.TextBox();
			this.labelBaseName = new System.Windows.Forms.Label();
			this.labelStartVal = new System.Windows.Forms.Label();
			this.labelEnd = new System.Windows.Forms.Label();
			this.checkBoxAddZero = new System.Windows.Forms.CheckBox();
			this.buttonCreateFolder = new System.Windows.Forms.Button();
			this.buttonCreateEmptyFile = new System.Windows.Forms.Button();
			this.textBoxExt = new System.Windows.Forms.TextBox();
			this.labelExt = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			this.textBoxInfo = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.checkBoxConfirm = new System.Windows.Forms.CheckBox();
			this.numericUpDownStart = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownEnd = new System.Windows.Forms.NumericUpDown();
			this.checkBoxCloseOnCreate = new System.Windows.Forms.CheckBox();
			this.chbSaveOnClose = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownEnd)).BeginInit();
			this.SuspendLayout();
			// 
			// textBoxBaseName
			// 
			this.textBoxBaseName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxBaseName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.textBoxBaseName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.textBoxBaseName.Location = new System.Drawing.Point(115, 14);
			this.textBoxBaseName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.textBoxBaseName.Name = "textBoxBaseName";
			this.textBoxBaseName.Size = new System.Drawing.Size(373, 23);
			this.textBoxBaseName.TabIndex = 0;
			this.textBoxBaseName.Text = "New item ( %n% )";
			this.textBoxBaseName.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
			this.textBoxBaseName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxes_KeyPress);
			// 
			// labelBaseName
			// 
			this.labelBaseName.AutoSize = true;
			this.labelBaseName.Location = new System.Drawing.Point(14, 18);
			this.labelBaseName.Name = "labelBaseName";
			this.labelBaseName.Size = new System.Drawing.Size(64, 15);
			this.labelBaseName.TabIndex = 1;
			this.labelBaseName.Text = "Base name";
			// 
			// labelStartVal
			// 
			this.labelStartVal.AutoSize = true;
			this.labelStartVal.Location = new System.Drawing.Point(14, 47);
			this.labelStartVal.Name = "labelStartVal";
			this.labelStartVal.Size = new System.Drawing.Size(31, 15);
			this.labelStartVal.TabIndex = 4;
			this.labelStartVal.Text = "Start";
			// 
			// labelEnd
			// 
			this.labelEnd.AutoSize = true;
			this.labelEnd.Location = new System.Drawing.Point(14, 80);
			this.labelEnd.Name = "labelEnd";
			this.labelEnd.Size = new System.Drawing.Size(27, 15);
			this.labelEnd.TabIndex = 5;
			this.labelEnd.Text = "End";
			// 
			// checkBoxAddZero
			// 
			this.checkBoxAddZero.AutoSize = true;
			this.checkBoxAddZero.Checked = true;
			this.checkBoxAddZero.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxAddZero.Location = new System.Drawing.Point(255, 46);
			this.checkBoxAddZero.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxAddZero.Name = "checkBoxAddZero";
			this.checkBoxAddZero.Size = new System.Drawing.Size(94, 19);
			this.checkBoxAddZero.TabIndex = 4;
			this.checkBoxAddZero.Text = "Fill with Zero";
			this.checkBoxAddZero.UseVisualStyleBackColor = true;
			this.checkBoxAddZero.CheckedChanged += new System.EventHandler(this.checkBoxAddZero_CheckedChanged);
			// 
			// buttonCreateFolder
			// 
			this.buttonCreateFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCreateFolder.Image = global::QuizoPlugin.Resource.folder;
			this.buttonCreateFolder.Location = new System.Drawing.Point(257, 149);
			this.buttonCreateFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCreateFolder.Name = "buttonCreateFolder";
			this.buttonCreateFolder.Size = new System.Drawing.Size(56, 56);
			this.buttonCreateFolder.TabIndex = 7;
			this.buttonCreateFolder.UseVisualStyleBackColor = true;
			this.buttonCreateFolder.Click += new System.EventHandler(this.buttonCreateFolder_Click);
			// 
			// buttonCreateEmptyFile
			// 
			this.buttonCreateEmptyFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCreateEmptyFile.Image = global::QuizoPlugin.Resource.text;
			this.buttonCreateEmptyFile.Location = new System.Drawing.Point(319, 149);
			this.buttonCreateEmptyFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCreateEmptyFile.Name = "buttonCreateEmptyFile";
			this.buttonCreateEmptyFile.Size = new System.Drawing.Size(56, 56);
			this.buttonCreateEmptyFile.TabIndex = 8;
			this.buttonCreateEmptyFile.UseVisualStyleBackColor = true;
			this.buttonCreateEmptyFile.Click += new System.EventHandler(this.buttonCreateEmptyFile_Click);
			// 
			// textBoxExt
			// 
			this.textBoxExt.Location = new System.Drawing.Point(115, 107);
			this.textBoxExt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.textBoxExt.Name = "textBoxExt";
			this.textBoxExt.Size = new System.Drawing.Size(91, 23);
			this.textBoxExt.TabIndex = 3;
			this.textBoxExt.Text = ".txt";
			this.textBoxExt.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
			this.textBoxExt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxes_KeyPress);
			// 
			// labelExt
			// 
			this.labelExt.AutoSize = true;
			this.labelExt.Location = new System.Drawing.Point(14, 110);
			this.labelExt.Name = "labelExt";
			this.labelExt.Size = new System.Drawing.Size(57, 15);
			this.labelExt.TabIndex = 10;
			this.labelExt.Text = "Extension";
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(381, 179);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(119, 26);
			this.buttonCancel.TabIndex = 10;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSave.Location = new System.Drawing.Point(381, 149);
			this.buttonSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(119, 26);
			this.buttonSave.TabIndex = 9;
			this.buttonSave.Text = "Save as default";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// textBoxInfo
			// 
			this.textBoxInfo.AcceptsTab = true;
			this.textBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxInfo.Location = new System.Drawing.Point(14, 149);
			this.textBoxInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.textBoxInfo.Multiline = true;
			this.textBoxInfo.Name = "textBoxInfo";
			this.textBoxInfo.ReadOnly = true;
			this.textBoxInfo.Size = new System.Drawing.Size(237, 53);
			this.textBoxInfo.TabIndex = 6;
			// 
			// checkBoxConfirm
			// 
			this.checkBoxConfirm.AutoSize = true;
			this.checkBoxConfirm.Checked = true;
			this.checkBoxConfirm.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxConfirm.Location = new System.Drawing.Point(255, 69);
			this.checkBoxConfirm.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxConfirm.Name = "checkBoxConfirm";
			this.checkBoxConfirm.Size = new System.Drawing.Size(210, 19);
			this.checkBoxConfirm.TabIndex = 5;
			this.checkBoxConfirm.Text = "Confirm before creating many files";
			this.checkBoxConfirm.UseVisualStyleBackColor = true;
			// 
			// numericUpDownStart
			// 
			this.numericUpDownStart.Location = new System.Drawing.Point(115, 45);
			this.numericUpDownStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericUpDownStart.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.numericUpDownStart.Name = "numericUpDownStart";
			this.numericUpDownStart.Size = new System.Drawing.Size(91, 23);
			this.numericUpDownStart.TabIndex = 1;
			this.numericUpDownStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownStart.ValueChanged += new System.EventHandler(this.numericUpDowns_ValueChanged);
			// 
			// numericUpDownEnd
			// 
			this.numericUpDownEnd.Location = new System.Drawing.Point(115, 76);
			this.numericUpDownEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numericUpDownEnd.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.numericUpDownEnd.Name = "numericUpDownEnd";
			this.numericUpDownEnd.Size = new System.Drawing.Size(91, 23);
			this.numericUpDownEnd.TabIndex = 2;
			this.numericUpDownEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownEnd.ValueChanged += new System.EventHandler(this.numericUpDowns_ValueChanged);
			// 
			// checkBoxCloseOnCreate
			// 
			this.checkBoxCloseOnCreate.AutoSize = true;
			this.checkBoxCloseOnCreate.Location = new System.Drawing.Point(255, 92);
			this.checkBoxCloseOnCreate.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxCloseOnCreate.Name = "checkBoxCloseOnCreate";
			this.checkBoxCloseOnCreate.Size = new System.Drawing.Size(118, 19);
			this.checkBoxCloseOnCreate.TabIndex = 17;
			this.checkBoxCloseOnCreate.Text = "Close on creation";
			this.checkBoxCloseOnCreate.UseVisualStyleBackColor = true;
			// 
			// chbSaveOnClose
			// 
			this.chbSaveOnClose.AutoSize = true;
			this.chbSaveOnClose.Location = new System.Drawing.Point(255, 115);
			this.chbSaveOnClose.Margin = new System.Windows.Forms.Padding(2);
			this.chbSaveOnClose.Name = "chbSaveOnClose";
			this.chbSaveOnClose.Size = new System.Drawing.Size(141, 19);
			this.chbSaveOnClose.TabIndex = 18;
			this.chbSaveOnClose.Text = "Save settings on close";
			this.chbSaveOnClose.UseVisualStyleBackColor = true;
			// 
			// CreateConsecutivesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(512, 218);
			this.Controls.Add(this.chbSaveOnClose);
			this.Controls.Add(this.checkBoxCloseOnCreate);
			this.Controls.Add(this.numericUpDownEnd);
			this.Controls.Add(this.numericUpDownStart);
			this.Controls.Add(this.checkBoxConfirm);
			this.Controls.Add(this.textBoxInfo);
			this.Controls.Add(this.buttonSave);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.labelExt);
			this.Controls.Add(this.textBoxExt);
			this.Controls.Add(this.buttonCreateEmptyFile);
			this.Controls.Add(this.buttonCreateFolder);
			this.Controls.Add(this.checkBoxAddZero);
			this.Controls.Add(this.labelEnd);
			this.Controls.Add(this.labelStartVal);
			this.Controls.Add(this.labelBaseName);
			this.Controls.Add(this.textBoxBaseName);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(528, 256);
			this.Name = "CreateConsecutivesForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateConsecutivesForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownEnd)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxBaseName;
		private System.Windows.Forms.Label labelBaseName;
		private System.Windows.Forms.Label labelStartVal;
		private System.Windows.Forms.Label labelEnd;
		private System.Windows.Forms.CheckBox checkBoxAddZero;
		private System.Windows.Forms.Button buttonCreateFolder;
		private System.Windows.Forms.Button buttonCreateEmptyFile;
		private System.Windows.Forms.TextBox textBoxExt;
		private System.Windows.Forms.Label labelExt;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.TextBox textBoxInfo;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox checkBoxConfirm;
		private System.Windows.Forms.NumericUpDown numericUpDownStart;
		private System.Windows.Forms.NumericUpDown numericUpDownEnd;
		private System.Windows.Forms.CheckBox checkBoxCloseOnCreate;
		private System.Windows.Forms.CheckBox chbSaveOnClose;
	}
}