using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace StorageManagementCore.Backend {
	public class Services {
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
		///  Kills first all depending ServiceControllers and then itself
		/// </summary>
		/// <param name="toKill">The ServiceController to kill</param>
		/// <param name="data">The Hierarchy of dependent processes discovered</param>
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
			if (!RecursiveServiceKiller(toRestart, out ServiceHierarchy hrc)) {
				return false;
			}

			if (!RecursiveServiceStarter(hrc)) {
				return false;
			}

			return true;
		}

		private class ServiceHierarchy {
			public readonly ServiceController Me;
			public readonly List<ServiceHierarchy> Members;

			private ServiceHierarchy() { }

			public ServiceHierarchy(ServiceController me) {
				Me = me;
				Members = new List<ServiceHierarchy>();
			}
		}
	}
}