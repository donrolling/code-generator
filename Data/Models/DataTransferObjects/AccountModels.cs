using System.ComponentModel.DataAnnotations;

namespace Data.Models.DataTransferObjects {
	public class ChangePasswordModel {
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string CurrentPassword { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 8, ErrorMessage = "The {0} must be at least {2} characters long.")]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		//[System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmNewPassword { get; set; }
	}

	public class ForgotPasswordModel {
		[Required]
		[Display(Name = "Email Address")]
		[DataType(DataType.EmailAddress)]
		public string EmailAddress { get; set; }
	}

	public class LoginModel {
		[Required]
		[Display(Name = "User name")]
		[StringLength(50, MinimumLength = 1)]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		[StringLength(50, MinimumLength = 1)]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}

	public class RegisterModel {
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email address")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		//[System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class MyProfileModel {
		public long UserId { get; set; }
		public string Email { get; set; }
	}
}
