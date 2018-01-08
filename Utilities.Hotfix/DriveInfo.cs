using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Utilities.Hotfix
{
	public struct DriveInfoSystem
	{
		public readonly string Drive;
		public readonly long Result;
		public readonly long Available;
		public readonly long Total;
		public readonly long Free;

		public DriveInfoSystem(string drive, long result, long available, long total, long free)
		{
			this.Drive = drive;
			this.Result = result;
			this.Available = available;
			this.Total = total;
			this.Free = free;
		}

		public static long ToMB(long bytes)
		{
			return (long)(Convert.ToDouble(bytes)/1024/1024);
		}
	}

	/// <summary>
	/// Helper class to get info about available/free/occupied space on specific drive or folder
	/// </summary>
	public sealed class DriveInfo
	{
		[DllImport("kernel32.dll", EntryPoint = "GetDiskFreeSpaceExA")]
		private static extern long GetDiskFreeSpaceEx(    string	lpDirectoryName,
														out long	lpFreeBytesAvailableToCaller,
														out long	lpTotalNumberOfBytes,
														out long	lpTotalNumberOfFreeBytes);

		public static DriveInfoSystem GetInfo(string drive)
		{
			long result, available, total, free;
			result = GetDiskFreeSpaceEx(drive, out available, out total, out free);
			return new DriveInfoSystem(drive, result, available, total, free);
		}

		/// <summary>
		/// Gets the info.
		/// </summary>
		/// <param name="drive">[IN] The drive letter.</param>
		/// <param name="available">[OUT] The available free space in bytes.</param>
		/// <param name="total">[OUT] The total space in bytes.</param>
		/// <param name="free">[OUT] The free space in bytes.</param>
		/// <returns></returns>
		public static long GetInfo(string drive, out long available, out long total, out long free)
		{
			return GetDiskFreeSpaceEx(drive, out available, out total, out free);
		}

		/// <summary>
		/// Gets the size of the folder in bytes.
		/// </summary>
		/// <param name="objFolder">The folder object</param>
		/// <returns></returns>
		public static long GetFolderSize(DirectoryInfo objFolder)
		{
			long lngResultBytes=0;
			
			DirectoryInfo[] arrFolders = objFolder.GetDirectories();
			foreach(DirectoryInfo sub in arrFolders)
			{
				lngResultBytes += GetFolderSize(sub);
			}
			
			FileInfo[] arrFiles = objFolder.GetFiles();
			foreach(FileInfo file in arrFiles)
			{
				lngResultBytes += file.Length;
			}

			return lngResultBytes;
		}

	}



	
}
