namespace QuizoPlugins
{
	partial class MigemoOptionForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigemoOptionForm));
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.textBoxDLL = new System.Windows.Forms.TextBox();
			this.textBoxDic = new System.Windows.Forms.TextBox();
			this.buttonBrowseDll = new System.Windows.Forms.Button();
			this.buttonBrowseDic = new System.Windows.Forms.Button();
			this.chbPerfectMatch = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.chbUTF8 = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.Location = new System.Drawing.Point(297, 198);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 21);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(378, 198);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 21);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "キャンセル";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// textBoxDLL
			// 
			this.textBoxDLL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxDLL.Location = new System.Drawing.Point(85, 11);
			this.textBoxDLL.Name = "textBoxDLL";
			this.textBoxDLL.Size = new System.Drawing.Size(335, 19);
			this.textBoxDLL.TabIndex = 2;
			this.textBoxDLL.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
			// 
			// textBoxDic
			// 
			this.textBoxDic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxDic.Location = new System.Drawing.Point(85, 36);
			this.textBoxDic.Name = "textBoxDic";
			this.textBoxDic.Size = new System.Drawing.Size(335, 19);
			this.textBoxDic.TabIndex = 3;
			this.textBoxDic.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
			// 
			// buttonBrowseDll
			// 
			this.buttonBrowseDll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowseDll.Location = new System.Drawing.Point(426, 10);
			this.buttonBrowseDll.Name = "buttonBrowseDll";
			this.buttonBrowseDll.Size = new System.Drawing.Size(27, 21);
			this.buttonBrowseDll.TabIndex = 4;
			this.buttonBrowseDll.Text = "...";
			this.buttonBrowseDll.UseVisualStyleBackColor = true;
			this.buttonBrowseDll.Click += new System.EventHandler(this.button_Browse_Click);
			// 
			// buttonBrowseDic
			// 
			this.buttonBrowseDic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowseDic.Location = new System.Drawing.Point(426, 35);
			this.buttonBrowseDic.Name = "buttonBrowseDic";
			this.buttonBrowseDic.Size = new System.Drawing.Size(27, 21);
			this.buttonBrowseDic.TabIndex = 5;
			this.buttonBrowseDic.Text = "...";
			this.buttonBrowseDic.UseVisualStyleBackColor = true;
			this.buttonBrowseDic.Click += new System.EventHandler(this.button_Browse_Click);
			// 
			// chbPerfectMatch
			// 
			this.chbPerfectMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chbPerfectMatch.AutoSize = true;
			this.chbPerfectMatch.Location = new System.Drawing.Point(12, 83);
			this.chbPerfectMatch.Name = "chbPerfectMatch";
			this.chbPerfectMatch.Size = new System.Drawing.Size(96, 16);
			this.chbPerfectMatch.TabIndex = 6;
			this.chbPerfectMatch.Text = "完全一致検索";
			this.chbPerfectMatch.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 12);
			this.label1.TabIndex = 7;
			this.label1.Text = "dll パス";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 12);
			this.label2.TabIndex = 8;
			this.label2.Text = "辞書のパス";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Location = new System.Drawing.Point(12, 112);
			this.label3.Name = "label3";
			this.label3.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
			this.label3.Size = new System.Drawing.Size(441, 83);
			this.label3.TabIndex = 9;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// chbUTF8
			// 
			this.chbUTF8.AutoSize = true;
			this.chbUTF8.Location = new System.Drawing.Point(12, 61);
			this.chbUTF8.Name = "chbUTF8";
			this.chbUTF8.Size = new System.Drawing.Size(144, 16);
			this.chbUTF8.TabIndex = 10;
			this.chbUTF8.Text = "UTF-8の辞書を使用する";
			this.chbUTF8.UseVisualStyleBackColor = true;
			// 
			// MigemoOptionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(465, 231);
			this.Controls.Add(this.chbUTF8);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chbPerfectMatch);
			this.Controls.Add(this.buttonBrowseDic);
			this.Controls.Add(this.buttonBrowseDll);
			this.Controls.Add(this.textBoxDic);
			this.Controls.Add(this.textBoxDLL);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MigemoOptionForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Migemo ローダー オプション";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.TextBox textBoxDLL;
		private System.Windows.Forms.TextBox textBoxDic;
		private System.Windows.Forms.Button buttonBrowseDll;
		private System.Windows.Forms.Button buttonBrowseDic;
		private System.Windows.Forms.CheckBox chbPerfectMatch;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chbUTF8;
	}
}