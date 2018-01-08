using System;
using NUnit.Framework;
using System.Xml;
using System.IO;

namespace Utilities.Hotfix.UNITests
{
	/// <summary>
	/// Summary description for Construct.
	/// </summary>
	[TestFixture]
	public class UnitTest
	{
		public UnitTest()
		{
		}

		[TestFixtureSetUp]
		public virtual void Init()
		{
			Console.WriteLine("Base Test class constructed");
		}

		[TestFixtureTearDown]
		public virtual void Dispose()
		{
			Console.WriteLine("Base Test class disposed");
		}

		protected Stream GetResourceStream(string strResourceName)
		{
			return GetType().Module.Assembly.GetManifestResourceStream(GetResourceFullName(strResourceName));
		}
		
		protected string GetResourceFullName(string resName)
		{
			string		fullName	= null;
			string[]	names		= GetType().Module.Assembly.GetManifestResourceNames();
			foreach(string str in names)
			{
				if(str.EndsWith(resName))
				{
					fullName = str;
					break;
				}
			}
			return fullName;
		}

		protected void GetResourceToFile(string resourceFileName)
		{
			GetResourceToFile(resourceFileName,resourceFileName);
		}
		protected void GetResourceToFile(string resourceFileName,string destinationFileName)
		{
			string fullName = this.GetResourceFullName(resourceFileName);
			if(fullName == null)
			{
				throw new Exception("Can not find "+resourceFileName+" resource in container file");
			}
			
			Stream file			= GetType().Module.Assembly.GetManifestResourceStream(fullName);
			FileStream outFile	= new FileStream(destinationFileName,FileMode.Create);
			int bufferLen		= 1024; 
			
			byte[] buffer = new byte[bufferLen]; 
			int bytesRead; 
			do 
			{ 
				bytesRead = file.Read(buffer, 0, bufferLen); 
				outFile.Write(buffer, 0, bytesRead); 
			} while(bytesRead != 0); 
			outFile.Close();
		}

	}
}
