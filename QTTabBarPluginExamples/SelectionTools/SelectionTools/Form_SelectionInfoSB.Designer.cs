namespace QuizoPlugins
{
	partial class Form_SelectionInfoSB
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
			this.chbSize = new System.Windows.Forms.CheckBox();
			this.chbMod = new System.Windows.Forms.CheckBox();
			this.chbCrt = new System.Windows.Forms.CheckBox();
			this.chbAtt = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.chbDrive = new System.Windows.Forms.CheckBox();
			this.chbSizInByte = new System.Windows.Forms.CheckBox();
			this.chbNetwork = new System.Windows.Forms.CheckBox();
			this.chbFolderSize = new System.Windows.Forms.CheckBox();
			this.chbCurFolder = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// chbSize
			// 
			this.chbSize.AutoSize = true;
			this.chbSize.Location = new System.Drawing.Point(12, 13);
			this.chbSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chbSize.Name = "chbSize";
			this.chbSize.Size = new System.Drawing.Size(46, 19);
			this.chbSize.TabIndex = 0;
			this.chbSize.Text = "Size";
			this.chbSize.UseVisualStyleBackColor = true;
			this.chbSize.CheckedChanged += new System.EventHandler(this.chbFolSize_CheckedChanged);
			// 
			// chbMod
			// 
			this.chbMod.AutoSize = true;
			this.chbMod.Location = new System.Drawing.Point(12, 40);
			this.chbMod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chbMod.Name = "chbMod";
			this.chbMod.Size = new System.Drawing.Size(77, 19);
			this.chbMod.TabIndex = 1;
			this.chbMod.Text = "Mod date";
			this.chbMod.UseVisualStyleBackColor = true;
			// 
			// chbCrt
			// 
			this.chbCrt.AutoSize = true;
			this.chbCrt.Location = new System.Drawing.Point(12, 67);
			this.chbCrt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chbCrt.Name = "chbCrt";
			this.chbCrt.Size = new System.Drawing.Size(71, 19);
			this.chbCrt.TabIndex = 2;
			this.chbCrt.Text = "Creation";
			this.chbCrt.UseVisualStyleBackColor = true;
			// 
			// chbAtt
			// 
			this.chbAtt.AutoSize = true;
			this.chbAtt.Location = new System.Drawing.Point(12, 94);
			this.chbAtt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chbAtt.Name = "chbAtt";
			this.chbAtt.Size = new System.Drawing.Size(78, 19);
			this.chbAtt.TabIndex = 3;
			this.chbAtt.Text = "Attributes";
			this.chbAtt.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(301, 150);
			this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(87, 26);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// chbDrive
			// 
			this.chbDrive.AutoSize = true;
			this.chbDrive.Location = new System.Drawing.Point(12, 121);
			this.chbDrive.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chbDrive.Name = "chbDrive";
			this.chbDrive.Size = new System.Drawing.Size(120, 19);
			this.chbDrive.TabIndex = 4;
			this.chbDrive.Text = "Current Drive Info";
			this.chbDrive.UseVisualStyleBackColor = true;
			// 
			// chbSizInByte
			// 
			this.chbSizInByte.AutoSize = true;
			this.chbSizInByte.Location = new System.Drawing.Point(184, 13);
			this.chbSizInByte.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chbSizInByte.Name = "chbSizInByte";
			this.chbSizInByte.Size = new System.Drawing.Size(85, 19);
			this.chbSizInByte.TabIndex = 6;
			this.chbSizInByte.Text = "Size in byte";
			this.chbSizInByte.UseVisualStyleBackColor = true;
			// 
			// chbNetwork
			// 
			this.chbNetwork.AutoSize = true;
			this.chbNetwork.Location = new System.Drawing.Point(184, 65);
			this.chbNetwork.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.chbNetwork.Name = "chbNetwork";
			this.chbNetwork.Size = new System.Drawing.Size(125, 19);
			this.chbNetwork.TabIndex = 7;
			this.chbNetwork.Text = "Enable for network";
			this.chbNetwork.UseVisualStyleBackColor = true;
			// 
			// chbFolderSize
			// 
			this.chbFolderSize.AutoSize = true;
			this.chbFolderSize.Location = new System.Drawing.Point(184, 39);
			this.chbFolderSize.Name = "chbFolderSize";
			this.chbFolderSize.Size = new System.Drawing.Size(81, 19);
			this.chbFolderSize.TabIndex = 8;
			this.chbFolderSize.Text = "Folder size";
			this.chbFolderSize.UseVisualStyleBackColor = true;
			// 
			// chbCurFolder
			// 
			this.chbCurFolder.AutoSize = true;
			this.chbCurFolder.Location = new System.Drawing.Point(12, 147);
			this.chbCurFolder.Name = "chbCurFolder";
			this.chbCurFolder.Size = new System.Drawing.Size(126, 19);
			this.chbCurFolder.TabIndex = 9;
			this.chbCurFolder.Text = "Current Folder Info";
			this.chbCurFolder.UseVisualStyleBackColor = true;
			// 
			// Form_SelectionInfoSB
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 189);
			this.Controls.Add(this.chbCurFolder);
			this.Controls.Add(this.chbFolderSize);
			this.Controls.Add(this.chbNetwork);
			this.Controls.Add(this.chbSizInByte);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.chbDrive);
			this.Controls.Add(this.chbAtt);
			this.Controls.Add(this.chbCrt);
			this.Controls.Add(this.chbMod);
			this.Controls.Add(this.chbSize);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form_SelectionInfoSB";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chbSize;
		private System.Windows.Forms.CheckBox chbMod;
		private System.Windows.Forms.CheckBox chbCrt;
		private System.Windows.Forms.CheckBox chbAtt;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.CheckBox chbDrive;
		private System.Windows.Forms.CheckBox chbSizInByte;
		private System.Windows.Forms.CheckBox chbNetwork;
		private System.Windows.Forms.CheckBox chbFolderSize;
		private System.Windows.Forms.CheckBox chbCurFolder;
	}
}