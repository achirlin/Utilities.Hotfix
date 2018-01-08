using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;

//Changes:
//	11.06.2007	Changed output format
//	01.07.2007	Changed output format, fixed error caused by using of MM in time footprint
//  04.09.2007  Fixed GetNewLogFilename()

namespace Utilities.Hotfix{
	
	// Logger class 
	
	public enum LogType {
		
		Info = 1,
		Warning = 2,
		Error = 3,
		Empty =4
	}
	
	public class Logger {
		
		// Privates
		private bool			isReady=false;
		private StreamWriter	swLog;
		private string			strLogFile;
		
		/// <summary>
		/// get current full qualified name of log file
		/// </summary>
		public string LogFileName
		{
			get
			{
				return strLogFile;
			}
		}

		/// <summary>
		/// Create logger object with default file name
		/// </summary>
		public Logger():this(GetNewLogFilename())
		{
		}

		// Constructors
		public Logger(string LogFileName) 
			{
			this.strLogFile = LogFileName;
			openFile();
			_writelog("");
			closeFile();
		}
		
		private void openFile() {
			try {
				swLog = File.AppendText(strLogFile);		
				isReady = true;
			} catch {
				isReady = false;
			}			
		}
		
		private void closeFile() {
			
			if(isReady) {
				try {
					swLog.Close();
				} catch {
				}
			}
		}
				
		public static string GetNewLogFilename() {
			AppDomain Ad = AppDomain.CurrentDomain;
			return System.IO.Path.Combine(Ad.BaseDirectory,DateTime.Now.ToString("dd-MM-yyyy") + ".log");
		}
		
		public void WriteLine(LogType logtype,string message) {
			// see ms-help://MS.MSDNQTR.2006JAN.1033/cpref/html/frlrfsystemglobalizationdatetimeformatinfoclasstopic.htm
			string stub = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss ", DateTimeFormatInfo.InvariantInfo);

			switch(logtype) 
			{
				case LogType.Info:
					stub += "   Info, ";
					break;
				case LogType.Warning:
					stub += "Warning, ";
					break;
				case LogType.Error:
					stub += "  Error, ";
					break;
				case LogType.Empty:
					stub = "";
					break;
			}

			string	strBlank = new string(' ',stub.Length);
			int intLastEndl = message.LastIndexOf(Environment.NewLine);
			if(intLastEndl > -1)
			{
				message = message.Substring(0,intLastEndl);
				message = message.Replace(Environment.NewLine,Environment.NewLine + strBlank);
				stub += message;
			}
			else
			{
				stub += message;
			}
			openFile();
			_writelog(stub);
			closeFile();
		}
		
		private void _writelog(string msg) {
			if(isReady) {
				swLog.WriteLine(msg);
			} else {
				Console.WriteLine("Error Cannot write to log file.");
			}					
		}

		/// <summary>
		/// Open log file in notepad.exe
		/// </summary>
		public void Show()
		{
			// Shell Commands within C# see example:
			// http://www.c-sharpcorner.com/UploadFile/DipalChoksi/ShellCommandsInCS12032005042031AM/ShellCommandsInCS.aspx
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			
			try
			{
				proc.EnableRaisingEvents=false;
			
				proc.StartInfo.FileName="notepad.exe";
				proc.StartInfo.UseShellExecute=false;
				proc.StartInfo.Arguments=strLogFile;
				proc.StartInfo.WindowStyle= ProcessWindowStyle.Normal;
			
				//asynchronous start of application
				proc.Start();
			}
			catch
			{
				Console.WriteLine("Unable to open Notepad with created log.");
			}

		}
	}
}
