using System.Linq;
using System.Net;
using Business.Common;

namespace Business.Common.Logging {
	public static class MachineInfo {
		public static string ComputerName { get { return Dns.GetHostName(); } }

		public static string IP_Address {
			get {
				var localIPs = Dns.GetHostAddresses(ComputerName);
				if (localIPs.Any()) {
					return Conversion.ArrayToCsv(localIPs);
				}
				return string.Empty;
			}
		}
	}
}