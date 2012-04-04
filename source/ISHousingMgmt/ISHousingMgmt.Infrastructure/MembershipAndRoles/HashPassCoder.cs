using System;
using System.Security.Cryptography;
using System.Web.Security;
using ISHousingMgmt.Domain.MembershipAndRoles;

namespace ISHousingMgmt.Infrastructure.MembershipAndRoles {
	/// <summary>
	/// Hash koder lozinke.
	/// </summary>
	/// <remarks>Razred ne podrzava(ne implementira) <see cref="HashPassCoder.Decode"/> metodu.</remarks>
	public class HashPassCoder :  IPasswordCoder {

		#region Members

		/// <summary>
		/// Hash algoritam
		/// </summary>
		private HashAlgorithm hashAlgorithm;

		/// <summary>
		/// Dohvaca oblik lozinke
		/// </summary>
		public MembershipPasswordFormat PasswordFormat { get { return MembershipPasswordFormat.Hashed; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="hashAlgorithm">hash algoritam</param>
		public HashPassCoder(HashAlgorithm hashAlgorithm) {
			this.hashAlgorithm = hashAlgorithm;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Kodira lozinku
		/// </summary>
		/// <param name="decodedPassword">plain-text lozinka</param>
		/// <returns>hash kodirana lozinka</returns>
		public byte[] Encode(byte[] decodedPassword) {
			return hashAlgorithm.ComputeHash(decodedPassword);
		}

		/// <summary>
		/// Nije podrzano za hash kodiranu lozinku
		/// </summary>
		/// <param name="encodedPassword">kodirana lozinka</param>
		/// <returns>baca iznimku NotSupportedException</returns>
		public byte[] Decode(byte[] encodedPassword) {
			throw new NotSupportedException();
		}

		#endregion
	}
}
