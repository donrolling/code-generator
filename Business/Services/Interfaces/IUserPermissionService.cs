namespace Business.Service.Interfaces {
	public interface IUserPermissionService {
		bool IsInRole(long userId, string systemRole);
	}
}
