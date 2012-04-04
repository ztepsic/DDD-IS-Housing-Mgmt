using System;
using System.Web.Security;
using ISHousingMgmt.Domain.MembershipAndRoles;

namespace ISHousingMgmt.Infrastructure.MembershipAndRoles {
	/// <summary>
	/// Kriptirajuci koder lozinke.
	/// </summary>
	/// <remarks>Lozinka se kriptira pomocu simetricnog algoritma te je istu moguce dekodirati istim algoritmom i kljucem.</remarks>
	public class EncryptionPassCoder : IPasswordCoder {

		#region Members

		/// <summary>
		/// Metoda koja kriptira
		/// </summary>
		private Func<byte[], byte[]> encryptMethod;

		/// <summary>
		/// Metoda koja dekriptira
		/// </summary>
		private Func<byte[], byte[]> decryptMethod;

		/// <summary>
		/// Dohvaca oblik lozinke
		/// </summary>
		public MembershipPasswordFormat PasswordFormat { get { return MembershipPasswordFormat.Encrypted; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="encryptMethod">kriptirajuca metoda</param>
		/// <param name="decryptMethod">dekriptirajuca metoda</param>
		public EncryptionPassCoder(Func<byte[], byte[]> encryptMethod, Func<byte[], byte[]> decryptMethod) {
			this.encryptMethod = encryptMethod;
			this.decryptMethod = decryptMethod;
		}

		#endregion

		#region Methods

		#region Implementation of IPasswordCoder

		/// <summary>
		/// Kodira lozinku
		/// </summary>
		/// <param name="decodedPassword">plain-text lozinka</param>
		/// <returns>kodirana lozinka</returns>
		public byte[] Encode(byte[] decodedPassword) {
			return encryptMethod.Invoke(decodedPassword);
		}

		/// <summary>
		/// Dekodira kodiranu lozinku
		/// </summary>
		/// <param name="encodedPassword">kodirana lozinka</param>
		/// <returns>dekodirana/plain text lozinka</returns>
		public byte[] Decode(byte[] encodedPassword) {
			return decryptMethod.Invoke(encodedPassword);
		}

		#endregion

		#endregion
	}
}
