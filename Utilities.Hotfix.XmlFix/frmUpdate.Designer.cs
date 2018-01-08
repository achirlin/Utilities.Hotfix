namespace Utilities.Hotfix.XmlFix
{
	partial class frmUpdate
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.mobjCmdExit = new System.Windows.Forms.Button();
			this.mobjCmdUpdate = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.mobjCmdOpen = new System.Windows.Forms.Button();
			this.mobjCmdDelete = new System.Windows.Forms.Button();
			this.mobjCmdAdd = new System.Windows.Forms.Button();
			this.mobjLstTargetFiles = new System.Windows.Forms.ListBox();
			this.mobjOpenDialog = new System.Windows.Forms.OpenFileDialog();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.mobjRoolBack = new System.Windows.Forms.Button();
			this.mobjCmdHelp = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mobjCmdExit
			// 
			this.mobjCmdExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mobjCmdExit.Location = new System.Drawing.Point(311, 235);
			this.mobjCmdExit.Name = "mobjCmdExit";
			this.mobjCmdExit.Size = new System.Drawing.Size(75, 23);
			this.mobjCmdExit.TabIndex = 0;
			this.mobjCmdExit.Text = "&Exit";
			this.toolTip1.SetToolTip(this.mobjCmdExit, "Close the program");
			this.mobjCmdExit.UseVisualStyleBackColor = true;
			this.mobjCmdExit.Click += new System.EventHandler(this.mobjCmdExit_Click);
			// 
			// mobjCmdUpdate
			// 
			this.mobjCmdUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mobjCmdUpdate.Location = new System.Drawing.Point(230, 235);
			this.mobjCmdUpdate.Name = "mobjCmdUpdate";
			this.mobjCmdUpdate.Size = new System.Drawing.Size(75, 23);
			this.mobjCmdUpdate.TabIndex = 1;
			this.mobjCmdUpdate.Text = "&Update ...";
			this.toolTip1.SetToolTip(this.mobjCmdUpdate, "Update target files");
			this.mobjCmdUpdate.UseVisualStyleBackColor = true;
			this.mobjCmdUpdate.Click += new System.EventHandler(this.mobjCmdUpdate_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.mobjCmdOpen);
			this.groupBox1.Controls.Add(this.mobjCmdDelete);
			this.groupBox1.Controls.Add(this.mobjCmdAdd);
			this.groupBox1.Controls.Add(this.mobjLstTargetFiles);
			this.groupBox1.Location = new System.Drawing.Point(12, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(374, 220);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Target Files";
			// 
			// mobjCmdOpen
			// 
			this.mobjCmdOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mobjCmdOpen.Image = global::Utilities.Hotfix.XmlFix.Properties.Resources.a;
			this.mobjCmdOpen.Location = new System.Drawing.Point(338, 78);
			this.mobjCmdOpen.Name = "mobjCmdOpen";
			this.mobjCmdOpen.Size = new System.Drawing.Size(29, 23);
			this.mobjCmdOpen.TabIndex = 1;
			this.toolTip1.SetToolTip(this.mobjCmdOpen, "Open the configuration file in notepad.");
			this.mobjCmdOpen.UseVisualStyleBackColor = true;
			this.mobjCmdOpen.Click += new System.EventHandler(this.mobjCmdOpen_Click);
			// 
			// mobjCmdDelete
			// 
			this.mobjCmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mobjCmdDelete.Location = new System.Drawing.Point(338, 49);
			this.mobjCmdDelete.Name = "mobjCmdDelete";
			this.mobjCmdDelete.Size = new System.Drawing.Size(29, 23);
			this.mobjCmdDelete.TabIndex = 1;
			this.mobjCmdDelete.Text = "--";
			this.toolTip1.SetToolTip(this.mobjCmdDelete, "Remove from list of target files");
			this.mobjCmdDelete.UseVisualStyleBackColor = true;
			this.mobjCmdDelete.Click += new System.EventHandler(this.mobjCmdDelete_Click);
			// 
			// mobjCmdAdd
			// 
			this.mobjCmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mobjCmdAdd.Location = new System.Drawing.Point(338, 20);
			this.mobjCmdAdd.Name = "mobjCmdAdd";
			this.mobjCmdAdd.Size = new System.Drawing.Size(29, 23);
			this.mobjCmdAdd.TabIndex = 1;
			this.mobjCmdAdd.Text = "+";
			this.toolTip1.SetToolTip(this.mobjCmdAdd, "Add config file to target files");
			this.mobjCmdAdd.UseVisualStyleBackColor = true;
			this.mobjCmdAdd.Click += new System.EventHandler(this.mobjCmdAdd_Click);
			// 
			// mobjLstTargetFiles
			// 
			this.mobjLstTargetFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.mobjLstTargetFiles.FormattingEnabled = true;
			this.mobjLstTargetFiles.Location = new System.Drawing.Point(6, 19);
			this.mobjLstTargetFiles.Name = "mobjLstTargetFiles";
			this.mobjLstTargetFiles.Size = new System.Drawing.Size(326, 186);
			this.mobjLstTargetFiles.TabIndex = 0;
			// 
			// mobjOpenDialog
			// 
			this.mobjOpenDialog.DefaultExt = "*.config";
			this.mobjOpenDialog.Filter = "XML config|*.config";
			this.mobjOpenDialog.RestoreDirectory = true;
			this.mobjOpenDialog.SupportMultiDottedExtensions = true;
			this.mobjOpenDialog.Title = "Select the target updated configuration file";
			// 
			// mobjRoolBack
			// 
			this.mobjRoolBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mobjRoolBack.Location = new System.Drawing.Point(93, 235);
			this.mobjRoolBack.Name = "mobjRoolBack";
			this.mobjRoolBack.Size = new System.Drawing.Size(75, 23);
			this.mobjRoolBack.TabIndex = 4;
			this.mobjRoolBack.Text = "Rollback...";
			this.toolTip1.SetToolTip(this.mobjRoolBack, "Revert changes made in the target files");
			this.mobjRoolBack.UseVisualStyleBackColor = true;
			this.mobjRoolBack.Click += new System.EventHandler(this.mobjRoolBack_Click);
			// 
			// mobjCmdHelp
			// 
			this.mobjCmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mobjCmdHelp.Location = new System.Drawing.Point(12, 235);
			this.mobjCmdHelp.Name = "mobjCmdHelp";
			this.mobjCmdHelp.Size = new System.Drawing.Size(75, 23);
			this.mobjCmdHelp.TabIndex = 3;
			this.mobjCmdHelp.Text = "&Help";
			this.mobjCmdHelp.UseVisualStyleBackColor = true;
			this.mobjCmdHelp.Click += new System.EventHandler(this.mobjCmdHelp_Click);
			// 
			// frmUpdate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(398, 270);
			this.Controls.Add(this.mobjRoolBack);
			this.Controls.Add(this.mobjCmdHelp);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.mobjCmdUpdate);
			this.Controls.Add(this.mobjCmdExit);
			this.MinimizeBox = false;
			this.Name = "frmUpdate";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Configuration File Update Tool";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button mobjCmdExit;
		private System.Windows.Forms.Button mobjCmdUpdate;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button mobjCmdDelete;
		private System.Windows.Forms.Button mobjCmdAdd;
		private System.Windows.Forms.ListBox mobjLstTargetFiles;
		private System.Windows.Forms.OpenFileDialog mobjOpenDialog;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button mobjCmdHelp;
		private System.Windows.Forms.Button mobjRoolBack;
		private System.Windows.Forms.Button mobjCmdOpen;
	}
}

