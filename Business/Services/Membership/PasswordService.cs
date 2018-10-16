using System;
using System.Security.Cryptography;
using System.Web.Security;

namespace Business.Services.Membership {
	public class PasswordService {
		public const int PasswordMinLength = 8;
		public const int PasswordMaxLength = 16;
		public const string InvalidPassword = "Sorry, the password entered does not match the username provided. Please try again.";
		public const string NoPassword = "Please enter a password.";
		public const string PasswordsDoNotMatch = "Sorry, the passwords that you entered do not match. Please try again.";
		public const string PasswordNotSecure = "Password is not secure.";
		public static string PasswordWrongLength = "Password must be between " + PasswordMinLength + " and " + PasswordMaxLength + " characters.";
		public const string CurrentPasswordIncorrect = "Your current password is incorrect.";

		public string Password { get; set; }
		public string Salt { get; set; }
		public string EncryptedPassword { get; set; }

		/// <summary>
		/// Used to generate a new random password.
		/// </summary>
		public PasswordService() {
			generatePassword();
			generateSalt();
			setEncytpedPassword();
		}
		/// <summary>
		/// Used to set the password. New salt will be generated.
		/// </summary>
		/// <param name="password">Unencrypted password.</param>
		public PasswordService(string password) {
			Password = password;
			generateSalt();
			setEncytpedPassword();
		}
		/// <summary>
		/// Used to check a password.
		/// </summary>
		/// <param name="password">Unencrypted password.</param>
		/// <param name="salt">Salt must be passed in for valid password hashing.</param>
		public PasswordService(string password, string salt) {
			Password = password;
			Salt = salt;
			setEncytpedPassword();
		}

		private void generatePassword() {
			Password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
		}
		private void generateSalt() {
			Salt = createSalt(Password.Length);
		}
		private void setEncytpedPassword() {
			EncryptedPassword = createPasswordHash(Password, Salt);
		}

		private static string createSalt(int size) {
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] buff = new byte[size];
			rng.GetBytes(buff);

			return Convert.ToBase64String(buff);
		}
#pragma warning disable
		private static string createPasswordHash(string pwd, string salt) {
			string saltAndPwd = String.Concat(pwd, salt);
			string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
			return hashedPwd;
		}
#pragma warning restore
	}
}
