
using System.Web.Security;

namespace ISHousingMgmt.Domain.MembershipAndRoles {

	/// <summary>
	/// Oblik lozinke
	/// </summary>
	public enum PasswordFormat {
		Clear, // plain-text
		Encrypted, // kriptirano
		Hashed // hashirano
	}

	/// <summary>
	/// Sucelje koje definira operacije kodiranja i dekodiranja lozinke
	/// </summary>
	public interface IPasswordCoder {

		/// <summary>
		/// Dohvaca oblik lozinke
		/// </summary>
		MembershipPasswordFormat PasswordFormat { get; }

		/// <summary>
		/// Kodira lozinku
		/// </summary>
		/// <param name="decodedPassword">plain-text lozinka</param>
		/// <returns>kodirana lozinka</returns>
		byte[] Encode(byte[] decodedPassword);

		/// <summary>
		/// Dekodira kodiranu lozinku
		/// </summary>
		/// <param name="encodedPassword">kodirana lozinka</param>
		/// <returns>dekodirana/plain text lozinka</returns>
		byte[] Decode(byte[] encodedPassword);
	}
}
