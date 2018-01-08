using System;
using System.ServiceProcess;
using System.Collections;

namespace Utilities.Hotfix
{
	/// <summary>
	/// Class implements functionality of stopping/starting NT services
	/// related to configuration for specific hotfix. 
	/// </summary>
	public class ServiceProcessor: IEnumerable
	{
		ArrayList		Services	= new ArrayList();
		HotfixSettings	settings;
		Logger			log;
		public string					strFailedList;

		/// <summary>
		/// Wrapped ServiceController class with memory of before/after status
		/// </summary>
		public class NTService
		{
			public ServiceController		service;
			public ServiceControllerStatus	StatusBefore;
			public ServiceControllerStatus	StatusAfter;
			public int						intStartOrder;
			public int						intStopOrder;
			public string					strServiceName;


			public NTService(	ServiceController		service,
								ServiceControllerStatus	StatusBefore, 
								int						intStartOrder,
								int						intStopOrder,
								string					strServiceName
							 )
			{
				this.service		=	service;
				this.StatusBefore	=	StatusBefore;
				// initial after status equal to before status
				this.StatusAfter	=	StatusBefore;
				this.intStartOrder	=	intStartOrder;
				this.intStopOrder	=	intStopOrder;
				this.strServiceName =	strServiceName;
			}

			public override string ToString()
			{
				string strToString;
				strToString = string.Format("{0}: ({1})",this.service.ServiceName,this.service.Status.ToString().ToLower()); 
				return strToString;
			}

		}

		
		/// <summary>
		/// Initializes service processor instance by only those services
		/// that installed on processed machine and appeared in settings of hotfix
		/// </summary>
		/// <param name="settings">Settings object of hotfix</param>
		/// <param name="log">Logger object</param>
		public ServiceProcessor(HotfixSettings settings, Logger log)
		{
			this.settings = settings;
			this.log = log;

			//-------------------------------------------------------
			// Get list of installed services
			// TODO: remote access
			//-------------------------------------------------------
			ServiceController[] existingServices = ServiceController.GetServices();
			
			//-------------------------------------------------------
			// Sort array in ascending order to be able to use binary search 
			//-------------------------------------------------------
			Array.Sort(existingServices, 0 ,existingServices.Length, new ServiceComparer());
			
			foreach(HotfixSettings.Service service in this.settings.Services)
			{
				int index = -1;

				index = Array.BinarySearch(existingServices, 0, existingServices.Length,
											new ServiceController(service.SERVICE_NAME),
											new ServiceComparer());
				if(index > 0)
				{
					// Some NT Service with given name found 
					NTService objService = new NTService(
						existingServices[index],
						existingServices[index].Status,
						service.intStartOrder,
						service.intStopOrder,
						service.SERVICE_NAME
						);
					
					// Add found service to processed list of services
					Services.Add(objService);
				}
			}
		}

		/// <summary>
		/// Custom comparer that implements case insensitive comparasion
		/// of Services by SERVICE_NAME.
		/// </summary>
		private class ServiceComparer : IComparer  
		{
			private static CaseInsensitiveComparer mobjCaseInsensitiveComparer = new CaseInsensitiveComparer();
			
			//Calls CaseInsensitiveComparer.Compare
			int IComparer.Compare( Object x, Object y )  
			{
				// if service that compared actually is not installed then
				// during comparison exception will be thrown
				try
				{
					return(
						mobjCaseInsensitiveComparer.Compare(
						((ServiceController)x).ServiceName ,
						((ServiceController)y).ServiceName 
						));
				}
				catch(Exception)
				{
					return -1; //not equal
				}
			}

		}


		/// <summary>
		/// Comaparer of wrapped controlled services by StartOrder property
		/// to be able to start services in appropriate start order
		/// </summary>
		private class ServiceStartOrderComparer : IComparer
		{
			int IComparer.Compare( Object x, Object y )  
			{
				try
				{
					return
						((NTService)x).intStartOrder - 
						((NTService)y).intStartOrder ;
				}
				catch(Exception)
				{
					return -1; //not equal
				}
			}
		}


		/// <summary>
		/// Comaparer of wrapped controlled services by StopOrder property
		/// to be able to stop services in appropriate stop order
		/// </summary>
		private class ServiceStopOrderComparer: IComparer
		{
			#region IComparer Members

			public int Compare(object x, object y)
			{
				try
				{
					return
						((NTService)x).intStopOrder - 
						((NTService)y).intStopOrder ;
				}
				catch(Exception)
				{
					return -1; //not equal
				}
			}
			#endregion
		}


		/// <summary>
		/// Set status of <see cref="controlled"/> services to <see cref="WantedStatus"/>
		/// </summary>
		/// <param name="controlled"></param>
		/// <param name="WantedSatatus"></param>
		/// <returns>True  - <see cref="WantedStatus"/> reached
		///          False - time-out expired during try</returns>
		public virtual bool SetServiceStatus(ServiceController controlled, 
											ServiceControllerStatus WantedSatatus)
		{
			TimeSpan		timeout		= new TimeSpan(0,1,30); //TODO: parameter!!!

			bool IsStatusReached = (WantedSatatus == controlled.Status);

			if( !IsStatusReached )
			{
				switch(WantedSatatus)
				{
					case ServiceControllerStatus.Stopped: 
						System.Diagnostics.Debug.Assert(controlled.CanStop,
														"Service " + controlled.DisplayName + " cannot be stopped");
						controlled.Stop(); 
						break;
					
					case ServiceControllerStatus.Running: 
						controlled.Start(); 
						break;
					
					case ServiceControllerStatus.Paused: 
						System.Diagnostics.Debug.Assert(controlled.CanPauseAndContinue,
														"Service " + controlled.ServiceName + " cannot be paused\\continued");
						controlled.Pause(); 
						break;
					
					default:
						System.Diagnostics.Debug.Assert(false,"Unsupported Status: " + WantedSatatus);
						break;
				}
				
				//
				// Wait for wanted status or for the specified time-out to expire
				// See also 
				try
				{
					controlled.WaitForStatus(WantedSatatus, timeout);
				}
				catch(System.ServiceProcess.TimeoutException)
				{
					log.WriteLine(LogType.Error, "HI021: " + 
						string.Format("TIMEOUT: Service \"{0}\" failed to reach status of {1} in given time",controlled.ServiceName,WantedSatatus.ToString()));
				}
				catch(Exception objException)
				{
					log.WriteLine(LogType.Error, "HI051: " + 
						string.Format("Service \"{0}\" failed to reach status of {1}, Error: {2} ",
										controlled.ServiceName,
										WantedSatatus.ToString(),
										objException.ToString()));
				}
				finally
				{
					IsStatusReached = ( WantedSatatus == controlled.Status );
				}
			}
			return IsStatusReached;
		}


		/// <summary>
		/// Stop all controlled services
		/// </summary>
		/// <returns>True if all processed services stopped successfully</returns>
		public bool Stop()
		{
			bool blnIsStoppedStatus = true;
			
			try
			{
				Services.Sort(0, Services.Count, new ServiceStopOrderComparer());

				foreach(NTService wrapped in Services)
				{
					ServiceController controlled = wrapped.service;
					
					wrapped.StatusBefore = controlled.Status;
					
					if( controlled.CanStop )
					{
						SetServiceStatus(controlled, ServiceControllerStatus.Stopped);
					}
					
					//save "after Status" after the try of set new Status
					wrapped.StatusAfter = controlled.Status;

					//accumulate Status of processed service
					blnIsStoppedStatus &= (wrapped.StatusAfter == ServiceControllerStatus.Stopped);

					log.WriteLine(LogType.Info, "HI022: " +
						String.Format("Service: \"{0}\" : Current Status: {1} , Previous Status: {2}",
						wrapped.strServiceName,
						wrapped.StatusAfter.ToString().ToUpper(),
						wrapped.StatusBefore.ToString().ToUpper()));
				}
			}
			catch(Exception objException)
			{
				blnIsStoppedStatus = false;
				log.WriteLine(LogType.Error, "HI023: " +
					String.Format("Failed stopping of controlled services: {1} ",objException.ToString()));
			}

			return blnIsStoppedStatus;
		}


		/// <summary>
		/// Restore controlled services to Status before processing
		/// </summary>
		/// <returns>True - All processed service restored to "before" Status</returns>
		public bool Restore()
		{
			
			bool blnIsRestored = true;
			strFailedList = string.Empty;

			Services.Sort(0, Services.Count, new ServiceStartOrderComparer());

			foreach(NTService wrapped in Services)
			{
				try
				{
						ServiceController controlled = wrapped.service;
					
						if(wrapped.StatusBefore != wrapped.StatusAfter )
						{
							SetServiceStatus(wrapped.service, wrapped.StatusBefore);
							
							wrapped.StatusAfter = controlled.Status;
							
							log.WriteLine(LogType.Info, "HI024: " +
								String.Format("Service: \"{0}\" : Current Status: {1} , Previous Status: {2}",
												wrapped.strServiceName,
												wrapped.StatusAfter.ToString().ToUpper(),
												wrapped.StatusBefore.ToString().ToUpper()));
						}
						
					
						//accumulate Status of processed service after applied action
						blnIsRestored &= (wrapped.StatusAfter == wrapped.StatusBefore);


				}
				catch //Exception objException)
				{
					// see http://support.microsoft.com/kb/839174
					blnIsRestored = false;
					log.WriteLine(LogType.Error, "HI025: " +
						String.Format("Failed restoration of controlled service: {0} ", wrapped.strServiceName));
					
					// collect names of services failed to start up to try
					// start them up after
					strFailedList += "\"" + wrapped.strServiceName + "\" ";
				}
			}

			return blnIsRestored;
		}
		
		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return Services.GetEnumerator();
		}

		#endregion
	}

}
