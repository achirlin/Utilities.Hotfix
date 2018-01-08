using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace Utilities.Hotfix
{
	public class ConfigForm : System.Windows.Forms.Form
	{
		#region Windows designer Code
		private System.Windows.Forms.TabControl tabSettings;
		private System.Windows.Forms.Button cmdExit;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.TabPage tabBackup;
		private System.Windows.Forms.TabPage tabServices;
		private System.Windows.Forms.TabPage tabAbout;
		private System.Windows.Forms.CheckBox chkIsDoBackup;
		private System.Windows.Forms.FolderBrowserDialog fbdBackup;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel pnlBackup;
		private System.Windows.Forms.CheckBox chkRelative;
		private System.Windows.Forms.Button cmdBrowseBackupFolder;
		private System.Windows.Forms.TextBox txtBackupFolder;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstServices;
		private System.Windows.Forms.Label lblServicesList;
		private System.Windows.Forms.Label lblServiceStatus;
		private System.Windows.Forms.Button cmdServiceStart;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lblPath;
		private System.Windows.Forms.Label lblCapVersion;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Button cmdRefresh;
		#endregion // Windows designer Code
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabSettings = new System.Windows.Forms.TabControl();
			this.tabServices = new System.Windows.Forms.TabPage();
			this.cmdRefresh = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.cmdServiceStart = new System.Windows.Forms.Button();
			this.lblServiceStatus = new System.Windows.Forms.Label();
			this.lblServicesList = new System.Windows.Forms.Label();
			this.lstServices = new System.Windows.Forms.ListBox();
			this.tabBackup = new System.Windows.Forms.TabPage();
			this.pnlBackup = new System.Windows.Forms.Panel();
			this.lblPath = new System.Windows.Forms.Label();
			this.chkRelative = new System.Windows.Forms.CheckBox();
			this.cmdBrowseBackupFolder = new System.Windows.Forms.Button();
			this.txtBackupFolder = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkIsDoBackup = new System.Windows.Forms.CheckBox();
			this.tabAbout = new System.Windows.Forms.TabPage();
			this.lblCapVersion = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.cmdExit = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.fbdBackup = new System.Windows.Forms.FolderBrowserDialog();
			this.tabSettings.SuspendLayout();
			this.tabServices.SuspendLayout();
			this.tabBackup.SuspendLayout();
			this.pnlBackup.SuspendLayout();
			this.tabAbout.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabSettings
			// 
			this.tabSettings.Controls.Add(this.tabServices);
			this.tabSettings.Controls.Add(this.tabBackup);
			this.tabSettings.Controls.Add(this.tabAbout);
			this.tabSettings.Location = new System.Drawing.Point(8, 8);
			this.tabSettings.Name = "tabSettings";
			this.tabSettings.SelectedIndex = 0;
			this.tabSettings.Size = new System.Drawing.Size(400, 168);
			this.tabSettings.TabIndex = 0;
			// 
			// tabServices
			// 
			this.tabServices.Controls.Add(this.cmdRefresh);
			this.tabServices.Controls.Add(this.button1);
			this.tabServices.Controls.Add(this.cmdServiceStart);
			this.tabServices.Controls.Add(this.lblServiceStatus);
			this.tabServices.Controls.Add(this.lblServicesList);
			this.tabServices.Controls.Add(this.lstServices);
			this.tabServices.Location = new System.Drawing.Point(4, 22);
			this.tabServices.Name = "tabServices";
			this.tabServices.Size = new System.Drawing.Size(392, 142);
			this.tabServices.TabIndex = 1;
			this.tabServices.Text = "Services";
			// 
			// cmdRefresh
			// 
			this.cmdRefresh.Location = new System.Drawing.Point(273, 110);
			this.cmdRefresh.Name = "cmdRefresh";
			this.cmdRefresh.Size = new System.Drawing.Size(115, 22);
			this.cmdRefresh.TabIndex = 5;
			this.cmdRefresh.Text = "&Refresh";
			this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(273, 82);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(115, 22);
			this.button1.TabIndex = 4;
			this.button1.Text = "Sto&p";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// cmdServiceStart
			// 
			this.cmdServiceStart.Location = new System.Drawing.Point(273, 55);
			this.cmdServiceStart.Name = "cmdServiceStart";
			this.cmdServiceStart.Size = new System.Drawing.Size(115, 22);
			this.cmdServiceStart.TabIndex = 3;
			this.cmdServiceStart.Text = "&Start";
			this.cmdServiceStart.Click += new System.EventHandler(this.cmdServiceStart_Click);
			// 
			// lblServiceStatus
			// 
			this.lblServiceStatus.Location = new System.Drawing.Point(277, 29);
			this.lblServiceStatus.Name = "lblServiceStatus";
			this.lblServiceStatus.Size = new System.Drawing.Size(108, 16);
			this.lblServiceStatus.TabIndex = 2;
			// 
			// lblServicesList
			// 
			this.lblServicesList.Location = new System.Drawing.Point(9, 7);
			this.lblServicesList.Name = "lblServicesList";
			this.lblServicesList.Size = new System.Drawing.Size(178, 15);
			this.lblServicesList.TabIndex = 1;
			this.lblServicesList.Text = "Controlled services list:";
			// 
			// lstServices
			// 
			this.lstServices.Location = new System.Drawing.Point(9, 26);
			this.lstServices.Name = "lstServices";
			this.lstServices.Size = new System.Drawing.Size(259, 108);
			this.lstServices.TabIndex = 0;
			this.lstServices.UseTabStops = false;
			// 
			// tabBackup
			// 
			this.tabBackup.Controls.Add(this.pnlBackup);
			this.tabBackup.Controls.Add(this.chkIsDoBackup);
			this.tabBackup.Location = new System.Drawing.Point(4, 22);
			this.tabBackup.Name = "tabBackup";
			this.tabBackup.Size = new System.Drawing.Size(392, 142);
			this.tabBackup.TabIndex = 0;
			this.tabBackup.Text = "Backup";
			// 
			// pnlBackup
			// 
			this.pnlBackup.Controls.Add(this.lblPath);
			this.pnlBackup.Controls.Add(this.chkRelative);
			this.pnlBackup.Controls.Add(this.cmdBrowseBackupFolder);
			this.pnlBackup.Controls.Add(this.txtBackupFolder);
			this.pnlBackup.Controls.Add(this.label1);
			this.pnlBackup.Location = new System.Drawing.Point(10, 51);
			this.pnlBackup.Name = "pnlBackup";
			this.pnlBackup.Size = new System.Drawing.Size(378, 87);
			this.pnlBackup.TabIndex = 5;
			// 
			// lblPath
			// 
			this.lblPath.Location = new System.Drawing.Point(9, 71);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(359, 11);
			this.lblPath.TabIndex = 9;
			// 
			// chkRelative
			// 
			this.chkRelative.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkRelative.Location = new System.Drawing.Point(254, 22);
			this.chkRelative.Name = "chkRelative";
			this.chkRelative.Size = new System.Drawing.Size(76, 17);
			this.chkRelative.TabIndex = 8;
			this.chkRelative.Text = "Relative:";
			this.chkRelative.CheckedChanged += new System.EventHandler(this.chkRelative_CheckedChanged);
			// 
			// cmdBrowseBackupFolder
			// 
			this.cmdBrowseBackupFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.cmdBrowseBackupFolder.Location = new System.Drawing.Point(338, 42);
			this.cmdBrowseBackupFolder.Name = "cmdBrowseBackupFolder";
			this.cmdBrowseBackupFolder.Size = new System.Drawing.Size(32, 22);
			this.cmdBrowseBackupFolder.TabIndex = 7;
			this.cmdBrowseBackupFolder.Text = "...";
			this.cmdBrowseBackupFolder.Click += new System.EventHandler(this.cmdBrowseBackupFolder_Click);
			// 
			// txtBackupFolder
			// 
			this.txtBackupFolder.Location = new System.Drawing.Point(8, 42);
			this.txtBackupFolder.Name = "txtBackupFolder";
			this.txtBackupFolder.Size = new System.Drawing.Size(323, 20);
			this.txtBackupFolder.TabIndex = 6;
			this.txtBackupFolder.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBackupFolder_KeyUp);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(153, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Set backup destination folder:";
			// 
			// chkIsDoBackup
			// 
			this.chkIsDoBackup.Checked = true;
			this.chkIsDoBackup.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIsDoBackup.Location = new System.Drawing.Point(11, 32);
			this.chkIsDoBackup.Name = "chkIsDoBackup";
			this.chkIsDoBackup.Size = new System.Drawing.Size(289, 18);
			this.chkIsDoBackup.TabIndex = 3;
			this.chkIsDoBackup.Text = "Backup files before installaion";
			this.chkIsDoBackup.CheckedChanged += new System.EventHandler(this.chkIsDoBackup_CheckedChanged);
			// 
			// tabAbout
			// 
			this.tabAbout.Controls.Add(this.lblCapVersion);
			this.tabAbout.Controls.Add(this.lblVersion);
			this.tabAbout.Location = new System.Drawing.Point(4, 22);
			this.tabAbout.Name = "tabAbout";
			this.tabAbout.Size = new System.Drawing.Size(392, 142);
			this.tabAbout.TabIndex = 2;
			this.tabAbout.Text = "About";
			// 
			// lblCapVersion
			// 
			this.lblCapVersion.Location = new System.Drawing.Point(12, 87);
			this.lblCapVersion.Name = "lblCapVersion";
			this.lblCapVersion.Size = new System.Drawing.Size(46, 14);
			this.lblCapVersion.TabIndex = 1;
			this.lblCapVersion.Text = "Version:";
			// 
			// lblVersion
			// 
			this.lblVersion.Location = new System.Drawing.Point(70, 87);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(315, 14);
			this.lblVersion.TabIndex = 1;
			// 
			// cmdExit
			// 
			this.cmdExit.Location = new System.Drawing.Point(336, 184);
			this.cmdExit.Name = "cmdExit";
			this.cmdExit.Size = new System.Drawing.Size(72, 24);
			this.cmdExit.TabIndex = 1;
			this.cmdExit.Text = "&Save";
			this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Location = new System.Drawing.Point(256, 184);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(72, 24);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "&Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// ConfigForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 214);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdExit);
			this.Controls.Add(this.tabSettings);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfigForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Hotfix configuration utility";
			this.tabSettings.ResumeLayout(false);
			this.tabServices.ResumeLayout(false);
			this.tabBackup.ResumeLayout(false);
			this.pnlBackup.ResumeLayout(false);
			this.pnlBackup.PerformLayout();
			this.tabAbout.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private HotfixSettings		objSettigns;
		private ServiceProcessor	objServiceProcessor;
		private Logger				log = new Logger(Logger.GetNewLogFilename());

		public ConfigForm()
		{

			InitializeComponent();
			this.lblVersion.Text = Application.ProductVersion.ToString();
			this.Text += " " + this.lblVersion.Text;
			#region Try to load settings file
			try
			{
				objSettigns = HotfixSettings.load(HotfixSettings.SETTINGS_FILENAME);
				objServiceProcessor = new ServiceProcessor(objSettigns,log);
			}
			catch(Exception inner)
			{
				string strMessage = string.Format("Failed to load settings file: {0}{1}",
												Environment.NewLine,
												Path.Combine(Application.StartupPath,
															 HotfixSettings.SETTINGS_FILENAME)
											  );
				throw new System.InvalidOperationException(strMessage,inner);
			}
			#endregion // Try to load settings file

			
		}

		public void SettingsToForm()
		{
			// Load value to backup turn On/Off checkbox
			chkIsDoBackup.Checked	= objSettigns.Backup.blnIsDoBackup;
			
			// Load value to checkbox of "relative/absolute" path
			chkRelative.Checked		= objSettigns.Backup.blnIsRelative; 
			
			// Load value to textbox of relative/absoulte path to folder of backup
			txtBackupFolder.Text= objSettigns.Backup.strFolder;

			// Load list of controlled services
			FillServicesListbox();
			
		}
		public void SettignsFromForm()
		{
			objSettigns.Backup.blnIsDoBackup = chkIsDoBackup.Checked;
			objSettigns.Backup.blnIsRelative=chkRelative.Checked;
			objSettigns.Backup.strFolder = txtBackupFolder.Text ;
		}

		public bool IsSettingsChanged()
		{
			return 
			(objSettigns.Backup.blnIsDoBackup != chkIsDoBackup.Checked ||
			objSettigns.Backup.blnIsRelative != chkRelative.Checked ||
			objSettigns.Backup.strFolder != txtBackupFolder.Text) ;
		}
		
		// Load list of controlled services
		private void FillServicesListbox()
		{
			int intPrevSelected = lstServices.SelectedIndex;
			lstServices.Items.Clear();
			IEnumerator ServiceEnum = objServiceProcessor.GetEnumerator();
			while(ServiceEnum.MoveNext())
			{
				lstServices.Items.Add((ServiceProcessor.NTService)ServiceEnum.Current);
			}
			if(intPrevSelected>=0 && intPrevSelected < lstServices.Items.Count)
			{
				lstServices.SelectedIndex=intPrevSelected;
			}
		}
		
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		[STAThread]
		static void Main() 
		{
			ConfigForm main;
			try
			{
				main = new ConfigForm();
			}
			catch(Exception thrown)
			{
				main=null;
				string strMessage = string.Format("Message: {0}{1}{0}{0}Internal Error:{0}{2}", 
													Environment.NewLine,
													thrown.Message,thrown.InnerException.Message);
				MessageBox.Show(strMessage);
			}
			if(main != null)
			{
				main.SettingsToForm();
				Application.Run(main);
			}
			
			Application.Exit();
		}

		private void cmdExit_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show("Do you want to save configuration?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SettignsFromForm();
				objSettigns.save(HotfixSettings.SETTINGS_FILENAME);
			}
			this.Close();
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			if(IsSettingsChanged())
				if(MessageBox.Show("Do you want to save configuration?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
				{
					SettignsFromForm();
					objSettigns.save(HotfixSettings.SETTINGS_FILENAME);
				}
			this.Close();		
		}

		private void cmdBrowseBackupFolder_Click(object sender, System.EventArgs e)
		{
			fbdBackup.Description = "Select backup destination folder:";
			fbdBackup.SelectedPath = txtBackupFolder.Text;
			DialogResult result = fbdBackup.ShowDialog();
			if(result == DialogResult.OK)
			{
				txtBackupFolder.Text = fbdBackup.SelectedPath;
			}
		}

		private void chkIsDoBackup_CheckedChanged(object sender, System.EventArgs e)
		{
			pnlBackup.Visible = chkIsDoBackup.Checked;
			
		}

		private void chkRelative_CheckedChanged(object sender, System.EventArgs e)
		{
			cmdBrowseBackupFolder.Visible = !chkRelative.Checked;
			txtBackupFolder.SelectAll();
			lblPath.Visible=chkRelative.Checked;
		}

		private void cmdRefresh_Click(object sender, System.EventArgs e)
		{
			FillServicesListbox();
		}

		private void cmdServiceStart_Click(object sender, System.EventArgs e)
		{
			ServiceProcessor.NTService service;
			if(lstServices.SelectedIndex >=0)
			{
				this.Cursor= Cursors.WaitCursor;
				service = ((ServiceProcessor.NTService)lstServices.SelectedItem);
				objServiceProcessor.SetServiceStatus(service.service,System.ServiceProcess.ServiceControllerStatus.Running);
				FillServicesListbox();
				this.Cursor = Cursors.Arrow;
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			ServiceProcessor.NTService service;
			if(lstServices.SelectedIndex >=0)
			{
				this.Cursor= Cursors.WaitCursor;
				service = ((ServiceProcessor.NTService)lstServices.SelectedItem);
				objServiceProcessor.SetServiceStatus(service.service,System.ServiceProcess.ServiceControllerStatus.Stopped);
				FillServicesListbox();
				this.Cursor = Cursors.Arrow;
			}		
		}

		private void txtBackupFolder_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(chkRelative.Checked)
			{
				try
				{
					DirectoryInfo dir = new DirectoryInfo( Path.Combine(Application.StartupPath,txtBackupFolder.Text));
					lblPath.Text = dir.FullName;
				}
				catch
				{
					lblPath.Text = "Wrong path ...";
				}
			}		
		}




	}
}
