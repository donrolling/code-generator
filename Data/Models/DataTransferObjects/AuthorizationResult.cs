using Business.Common.Statuses;

namespace Data.Models.DataTransferObjects {
	public class AuthorizationResult {
		public bool Authorized {
			get {
				return AccessFailureReason == AccessFailureReason.NotFailed ? true : false;
			}
		}

		private AccessFailureReason _accessFailureReason = AccessFailureReason.NotFailed;
		public AccessFailureReason AccessFailureReason {
			get {
				return _accessFailureReason;
			}
			set {
				_accessFailureReason = value;
			}
		}
	}
}