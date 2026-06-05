namespace QuizoPlugins
{
	partial class SettingWindow
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
			this.nudInitialW = new System.Windows.Forms.NumericUpDown();
			this.nudInitialH = new System.Windows.Forms.NumericUpDown();
			this.labelX1 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.chbInitialSize = new System.Windows.Forms.CheckBox();
			this.checkBoxResizeMode = new System.Windows.Forms.CheckBox();
			this.nudDelta = new System.Windows.Forms.NumericUpDown();
			this.labelDELTARESIZE = new System.Windows.Forms.Label();
			this.buttonRestoreSize = new System.Windows.Forms.Button();
			this.cmbPresets = new System.Windows.Forms.ComboBox();
			this.nudPresets_X = new System.Windows.Forms.NumericUpDown();
			this.nudPresets_Y = new System.Windows.Forms.NumericUpDown();
			this.nudPresets_W = new System.Windows.Forms.NumericUpDown();
			this.nudPresets_H = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.buttonSet = new System.Windows.Forms.Button();
			this.buttonDel = new System.Windows.Forms.Button();
			this.chbInitialLoc = new System.Windows.Forms.CheckBox();
			this.nudInitialX = new System.Windows.Forms.NumericUpDown();
			this.nudInitialY = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.buttonRestoreLoc = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.buttonGetCurLoc = new System.Windows.Forms.Button();
			this.buttonGetCurSize = new System.Windows.Forms.Button();
			this.groupBoxPresets = new System.Windows.Forms.GroupBox();
			this.btnApplyPreset = new System.Windows.Forms.Button();
			this.chbPresetSizeEnabled = new System.Windows.Forms.CheckBox();
			this.chbPresetPosEnabled = new System.Windows.Forms.CheckBox();
			this.buttonGetCurrentToPreset = new System.Windows.Forms.Button();
			this.chbStartingPreset = new System.Windows.Forms.CheckBox();
			this.cmbStartingPreset = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialH)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudDelta)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPresets_X)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPresets_Y)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPresets_W)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPresets_H)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialY)).BeginInit();
			this.groupBoxPresets.SuspendLayout();
			this.SuspendLayout();
			// 
			// nudInitialW
			// 
			this.nudInitialW.Location = new System.Drawing.Point(189, 50);
			this.nudInitialW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudInitialW.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
			this.nudInitialW.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
			this.nudInitialW.Name = "nudInitialW";
			this.nudInitialW.Size = new System.Drawing.Size(85, 23);
			this.nudInitialW.TabIndex = 1;
			this.nudInitialW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudInitialW.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			// 
			// nudInitialH
			// 
			this.nudInitialH.Location = new System.Drawing.Point(311, 50);
			this.nudInitialH.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudInitialH.Minimum = new decimal(new int[] {
            96,
            0,
            0,
            0});
			this.nudInitialH.Name = "nudInitialH";
			this.nudInitialH.Size = new System.Drawing.Size(85, 23);
			this.nudInitialH.TabIndex = 2;
			this.nudInitialH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudInitialH.Value = new decimal(new int[] {
            96,
            0,
            0,
            0});
			// 
			// labelX1
			// 
			this.labelX1.AutoSize = true;
			this.labelX1.Location = new System.Drawing.Point(289, 54);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(16, 15);
			this.labelX1.TabIndex = 3;
			this.labelX1.Text = "H";
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(564, 245);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(87, 23);
			this.buttonOK.TabIndex = 13;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(657, 245);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(87, 23);
			this.buttonCancel.TabIndex = 14;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// chbInitialSize
			// 
			this.chbInitialSize.AutoSize = true;
			this.chbInitialSize.Location = new System.Drawing.Point(14, 52);
			this.chbInitialSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chbInitialSize.Name = "chbInitialSize";
			this.chbInitialSize.Size = new System.Drawing.Size(90, 19);
			this.chbInitialSize.TabIndex = 1;
			this.chbInitialSize.Text = "Starting Size";
			this.chbInitialSize.UseVisualStyleBackColor = true;
			this.chbInitialSize.CheckedChanged += new System.EventHandler(this.chbInitialSize_CheckedChanged);
			// 
			// checkBoxResizeMode
			// 
			this.checkBoxResizeMode.AutoSize = true;
			this.checkBoxResizeMode.Checked = true;
			this.checkBoxResizeMode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxResizeMode.Location = new System.Drawing.Point(14, 126);
			this.checkBoxResizeMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.checkBoxResizeMode.Name = "checkBoxResizeMode";
			this.checkBoxResizeMode.Size = new System.Drawing.Size(202, 19);
			this.checkBoxResizeMode.TabIndex = 5;
			this.checkBoxResizeMode.Text = "Reposition window when resizing";
			this.checkBoxResizeMode.UseVisualStyleBackColor = true;
			// 
			// nudDelta
			// 
			this.nudDelta.Location = new System.Drawing.Point(198, 162);
			this.nudDelta.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudDelta.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
			this.nudDelta.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudDelta.Name = "nudDelta";
			this.nudDelta.Size = new System.Drawing.Size(76, 23);
			this.nudDelta.TabIndex = 4;
			this.nudDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudDelta.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
			// 
			// labelDELTARESIZE
			// 
			this.labelDELTARESIZE.AutoSize = true;
			this.labelDELTARESIZE.Location = new System.Drawing.Point(14, 164);
			this.labelDELTARESIZE.Name = "labelDELTARESIZE";
			this.labelDELTARESIZE.Size = new System.Drawing.Size(154, 15);
			this.labelDELTARESIZE.TabIndex = 9;
			this.labelDELTARESIZE.Text = "Resize / move delta in pixels";
			// 
			// buttonRestoreSize
			// 
			this.buttonRestoreSize.Location = new System.Drawing.Point(402, 47);
			this.buttonRestoreSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonRestoreSize.Name = "buttonRestoreSize";
			this.buttonRestoreSize.Size = new System.Drawing.Size(168, 28);
			this.buttonRestoreSize.TabIndex = 3;
			this.buttonRestoreSize.Text = "Apply size";
			this.buttonRestoreSize.UseVisualStyleBackColor = true;
			this.buttonRestoreSize.Click += new System.EventHandler(this.buttonRestoreSize_Click);
			// 
			// cmbPresets
			// 
			this.cmbPresets.FormattingEnabled = true;
			this.cmbPresets.Location = new System.Drawing.Point(24, 22);
			this.cmbPresets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.cmbPresets.Name = "cmbPresets";
			this.cmbPresets.Size = new System.Drawing.Size(265, 23);
			this.cmbPresets.TabIndex = 6;
			this.cmbPresets.SelectedIndexChanged += new System.EventHandler(this.cmbPresets_SelectedIndexChanged);
			// 
			// nudPresets_X
			// 
			this.nudPresets_X.Location = new System.Drawing.Point(89, 66);
			this.nudPresets_X.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudPresets_X.Maximum = new decimal(new int[] {
            25600,
            0,
            0,
            0});
			this.nudPresets_X.Minimum = new decimal(new int[] {
            25600,
            0,
            0,
            -2147483648});
			this.nudPresets_X.Name = "nudPresets_X";
			this.nudPresets_X.Size = new System.Drawing.Size(80, 23);
			this.nudPresets_X.TabIndex = 9;
			this.nudPresets_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudPresets_Y
			// 
			this.nudPresets_Y.Location = new System.Drawing.Point(209, 66);
			this.nudPresets_Y.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudPresets_Y.Maximum = new decimal(new int[] {
            25600,
            0,
            0,
            0});
			this.nudPresets_Y.Minimum = new decimal(new int[] {
            25600,
            0,
            0,
            -2147483648});
			this.nudPresets_Y.Name = "nudPresets_Y";
			this.nudPresets_Y.Size = new System.Drawing.Size(80, 23);
			this.nudPresets_Y.TabIndex = 10;
			this.nudPresets_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudPresets_W
			// 
			this.nudPresets_W.Location = new System.Drawing.Point(89, 102);
			this.nudPresets_W.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudPresets_W.Maximum = new decimal(new int[] {
            2560,
            0,
            0,
            0});
			this.nudPresets_W.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
			this.nudPresets_W.Name = "nudPresets_W";
			this.nudPresets_W.Size = new System.Drawing.Size(80, 23);
			this.nudPresets_W.TabIndex = 11;
			this.nudPresets_W.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudPresets_W.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
			// 
			// nudPresets_H
			// 
			this.nudPresets_H.Location = new System.Drawing.Point(209, 102);
			this.nudPresets_H.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudPresets_H.Maximum = new decimal(new int[] {
            2560,
            0,
            0,
            0});
			this.nudPresets_H.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
			this.nudPresets_H.Name = "nudPresets_H";
			this.nudPresets_H.Size = new System.Drawing.Size(80, 23);
			this.nudPresets_H.TabIndex = 12;
			this.nudPresets_H.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudPresets_H.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(69, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 15);
			this.label2.TabIndex = 16;
			this.label2.Text = "X";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(188, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 15);
			this.label3.TabIndex = 17;
			this.label3.Text = "Y";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(69, 106);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(18, 15);
			this.label4.TabIndex = 18;
			this.label4.Text = "W";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(188, 106);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(16, 15);
			this.label5.TabIndex = 19;
			this.label5.Text = "H";
			// 
			// buttonSet
			// 
			this.buttonSet.Location = new System.Drawing.Point(307, 17);
			this.buttonSet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonSet.Name = "buttonSet";
			this.buttonSet.Size = new System.Drawing.Size(110, 23);
			this.buttonSet.TabIndex = 7;
			this.buttonSet.Text = "Set";
			this.buttonSet.UseVisualStyleBackColor = true;
			this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
			// 
			// buttonDel
			// 
			this.buttonDel.Location = new System.Drawing.Point(307, 49);
			this.buttonDel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonDel.Name = "buttonDel";
			this.buttonDel.Size = new System.Drawing.Size(110, 23);
			this.buttonDel.TabIndex = 8;
			this.buttonDel.Text = "Delete";
			this.buttonDel.UseVisualStyleBackColor = true;
			this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
			// 
			// chbInitialLoc
			// 
			this.chbInitialLoc.AutoSize = true;
			this.chbInitialLoc.Location = new System.Drawing.Point(14, 15);
			this.chbInitialLoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chbInitialLoc.Name = "chbInitialLoc";
			this.chbInitialLoc.Size = new System.Drawing.Size(116, 19);
			this.chbInitialLoc.TabIndex = 20;
			this.chbInitialLoc.Text = "Starting Location";
			this.chbInitialLoc.UseVisualStyleBackColor = true;
			this.chbInitialLoc.CheckedChanged += new System.EventHandler(this.chbInitialLoc_CheckedChanged);
			// 
			// nudInitialX
			// 
			this.nudInitialX.Location = new System.Drawing.Point(189, 13);
			this.nudInitialX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudInitialX.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudInitialX.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
			this.nudInitialX.Name = "nudInitialX";
			this.nudInitialX.Size = new System.Drawing.Size(85, 23);
			this.nudInitialX.TabIndex = 21;
			this.nudInitialX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudInitialY
			// 
			this.nudInitialY.Location = new System.Drawing.Point(309, 13);
			this.nudInitialY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.nudInitialY.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudInitialY.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
			this.nudInitialY.Name = "nudInitialY";
			this.nudInitialY.Size = new System.Drawing.Size(85, 23);
			this.nudInitialY.TabIndex = 22;
			this.nudInitialY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(165, 17);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(14, 15);
			this.label6.TabIndex = 23;
			this.label6.Text = "X";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(289, 17);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(14, 15);
			this.label7.TabIndex = 24;
			this.label7.Text = "Y";
			// 
			// buttonRestoreLoc
			// 
			this.buttonRestoreLoc.Location = new System.Drawing.Point(400, 10);
			this.buttonRestoreLoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonRestoreLoc.Name = "buttonRestoreLoc";
			this.buttonRestoreLoc.Size = new System.Drawing.Size(168, 28);
			this.buttonRestoreLoc.TabIndex = 25;
			this.buttonRestoreLoc.Text = "Apply location";
			this.buttonRestoreLoc.UseVisualStyleBackColor = true;
			this.buttonRestoreLoc.Click += new System.EventHandler(this.buttonRestoreLoc_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(165, 54);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(18, 15);
			this.label8.TabIndex = 26;
			this.label8.Text = "W";
			// 
			// buttonGetCurLoc
			// 
			this.buttonGetCurLoc.Location = new System.Drawing.Point(576, 10);
			this.buttonGetCurLoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonGetCurLoc.Name = "buttonGetCurLoc";
			this.buttonGetCurLoc.Size = new System.Drawing.Size(168, 28);
			this.buttonGetCurLoc.TabIndex = 27;
			this.buttonGetCurLoc.Text = "Get current location";
			this.buttonGetCurLoc.UseVisualStyleBackColor = true;
			this.buttonGetCurLoc.Click += new System.EventHandler(this.buttonGetCurLoc_Click);
			// 
			// buttonGetCurSize
			// 
			this.buttonGetCurSize.Location = new System.Drawing.Point(576, 47);
			this.buttonGetCurSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonGetCurSize.Name = "buttonGetCurSize";
			this.buttonGetCurSize.Size = new System.Drawing.Size(168, 28);
			this.buttonGetCurSize.TabIndex = 28;
			this.buttonGetCurSize.Text = "Get current size";
			this.buttonGetCurSize.UseVisualStyleBackColor = true;
			this.buttonGetCurSize.Click += new System.EventHandler(this.buttonGetCurSize_Click);
			// 
			// groupBoxPresets
			// 
			this.groupBoxPresets.Controls.Add(this.btnApplyPreset);
			this.groupBoxPresets.Controls.Add(this.chbPresetSizeEnabled);
			this.groupBoxPresets.Controls.Add(this.chbPresetPosEnabled);
			this.groupBoxPresets.Controls.Add(this.buttonGetCurrentToPreset);
			this.groupBoxPresets.Controls.Add(this.cmbPresets);
			this.groupBoxPresets.Controls.Add(this.buttonSet);
			this.groupBoxPresets.Controls.Add(this.buttonDel);
			this.groupBoxPresets.Controls.Add(this.label2);
			this.groupBoxPresets.Controls.Add(this.nudPresets_X);
			this.groupBoxPresets.Controls.Add(this.nudPresets_Y);
			this.groupBoxPresets.Controls.Add(this.nudPresets_W);
			this.groupBoxPresets.Controls.Add(this.nudPresets_H);
			this.groupBoxPresets.Controls.Add(this.label3);
			this.groupBoxPresets.Controls.Add(this.label5);
			this.groupBoxPresets.Controls.Add(this.label4);
			this.groupBoxPresets.Location = new System.Drawing.Point(327, 87);
			this.groupBoxPresets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBoxPresets.Name = "groupBoxPresets";
			this.groupBoxPresets.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.groupBoxPresets.Size = new System.Drawing.Size(425, 148);
			this.groupBoxPresets.TabIndex = 29;
			this.groupBoxPresets.TabStop = false;
			this.groupBoxPresets.Text = "Presets";
			// 
			// btnApplyPreset
			// 
			this.btnApplyPreset.Location = new System.Drawing.Point(307, 113);
			this.btnApplyPreset.Name = "btnApplyPreset";
			this.btnApplyPreset.Size = new System.Drawing.Size(110, 23);
			this.btnApplyPreset.TabIndex = 23;
			this.btnApplyPreset.Text = "Apply";
			this.btnApplyPreset.UseVisualStyleBackColor = true;
			this.btnApplyPreset.Click += new System.EventHandler(this.btnApplyPreset_Click);
			// 
			// chbPresetSizeEnabled
			// 
			this.chbPresetSizeEnabled.AutoSize = true;
			this.chbPresetSizeEnabled.Location = new System.Drawing.Point(24, 106);
			this.chbPresetSizeEnabled.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chbPresetSizeEnabled.Name = "chbPresetSizeEnabled";
			this.chbPresetSizeEnabled.Size = new System.Drawing.Size(15, 14);
			this.chbPresetSizeEnabled.TabIndex = 22;
			this.chbPresetSizeEnabled.UseVisualStyleBackColor = true;
			this.chbPresetSizeEnabled.CheckedChanged += new System.EventHandler(this.chbPresetEnabled_CheckedChanged);
			// 
			// chbPresetPosEnabled
			// 
			this.chbPresetPosEnabled.AutoSize = true;
			this.chbPresetPosEnabled.Location = new System.Drawing.Point(24, 70);
			this.chbPresetPosEnabled.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chbPresetPosEnabled.Name = "chbPresetPosEnabled";
			this.chbPresetPosEnabled.Size = new System.Drawing.Size(15, 14);
			this.chbPresetPosEnabled.TabIndex = 21;
			this.chbPresetPosEnabled.UseVisualStyleBackColor = true;
			this.chbPresetPosEnabled.CheckedChanged += new System.EventHandler(this.chbPresetEnabled_CheckedChanged);
			// 
			// buttonGetCurrentToPreset
			// 
			this.buttonGetCurrentToPreset.Location = new System.Drawing.Point(307, 81);
			this.buttonGetCurrentToPreset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonGetCurrentToPreset.Name = "buttonGetCurrentToPreset";
			this.buttonGetCurrentToPreset.Size = new System.Drawing.Size(110, 23);
			this.buttonGetCurrentToPreset.TabIndex = 20;
			this.buttonGetCurrentToPreset.Text = "Get current";
			this.buttonGetCurrentToPreset.UseVisualStyleBackColor = true;
			this.buttonGetCurrentToPreset.Click += new System.EventHandler(this.buttonGetCurrentToPreset_Click);
			// 
			// chbStartingPreset
			// 
			this.chbStartingPreset.AutoSize = true;
			this.chbStartingPreset.Location = new System.Drawing.Point(14, 89);
			this.chbStartingPreset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chbStartingPreset.Name = "chbStartingPreset";
			this.chbStartingPreset.Size = new System.Drawing.Size(102, 19);
			this.chbStartingPreset.TabIndex = 30;
			this.chbStartingPreset.Text = "Starting Preset";
			this.chbStartingPreset.UseVisualStyleBackColor = true;
			this.chbStartingPreset.CheckedChanged += new System.EventHandler(this.chbStartingPreset_CheckedChanged);
			// 
			// cmbStartingPreset
			// 
			this.cmbStartingPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbStartingPreset.FormattingEnabled = true;
			this.cmbStartingPreset.Location = new System.Drawing.Point(168, 87);
			this.cmbStartingPreset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.cmbStartingPreset.Name = "cmbStartingPreset";
			this.cmbStartingPreset.Size = new System.Drawing.Size(140, 23);
			this.cmbStartingPreset.TabIndex = 31;
			// 
			// SettingWindow
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(764, 284);
			this.Controls.Add(this.cmbStartingPreset);
			this.Controls.Add(this.chbStartingPreset);
			this.Controls.Add(this.buttonGetCurSize);
			this.Controls.Add(this.buttonGetCurLoc);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.buttonRestoreLoc);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.nudInitialY);
			this.Controls.Add(this.nudInitialX);
			this.Controls.Add(this.chbInitialLoc);
			this.Controls.Add(this.buttonRestoreSize);
			this.Controls.Add(this.nudDelta);
			this.Controls.Add(this.labelDELTARESIZE);
			this.Controls.Add(this.checkBoxResizeMode);
			this.Controls.Add(this.chbInitialSize);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.labelX1);
			this.Controls.Add(this.nudInitialH);
			this.Controls.Add(this.nudInitialW);
			this.Controls.Add(this.groupBoxPresets);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Window Manager";
			((System.ComponentModel.ISupportInitialize)(this.nudInitialW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialH)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudDelta)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPresets_X)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPresets_Y)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPresets_W)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPresets_H)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudInitialY)).EndInit();
			this.groupBoxPresets.ResumeLayout(false);
			this.groupBoxPresets.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown nudInitialW;
		private System.Windows.Forms.NumericUpDown nudInitialH;
		private System.Windows.Forms.Label labelX1;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.CheckBox chbInitialSize;
		private System.Windows.Forms.CheckBox checkBoxResizeMode;
		private System.Windows.Forms.NumericUpDown nudDelta;
		private System.Windows.Forms.Label labelDELTARESIZE;
		private System.Windows.Forms.Button buttonRestoreSize;
		private System.Windows.Forms.ComboBox cmbPresets;
		private System.Windows.Forms.NumericUpDown nudPresets_X;
		private System.Windows.Forms.NumericUpDown nudPresets_Y;
		private System.Windows.Forms.NumericUpDown nudPresets_W;
		private System.Windows.Forms.NumericUpDown nudPresets_H;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button buttonSet;
		private System.Windows.Forms.Button buttonDel;
		private System.Windows.Forms.CheckBox chbInitialLoc;
		private System.Windows.Forms.NumericUpDown nudInitialX;
		private System.Windows.Forms.NumericUpDown nudInitialY;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button buttonRestoreLoc;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button buttonGetCurLoc;
		private System.Windows.Forms.Button buttonGetCurSize;
		private System.Windows.Forms.GroupBox groupBoxPresets;
		private System.Windows.Forms.Button buttonGetCurrentToPreset;
		private System.Windows.Forms.CheckBox chbStartingPreset;
		private System.Windows.Forms.ComboBox cmbStartingPreset;
		private System.Windows.Forms.CheckBox chbPresetSizeEnabled;
		private System.Windows.Forms.CheckBox chbPresetPosEnabled;
		private System.Windows.Forms.Button btnApplyPreset;
	}
}