
using System.Web.Security;
using ISHousingMgmt.Domain.MembershipAndRoles;

namespace ISHousingMgmt.Infrastructure.MembershipAndRoles {
	/// <summary>
	/// Plain-text koder lozinke. Lozinka se zapravo ne kodira,
	/// odnosno ono sto ude kao parametar u istom obliku i izade
	/// </summary>
	public class ClearPassCoder : IPasswordCoder {

		#region Members

		/// <summary>
		/// Dohvaca oblik lozinke
		/// </summary>
		public MembershipPasswordFormat PasswordFormat { get { return MembershipPasswordFormat.Clear; } }

		#endregion

		#region Constructors and Init
		#endregion

		#region Methods

		#region IPasswordCoder Members

		/// <summary>
		/// Kodira lozinku u plain-text
		/// </summary>
		/// <param name="decodedPassword">plain-text lozinka</param>
		/// <returns>plain-text kodirana lozinka</returns>
		public byte[] Encode(byte[] decodedPassword) {
			return decodedPassword;
		}

		/// <summary>
		/// Dekodira plain-text kodiranu lozinku
		/// </summary>
		/// <param name="encodedPassword">plain-text kodirana lozinka</param>
		/// <returns>dekodirana/plain text lozinka</returns>
		public byte[] Decode(byte[] encodedPassword) {
			return encodedPassword;
		}

		#endregion

		#endregion
	}
}
