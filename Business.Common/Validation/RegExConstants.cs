namespace Business.Common.Validation {
	public class RegExConstants {
		//this phone expression allows for extensions as well as parens
		public const string PhoneNumber = @"^\(?\d{3}[) ]\s?\d{3}[- ]\d{4}(\s[ext]\d{1,4})?$";
		public const string Fax = @"(^(1)?[\s\-]?([\(]?[0-9]{1,3}[\)]?)?[ ]{0,2}([0-9])?[ ]{0,2}[0-9]{2,}[1-9]+[0-9 \\-]*$)|(|(\(___\)\s___\-____\sx____\))";
		public const string Email = @"^[A-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-z0-9!#$%&'*+/=?^_`{|}~-]+)*@([A-z0-9-]+(\.[A-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$";
		public const string EmailList = @"^([\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@([\w-]+(\.[\w-]+)*?\.[A-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?(,)?(\s)?\n?)*$";
		public const string Zip = @"^\d{5}([\-]\d{4})?$";
		public const string Date = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
		public const string Password = @"^.*(?=.{6,12})(?=.*[A-z])(?=.*\d)[A-z0-9!@#$%]+$";
		public const string DatabaseName = @"^[A-z0-9\.]+$";
		public const string URL = @"/((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/";
		public const string HexColor = @"^[0-9a-fA-F]{1,6}$";
	}
}