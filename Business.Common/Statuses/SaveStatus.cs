namespace Business.Common.Statuses {
	public enum Status {
		Success,
		Failure,
		ItemNotFound
	}

	public enum StatusDetail {
		New,
		Duplicate,
		Error,
		ItemNotFound,
		Unknown,
		Invalid,
		ItemHasChildren,
		OK
	}
}