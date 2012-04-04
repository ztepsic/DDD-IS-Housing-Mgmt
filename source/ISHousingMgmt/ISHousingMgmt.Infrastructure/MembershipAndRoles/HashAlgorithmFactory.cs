using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ISHousingMgmt.Infrastructure.MembershipAndRoles {

	/// <summary>
	/// .NET implementacije hash algoritama
	/// </summary>
	public enum HashAlgorithmImplementation {
		SHA1,
		MD5,
		SHA256,
		SHA384,
		SHA512,
		HMACSHA1,
		HMACSHA256,
		HMACSHA384,
		HMACSHA512,
		HMACMD5
	}

	/// <summary>
	/// Factory koja stvara hash algoritme
	/// </summary>
	public static class HashAlgorithmFactory {

		#region Members

		/// <summary>
		/// Rijecnik hash algorithm implementacija
		/// </summary>
		private static readonly IDictionary<string, HashAlgorithmImplementation> hashAlgorithmImplDict = new Dictionary<string, HashAlgorithmImplementation>() {
			{ "SHA ", HashAlgorithmImplementation.SHA1 },
			{ "SHA1", HashAlgorithmImplementation.SHA1 },
			{ "System.Security.Cryptography.SHA1", HashAlgorithmImplementation.SHA1 },
			{ "System.Security.Cryptography.HashAlgorithm", HashAlgorithmImplementation.SHA1 },
			{ "MD5", HashAlgorithmImplementation.MD5 },
			{ "System.Security.Cryptography.MD5", HashAlgorithmImplementation.MD5 },
			{ "SHA256", HashAlgorithmImplementation.SHA256 },
			{ "SHA-256", HashAlgorithmImplementation.SHA256 },
			{ "System.Security.Cryptography.SHA256", HashAlgorithmImplementation.SHA256 },
			{ "SHA384", HashAlgorithmImplementation.SHA384 },
			{ "SHA-384", HashAlgorithmImplementation.SHA384 },
			{ "System.Security.Cryptography.SHA384", HashAlgorithmImplementation.SHA384 },
			{ "SHA512", HashAlgorithmImplementation.SHA512 },
			{ "SHA-512", HashAlgorithmImplementation.SHA512 },
			{ "System.Security.Cryptography.SHA512", HashAlgorithmImplementation.SHA512 },
			{ "HMACSHA", HashAlgorithmImplementation.HMACSHA1 },
			{ "HMACSHA1", HashAlgorithmImplementation.HMACSHA1 },
			{ "System.Security.Cryptography.HMACSHA1", HashAlgorithmImplementation.HMACSHA1 },
			{ "HMACSHA256", HashAlgorithmImplementation.HMACSHA256 },
			{ "HMACSHA-256", HashAlgorithmImplementation.HMACSHA256 },
			{ "System.Security.Cryptography.HMACSHA256", HashAlgorithmImplementation.HMACSHA256 },
			{ "HMACSHA384", HashAlgorithmImplementation.HMACSHA384 },
			{ "HMACSHA-384", HashAlgorithmImplementation.HMACSHA384 },
			{ "System.Security.Cryptography.HMACSHA384", HashAlgorithmImplementation.HMACSHA384 },
			{ "HMACSHA512", HashAlgorithmImplementation.HMACSHA512 },
			{ "HMACSHA-512", HashAlgorithmImplementation.HMACSHA512 },
			{ "System.Security.Cryptography.HMACSHA512", HashAlgorithmImplementation.HMACSHA512 },
			{ "HMACMD5", HashAlgorithmImplementation.HMACMD5 },
			{ "System.Security.Cryptography.HMACMD5", HashAlgorithmImplementation.HMACMD5 }
		};

		#endregion

		#region Constructors and Init
		#endregion

		#region Methods

		/// <summary>
		/// Stvara hash algoritam na temelju zadanog parametra
		/// </summary>
		/// <param name="hashAlgorithm">vrsta hash algortima</param>
		/// <returns>hash algoritam one vrste koja je zadana parametrom</returns>
		public static HashAlgorithm Create(string hashAlgorithm) {
			if(hashAlgorithmImplDict.ContainsKey(hashAlgorithm)) {
				return Create(hashAlgorithmImplDict[hashAlgorithm]);
			} else {
				throw new ArgumentException("Provided hashAlgorithm argument has invalid value.");
			}
		}

		/// <summary>
		/// Stvara SH1 hash algoritam
		/// </summary>
		/// <returns>SH1 has algoritam</returns>
		public static HashAlgorithm Create() {
			return Create(HashAlgorithmImplementation.SHA1);
		}

		/// <summary>
		/// Stvara hash algoritma na temelju zadanog parametra
		/// </summary>
		/// <param name="hashAlgorithm">vrsta hash algortima</param>
		/// <returns>hash algoritam one vrste koja je zadana parametrom</returns>
		public static HashAlgorithm Create(HashAlgorithmImplementation hashAlgorithm) {
			return HashAlgorithm.Create(hashAlgorithm.ToString());
		}

		#endregion

	}
}
