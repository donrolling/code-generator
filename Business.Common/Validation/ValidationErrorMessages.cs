namespace Business.Common.Validation {
	public class ValidationErrorMessages {
		public const string PhoneNumber = "Phone number must be a valid phone number.";
		public const string Fax = "Fax number must be a valid fax number.";
		public const string Zip = "Please provide a valid zip code.";
		public const string Email = "Please provide a valid email address.";
		public const string EmailList = "All email addresses in this list must be valid. This list should be seperated by commas and should not end in a comma or a space.";
		public const string Date = "Please provide a valid date.";
		public const string Password = "Password must be between 6 and 12 characters, and contain one digit. Special characters are optional.";
		public const string DatabaseName = "A database name must not contain any special characters or spaces.";
		public const string Length = "This number of characters in the field {0} exceeds the maximum length of {1}.";
		public const string HexColor = "Please specify colors as 6 hexadecimal digits for red, green, and blue components (rrggbb).";
	}
}