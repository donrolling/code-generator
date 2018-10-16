namespace Data.Models {
	public enum BusinessUnitEnum {
		None,
		Company,
		Division,
		WebProperty,
		WebPropertyModule
	}

	public enum SystemRole {
		SystemAdministrator,
		CompanyAdministrator,
		DivisionAdministrator,
		WebPropertyAdministrator,
		WebPropertyModuleAdministrator,
		Guest,
	}
}