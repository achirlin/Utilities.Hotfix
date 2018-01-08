using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Utilities.Hotfix.XmlFix
{
	public partial class frmUpdate : Form
	{
		Update mobjUpdate = null;

		public frmUpdate()
		{
			InitializeComponent();
			mobjUpdate = new Update(mobjOpenDialog);
			UpdateList();
		}

		private void mobjCmdAdd_Click(object sender, EventArgs e)
		{
			mobjUpdate.Add();
			UpdateList();
		}

		private void mobjCmdDelete_Click(object sender, EventArgs e)
		{
			if (mobjLstTargetFiles.SelectedItem != null)
			{
				mobjUpdate.Remove((Update.FileRef)mobjLstTargetFiles.SelectedValue);
				UpdateList();
			}
		}

		private void UpdateList()
		{
			int index = mobjLstTargetFiles.SelectedIndex;
			mobjLstTargetFiles.DataSource = null;
			mobjLstTargetFiles.DataSource = mobjUpdate.mobjTargetFiles;
			mobjLstTargetFiles.DisplayMember = "ConfigFile";

			if (index < mobjLstTargetFiles.Items.Count && index > -1)
			{ 
				mobjLstTargetFiles.SelectedIndex = index;
			}

			mobjCmdUpdate.Enabled = mobjLstTargetFiles.Items.Count > 0;
			mobjRoolBack.Enabled = mobjCmdUpdate.Enabled;
			mobjCmdOpen.Enabled = mobjCmdUpdate.Enabled;


		}

		private void mobjCmdUpdate_Click(object sender, EventArgs e)
		{
			DialogResult YesNoAnswer;

			YesNoAnswer = MessageBox.Show(this, "Are you sure to update the configuration files?", string.Empty, 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (YesNoAnswer == DialogResult.Yes	)
			{
				mobjUpdate.UpdateTarget();
				MessageBox.Show(this, "Update Completed. Please review the log file.",
					"", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void mobjCmdExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void mobjCmdHelp_Click(object sender, EventArgs e)
		{
			StringBuilder objSB = new StringBuilder();

			objSB.AppendLine("1. Add configuration files to update by clicking the '+' button.");
			objSB.AppendLine("2. Please validate that files are not read only.");
			objSB.AppendLine("3. Press the 'Update...' button and confirm the update.");
			objSB.AppendLine("4. Review the log file in notepad.");
			objSB.AppendLine("");
			objSB.AppendLine("5. The configuration files will be searched for all View elements");
			objSB.AppendLine("   and openModal=\"true\" attribute will be added.");
			objSB.AppendLine("");
			objSB.AppendLine("6. Press 'Rollback...' to revert changes made to configuration files.");

			MessageBox.Show(this, objSB.ToString(), "Help");

		}

		private void mobjRoolBack_Click(object sender, EventArgs e)
		{
			DialogResult YesNoAnswer;

			YesNoAnswer = MessageBox.Show(this, "Are you sure to revert changes in the configuration files?", string.Empty, 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (YesNoAnswer == DialogResult.Yes	)
			{
				mobjUpdate.RollBack();
				MessageBox.Show(this, "Update Completed. Please review the log file.",
					"", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void mobjCmdOpen_Click(object sender, EventArgs e)
		{
			if (mobjLstTargetFiles.SelectedItem != null)
			{
				Process proc = new Process();
				try
				{
					proc.EnableRaisingEvents=false;
				
					proc.StartInfo.FileName="notepad.exe";
					proc.StartInfo.UseShellExecute=false;
					proc.StartInfo.Arguments = 
						((Update.FileRef)mobjLstTargetFiles.SelectedValue).ConfigFile;
					proc.StartInfo.WindowStyle= ProcessWindowStyle.Normal;
				
					//asynchronous start of application
					proc.Start();
				}
				catch
				{
					MessageBox.Show(this, "Unable to open Notepad with file.", "Error", 
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}



		}

	}
}