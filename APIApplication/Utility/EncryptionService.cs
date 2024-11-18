using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DemoAPIApplication
{
  public class EncryptionService
  {
	private static readonly byte[] Key = Encoding.UTF8.GetBytes("okekdklo78595337"); // Replace with your 16-byte key
	private static readonly byte[] IV = Encoding.UTF8.GetBytes("9589idlo4647huhr");   // Replace with your 16-byte IV


	// Encrypt a string
	public static string Encrypt(string plainText)
	{
	  if (string.IsNullOrEmpty(plainText))
		throw new ArgumentNullException(nameof(plainText));

	  using (Aes aes = Aes.Create())
	  {
		aes.Key = Key;
		aes.IV = IV;

		using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
		using (var memoryStream = new MemoryStream())
		{
		  using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
		  using (var writer = new StreamWriter(cryptoStream))
		  {
			writer.Write(plainText);
		  }

		  return Convert.ToBase64String(memoryStream.ToArray());
		}
	  }
	}

	// Decrypt an encrypted string
	public static string Decrypt(string cipherText)
	{
	  if (string.IsNullOrEmpty(cipherText))
		throw new ArgumentNullException(nameof(cipherText));

	  using (Aes aes = Aes.Create())
	  {
		aes.Key = Key;
		aes.IV = IV;

		using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
		using (var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
		using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
		using (var reader = new StreamReader(cryptoStream))
		{
		  return reader.ReadToEnd();
		}
	  }
	}
  }
}
