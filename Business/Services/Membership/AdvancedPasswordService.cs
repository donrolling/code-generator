using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

//this was created to help fulfill a more secure password policy.
//it is unfinished and should not be used until finished.
namespace Business.Services.Membership {
	public class AdvancedPasswordService {
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
		public AdvancedPasswordService() {
			generatePassword();
			generateSalt();
			setEncytpedPassword();
		}
		/// <summary>
		/// Used to set the password. New salt will be generated.
		/// </summary>
		/// <param name="password">Unencrypted password.</param>
		public AdvancedPasswordService(string password) {
			Password = password;
			generateSalt();
			setEncytpedPassword();
		}
		/// <summary>
		/// Used to check a password.
		/// </summary>
		/// <param name="password">Unencrypted password.</param>
		/// <param name="salt">Salt must be passed in for valid password hashing.</param>
		public AdvancedPasswordService(string password, string salt) {
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
			byte[] numArray = new byte[size];
			RandomNumberGenerator.Create().GetBytes(numArray);
			return Convert.ToBase64String(numArray);
		}

		private static string createPasswordHash(string pwd, string salt) {
			var saltBytes = (new UTF8Encoding(false)).GetBytes(salt);
			var hashedPwd = passwordToHash(saltBytes, pwd);
			var result = (new UTF8Encoding(false)).GetString(hashedPwd);
			return result;
		}

		private static byte[] passwordToHash(byte[] salt, string password, int iterationCount = 5000) {
			if ((int)salt.Length < 8) {
				throw new Exception(string.Concat("The supplied salt is too short, it must be at least ", 8, " bytes long as defined by CMinSaltLength"));
			}
			var utf8Bytes = (new UTF8Encoding(false)).GetBytes(password);
			if ((int)utf8Bytes.Length > 1024) {
				throw new Exception(string.Concat("The supplied password is longer than allowed, it must be smaller than ", 1024, " bytes long as defined by CMaxPasswordLength"));
			}
			return (new Rfc2898(password, salt, iterationCount)).GetDerivedKeyBytes_PBKDF2_HMACSHA512(64);
		}
	}

	public class Rfc2898 {
		public const int CMinIterations = 1000;

		public const int CMinSaltLength = 8;

		private readonly HMACSHA512 _hmacsha512Obj;

		private readonly int hLen;

		private readonly byte[] P;

		private readonly byte[] S;

		private readonly int c;

		private int dkLen;

		public Rfc2898(byte[] password, byte[] salt, int iterations) {
			if (iterations < 1000) {
				throw new Exception("Iteration count is less than the 1000 recommended in Rfc2898");
			}
			if ((int)salt.Length < 8) {
				throw new Exception("Salt is less than the 8 byte size recommended in Rfc2898");
			}
			this._hmacsha512Obj = new HMACSHA512(password);
			this.hLen = this._hmacsha512Obj.HashSize / 8;
			this.P = password;
			this.S = salt;
			this.c = iterations;
		}

		public Rfc2898(string password, byte[] salt, int iterations)
			: this((new UTF8Encoding(false)).GetBytes(password), salt, iterations) {
		}

		public Rfc2898(string password, string salt, int iterations)
			: this((new UTF8Encoding(false)).GetBytes(password), (new UTF8Encoding(false)).GetBytes(salt), iterations) {
		}

		private byte[] F(byte[] P, byte[] S, int c, int i) {
			byte[] numArray = this.pMergeByteArrays(S, this.INT(i));
			byte[] numArray1 = this.PRF(P, numArray);
			byte[] numArray2 = numArray1;
			for (int num = 1; num < c; num++) {
				numArray1 = this.PRF(P, numArray1);
				for (int j = 0; j < (int)numArray1.Length; j++) {
					numArray2[j] = (byte)(numArray2[j] ^ numArray1[j]);
				}
			}
			return numArray2;
		}

		public byte[] GetDerivedKeyBytes_PBKDF2_HMACSHA512(int keyLength) {
			this.dkLen = keyLength;
			double num = Math.Ceiling((double)this.dkLen / (double)this.hLen);
			byte[] numArray = new byte[0];
			for (int i = 1; (double)i <= num; i++) {
				numArray = this.pMergeByteArrays(numArray, this.F(this.P, this.S, this.c, i));
			}
			return numArray.Take<byte>(this.dkLen).ToArray<byte>();
		}

		private byte[] INT(int i) {
			byte[] bytes = BitConverter.GetBytes(i);
			if (BitConverter.IsLittleEndian) {
				Array.Reverse(bytes);
			}
			return bytes;
		}

		public static byte[] PBKDF2(byte[] P, byte[] S, int c, int dkLen) {
			return (new Rfc2898(P, S, c)).GetDerivedKeyBytes_PBKDF2_HMACSHA512(dkLen);
		}

		private byte[] pMergeByteArrays(byte[] source1, byte[] source2) {
			byte[] numArray = new byte[(int)source1.Length + (int)source2.Length];
			Buffer.BlockCopy(source1, 0, numArray, 0, (int)source1.Length);
			Buffer.BlockCopy(source2, 0, numArray, (int)source1.Length, (int)source2.Length);
			return numArray;
		}

		private byte[] PRF(byte[] P, byte[] S) {
			return this._hmacsha512Obj.ComputeHash(this.pMergeByteArrays(P, S));
		}
	}
}
