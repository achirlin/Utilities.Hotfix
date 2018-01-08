#undef NET2
#if NET2
	using System.Collections.Generic;
#else
	using System.Collections;
#endif
using System;
using System.IO;using System.Text;

// I tried using Microsoft's MSDN Online to search for a way to copy directories 
// with their subfolders and files, when I determined that the Directory.Copy() 
// method did not exist natively.
//#
//# Iterative Version of xDirectory.Copy() 
//# Changes:
//#		Created:
//#		18/05/2006	John D. Storer II, CodeProject, http://www.codeproject.com/cs/files/xdirectorycopy.asp
//#		03/06/2007	Alexander Chirlin, Backward adapted to .NET 1.1, to set compatible for
//#					.NET 2.0 use precompilation variable 
//#		11/07/2007	Alexander Chirlin, Added processing of excluded files

namespace Utilities.Hotfix
{
	/// <summary>
    /// xDirectory v2.0 - Copy a Source Directory and it's SubDirectories/Files.
    /// Coder: John Storer II
    /// Date: Thursday, May 18, 2006
    /// </summary>

    public class xDirectory
    {
        /// <summary>
        /// Default Overwrite Value - Change to Preference.
        /// </summary>
        private static bool _DefaultOverwrite = true;

        /// <summary>
        /// Default Folder Iteration Limit - Change to Preference.
        /// </summary>
        private static int _DefaultIterationLimit = 1000000;

		/// <summary>
		/// Array of standard file extentions of files excluded from copying
		/// </summary>
		public static string[] arrExcludeFileFilters	= new string[]{};

		///////////////////////////////////////////////////////////
        /////////////////// String Copy Methods ///////////////////
        ///////////////////////////////////////////////////////////

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="sSource">The Source Directory</param>
        /// <param name="sDestination">The Destination Directory</param>
        public static void Copy(string sSource, string sDestination)
        {
            Copy(new DirectoryInfo(sSource), new DirectoryInfo(sDestination), null, null, _DefaultOverwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="sSource">The Source Directory</param>
        /// <param name="sDestination">The Destination Directory</param>
        /// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
        public static void Copy(string sSource, string sDestination, bool Overwrite)
        {
            Copy(new DirectoryInfo(sSource), new DirectoryInfo(sDestination), null, null, Overwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="sSource">The Source Directory</param>
        /// <param name="sDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        public static void Copy(string sSource, string sDestination, string FileFilter)
        {
            Copy(new DirectoryInfo(sSource), new DirectoryInfo(sDestination), FileFilter, null, _DefaultOverwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="sSource">The Source Directory</param>
        /// <param name="sDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
        public static void Copy(string sSource, string sDestination, string FileFilter, bool Overwrite)
        {
            Copy(new DirectoryInfo(sSource), new DirectoryInfo(sDestination), FileFilter, null, Overwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="sSource">The Source Directory</param>
        /// <param name="sDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        public static void Copy(string sSource, string sDestination, string FileFilter, string DirectoryFilter)
        {
            Copy(new DirectoryInfo(sSource), new DirectoryInfo(sDestination), FileFilter, DirectoryFilter, _DefaultOverwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="sSource">The Source Directory</param>
        /// <param name="sDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
        public static void Copy(string sSource, string sDestination, string FileFilter, string DirectoryFilter, bool Overwrite)
        {
            Copy(new DirectoryInfo(sSource), new DirectoryInfo(sDestination), FileFilter, DirectoryFilter, Overwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="sSource">The Source Directory</param>
        /// <param name="sDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
        /// <param name="FolderLimit">Iteration Limit - Total Number of Folders/SubFolders to Copy</param>
        public static void Copy(string sSource, string sDestination, string FileFilter, string DirectoryFilter, bool Overwrite, int FolderLimit)
        {
            Copy(new DirectoryInfo(sSource), new DirectoryInfo(sDestination), FileFilter, DirectoryFilter, Overwrite, FolderLimit);
        }

        //////////////////////////////////////////////////////////////////
        /////////////////// DirectoryInfo Copy Methods ///////////////////
        //////////////////////////////////////////////////////////////////

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="diSource">The Source Directory</param>
        /// <param name="diDestination">The Destination Directory</param>
        public static void Copy(DirectoryInfo diSource, DirectoryInfo diDestination)
        {
            Copy(diSource, diDestination, null, null, _DefaultOverwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="diSource">The Source Directory</param>
        /// <param name="diDestination">The Destination Directory</param>
        /// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
        public static void Copy(DirectoryInfo diSource, DirectoryInfo diDestination, bool Overwrite)
        {
            Copy(diSource, diDestination, null, null, Overwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="diSource">The Source Directory</param>
        /// <param name="diDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        public static void Copy(DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter)
        {
            Copy(diSource, diDestination, FileFilter, null, _DefaultOverwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="diSource">The Source Directory</param>
        /// <param name="diDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
        public static void Copy(DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter, bool Overwrite)
        {
            Copy(diSource, diDestination, FileFilter, null, Overwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="diSource">The Source Directory</param>
        /// <param name="diDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        public static void Copy(DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter, string DirectoryFilter)
        {
            Copy(diSource, diDestination, FileFilter, DirectoryFilter, _DefaultOverwrite, _DefaultIterationLimit);
        }

        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="diSource">The Source Directory</param>
        /// <param name="diDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
        public static void Copy(DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter, string DirectoryFilter, bool Overwrite)
        {
            Copy(diSource, diDestination, FileFilter, DirectoryFilter, Overwrite, _DefaultIterationLimit);
        }


        /////////////////////////////////////////////////////////////////////
        /////////////////// The xDirectory.Copy() Method! ///////////////////
        /////////////////////////////////////////////////////////////////////


        /// <summary>
        /// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
        /// </summary>
        /// <param name="diSource">The Source Directory</param>
        /// <param name="diDestination">The Destination Directory</param>
        /// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
        /// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
        /// <param name="FolderLimit">Iteration Limit - Total Number of Folders/SubFolders to Copy</param>
		
		// Better Overwrite Test W/O IOException from CopyTo() /////
		//if (Overwrite)
		//	fi.CopyTo(sFilePath, true);
		//else
		//{
		//	///// Prevent Silly IOException /////
		//	if (!File.Exists(sFilePath))
		//		fi.CopyTo(sFilePath, true);
		//}
        public static void Copy(DirectoryInfo diSource, 
								DirectoryInfo diDestination, 
								string FileFilter, 
								string DirectoryFilter, 
								bool Overwrite, 
								int FolderLimit)
        {
			
			#if NET20
				List<DirectoryInfo> diSourceList = new List<DirectoryInfo>();
				List<FileInfo> fiSourceList = new List<FileInfo>();
			#else
				ArrayList diSourceList = new ArrayList();
				ArrayList fiSourceList = new ArrayList();
			#endif
			
			///// Error Checking /////
			if (diDestination == null) 
				throw new ArgumentException("Destination Directory: NULL");
			if (diSource == null) 
				throw new ArgumentException("Source Directory: NULL");
			if (!diSource.Exists) 
				throw new IOException("Source Directory: Does Not Exist");
			if (!(FolderLimit > 0)) 
				throw new ArgumentException("Folder Limit: Less Than 1");
			if (DirectoryFilter == null || DirectoryFilter == string.Empty)
				DirectoryFilter = "*";
			if (FileFilter == null || FileFilter == string.Empty)
				FileFilter = "*";

			try
			{
				Investigate(diSource, ref fiSourceList, ref diSourceList, FileFilter, DirectoryFilter, Overwrite, FolderLimit);

                ///// Second Section: Create Folders from Listing /////
                foreach (DirectoryInfo di in diSourceList)
                {
                    if (di.Exists)
                    {
                        string sFolderPath = diDestination.FullName + @"\" + di.FullName.Remove(0, diSource.FullName.Length);
                        
                        ///// Prevent Silly IOException /////
                        if (!Directory.Exists(sFolderPath))
                            Directory.CreateDirectory(sFolderPath);
                    }
                }

                ///// Third Section: Copy Files from Listing /////
                foreach (FileInfo fi in fiSourceList)
                {
                    if (fi.Exists)
                    {
						string sFilePath = diDestination.FullName + @"\" + fi.FullName.Remove(0, diSource.FullName.Length);
						if (File.Exists(sFilePath))
						{
							FileAttributes fa = File.GetAttributes(sFilePath);
							if ((fa & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
							{
								fa-=FileAttributes.ReadOnly;
								File.SetAttributes(sFilePath,fa);
							}
						}
                        fi.CopyTo(sFilePath, true);
                    }
                }
            }
            catch
            { 
				throw; 
			}
        }


		public static void Investigate(
			DirectoryInfo diSource, 
			ref ArrayList arrFilesList, 
			ref ArrayList arrFoldersList,
			string FileFilter, 
			string DirectoryFilter, 
			bool Overwrite
			)
		{
			Investigate(diSource, ref arrFilesList, ref arrFoldersList,  FileFilter, DirectoryFilter, Overwrite, _DefaultIterationLimit);
		}

		public static void Investigate(
			DirectoryInfo diSource, 
			ref ArrayList arrFilesList, 
			ref ArrayList arrFoldersList
			)
		{
			Investigate(diSource, ref arrFilesList, ref arrFoldersList,  null, null, true, _DefaultIterationLimit);
		}

		public static void Investigate(DirectoryInfo diSource, 
				ref ArrayList arrFilesList, 
				ref ArrayList arrFoldersList,
				string FileFilter, 
				string DirectoryFilter, 
				bool Overwrite, 
				int FolderLimit)
		{
			int iterator = 0;
			#if NET20
				List<DirectoryInfo> diSourceList = new List<DirectoryInfo>();
				List<FileInfo> fiSourceList = new List<FileInfo>();
			#else
				ArrayList diSourceList = new ArrayList();
				ArrayList fiSourceList = new ArrayList();
			#endif

			try
			{
				///// Error Checking /////
				if (diSource == null) 
					throw new ArgumentException("Source Directory: NULL");
				if (!diSource.Exists) 
					throw new IOException("Source Directory: Does Not Exist");
				if (!(FolderLimit > 0)) 
					throw new ArgumentException("Folder Limit: Less Than 1");
				if (DirectoryFilter == null || DirectoryFilter == string.Empty)
					DirectoryFilter = "*";
				if (FileFilter == null || FileFilter == string.Empty)
					FileFilter = "*";

				///// Add Source Directory to List /////
				diSourceList.Add(diSource);

				///// First Section: Get Folder/File Listing /////
				while (iterator < diSourceList.Count && iterator < FolderLimit)
				{
					foreach (DirectoryInfo di in ((DirectoryInfo)diSourceList[iterator]).GetDirectories(DirectoryFilter))
						diSourceList.Add(di);

					foreach (FileInfo fi in ((DirectoryInfo)diSourceList[iterator]).GetFiles(FileFilter))
						fiSourceList.Add(fi);

					iterator++;
				}
				
				if(arrExcludeFileFilters.Length>0)
				{
					fiSourceList.Sort(0, fiSourceList.Count, new xDirectory.FileByFullNameComparer());
					iterator=0;
					while (iterator < diSourceList.Count && iterator < FolderLimit)
					{
						//iterate each exclusion filter
						foreach(string strExcludeFilter in arrExcludeFileFilters)
						{
							// exclude files from prepared list of files
							foreach (FileInfo fi in ((DirectoryInfo)diSourceList[iterator]).GetFiles(strExcludeFilter))
							{
								int index = fiSourceList.BinarySearch(fi,new xDirectory.FileByFullNameComparer());
								if(index > -1)
									fiSourceList.RemoveAt(index);
							}
						}
						iterator++;
					}
				}

				arrFilesList = fiSourceList;
				arrFoldersList = diSourceList;
			}
			catch
			{ 
				throw; 
			}
		}

		class FileByFullNameComparer : IComparer
		{
			#region IComparer Members

			public int Compare(object x, object y)
			{
				return ((FileInfo)x).FullName.CompareTo(((FileInfo)y).FullName);
			}

			#endregion
		}
	
    }
}
