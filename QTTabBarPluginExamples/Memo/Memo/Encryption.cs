using System;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace QuizoPlugin
{
	static class EncryptionUtil
	{
		// http://dobon.net/vb/dotnet/string/encryptstring.html を参考に改変
		// Dobon often forgets to dispose objects.

		/// <summary>
		/// Encrypt string into byte aray
		/// </summary>
		/// <param name="sourceString"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static byte[] Encrypt( string sourceString, string password )
		{
			using( var rijndael = new RijndaelManaged() )
			{
				rijndael.GenerateKeyFromPassword( password );

				using( var encryptor = rijndael.CreateEncryptor() )
				{
					byte[] buffer = Encoding.UTF8.GetBytes( sourceString );
					return encryptor.TransformFinalBlock( buffer, 0, buffer.Length );
				}
			}
		}

		/// <summary>
		/// Tries to decrypt byte array into string
		/// </summary>
		/// <param name="encryptedData"></param>
		/// <param name="password"></param>
		/// <param name="decrypted"></param>
		/// <returns></returns>
		public static bool TryDecrypt( byte[] encryptedData, string password, out string decrypted )
		{
			try
			{
				using( var rijndael = new RijndaelManaged() )
				{
					rijndael.GenerateKeyFromPassword( password );

					using( var decryptor = rijndael.CreateDecryptor() )
					{
						byte[] buffer = decryptor.TransformFinalBlock( encryptedData, 0, encryptedData.Length );
						decrypted = Encoding.UTF8.GetString( buffer );
						return true;
					}
				}
			}
			catch
			{
			}
			decrypted = null;
			return false;
		}

		private static void GenerateKeyFromPassword( this RijndaelManaged rijndael, string password )
		{
			// Create secret key and initialization vector from password

			byte[] salt = Encoding.UTF8.GetBytes( "The salt must be longer than 8 bytes 本日は晴天なり" );

			using( var deriveBytes = new Rfc2898DeriveBytes( password, salt ) )
			{
				deriveBytes.IterationCount = 1000;

				rijndael.Key =  deriveBytes.GetBytes( rijndael.KeySize / 8 );
				rijndael.IV = deriveBytes.GetBytes( rijndael.BlockSize / 8 );
			}
		}

		public static string PathToHash( string path )
		{
			using( var md5 = new MD5CryptoServiceProvider() )
			{
				StringBuilder sb = new StringBuilder();
				foreach( var b in md5.ComputeHash( Encoding.Unicode.GetBytes( path.ToUpper( CultureInfo.InvariantCulture ) ) ) )
				{
					sb.Append( b.ToString( "X2" ) );
				}
				return sb.ToString();
			}
		}	
	}

}