using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace StorageManagementCore.Backend {
	public class Services {
		private class ServiceHierarchy {
			public List<ServiceHierarchy> Members;
			public ServiceController Me;

			private ServiceHierarchy() { }

			public ServiceHierarchy(ServiceController me) {
				this.Me = me;
				Members = new List<ServiceHierarchy>();
			}

			public static ServiceHierarchy AllStarting(ServiceController me) {
				ServiceHierarchy toReturn = new ServiceHierarchy() {
					Me=me,
					Members = me.DependentServices
						.Where(x => x.Status == ServiceControllerStatus.Running || x.Status == ServiceControllerStatus.StartPending)
						.Select(AllStarting).ToList()
				};
				return toReturn;
			}
		}

		/// <summary>
		///  Starts all ServiceControllers depending on the given one
		/// </summary>
		/// <param name="toStart"></param>
		/// <returns>Whether the operation were successful</returns>
		private static bool RecursiveServiceStarter(ServiceHierarchy toStart) {
			if (!toStart.Members.All(RecursiveServiceStarter)) {
				return false;
			}

			try {
				toStart.Me.Start();
			}
			catch (Exception) {
				return false;
			}

			return true;
		}

		/// <summary>
		///  Kills first all depnding ServiceControllers and then itselves
		/// </summary>
		/// <param name="toKill">The ServiceController to kill</param>
		/// <returns>Whether the operation were successful</returns>
		private static bool RecursiveServiceKiller(ServiceController toKill, out ServiceHierarchy data) {
			data = new ServiceHierarchy(toKill);
			foreach (ServiceController killDependentService in toKill.DependentServices) {
				if (killDependentService.Status == ServiceControllerStatus.Running) {
					if (killDependentService.CanStop && RecursiveServiceKiller(killDependentService, out ServiceHierarchy s)) {
						data.Members.Add(s);
					}
					else {
						return false;
					}
				}
				else if (killDependentService.Status == ServiceControllerStatus.StartPending) { }
			}

			try {
				toKill.Stop();
			}
			catch (Exception) {
				return false;
			}

			return true;
		}

		/// <summary>
		///  Restarts a service and all depending services
		/// </summary>
		/// <param name="toRestart">The service to restart</param>
		/// <returns>Whether the operation were successful</returns>
		public static bool RecursiveServiceRestart(ServiceController toRestart) {
			if (!RecursiveServiceKiller(toRestart,out ServiceHierarchy hrc)) {
				return false;
			}
			else {
				if (!RecursiveServiceStarter(hrc)) {
					return false;
				}
				else {
					return true;
				}
			}
		}
	}
}