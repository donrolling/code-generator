using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace Business.Common {
	public class CryptConverter {
		protected byte[] Iv;
		protected byte[] Key;

		public CryptConverter(byte[] key, byte[] iv) {
			this.Key = key;
			this.Iv = iv;
		}

		public CryptConverter(string key, string iv) {
			this.Key = HexStringToByteArray(key);
			this.Iv = HexStringToByteArray(iv);
		}

		public static string ByteArrayToHexString(byte[] bytes) {
			var builder = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes) {
				builder.Append(b.ToString("X2"));
			}

			return builder.ToString();
		}

		public static void GetNewKey(out byte[] key, out byte[] iv) {
			var provider = new TripleDESCryptoServiceProvider();
			provider.GenerateKey();
			provider.GenerateIV();
			key = provider.Key;
			iv = provider.IV;
		}

		public static byte[] HexStringToByteArray(string hex) {
			if (hex.Length % 2 == 1) {
				hex = "0" + hex;
			}

			var bytes = new byte[hex.Length / 2];

			for (int i = 0; i < bytes.Length; ++i) {
				bytes[i] = byte.Parse(hex.Substring(2 * i, 2), NumberStyles.HexNumber);
			}

			return bytes;
		}

		public byte[] Decrypt(byte[] encryptedBytes) {
			var tdes = new TripleDESCryptoServiceProvider();
			return transform(tdes.CreateDecryptor(this.Key, this.Iv), encryptedBytes);
		}

		public T DeserializeAndDecrypt<T>(string encryptedHex) {

			byte[] encryptedObj = HexStringToByteArray(encryptedHex);

			byte[] plainObj = this.Decrypt(encryptedObj);

			var encoding = new ASCIIEncoding();
			string serializedObj = encoding.GetString(plainObj);

			var serializer = new JavaScriptSerializer();
			return serializer.Deserialize<T>(serializedObj);
		}

		public byte[] Encrypt(byte[] plainBytes) {
			var tdes = new TripleDESCryptoServiceProvider();
			return transform(tdes.CreateEncryptor(this.Key, this.Iv), plainBytes);
		}

		public string SerializeAndEncrypt<T>(T obj) {

			var serializer = new JavaScriptSerializer();
			string serializedObj = serializer.Serialize(obj);

			var encoding = new ASCIIEncoding();
			byte[] plainObj = encoding.GetBytes(serializedObj);

			byte[] encryptedObj = this.Encrypt(plainObj);

			return ByteArrayToHexString(encryptedObj);
		}

		private static byte[] transform(ICryptoTransform cryptoTransform, byte[] bytes) {
			var tdes = new TripleDESCryptoServiceProvider();

			var memStream = new MemoryStream();
			var cryptStream = new CryptoStream(memStream, cryptoTransform, CryptoStreamMode.Write);

			cryptStream.Write(bytes, 0, bytes.Length);
			cryptStream.FlushFinalBlock();

			memStream.Position = 0;
			byte[] result = memStream.ToArray();

			memStream.Close();
			cryptStream.Close();

			return result;
		}
	}
}