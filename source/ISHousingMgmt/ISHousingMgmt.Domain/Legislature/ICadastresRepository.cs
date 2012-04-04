using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Legislature {
	/// <summary>
	/// Repozitorij katastra
	/// </summary>
	public interface ICadastresRepository : IReadOnlyRepository<Cadastre> {
		/// <summary>
		/// Dohvaca katastar preko maticnog broja
		/// </summary>
		/// <param name="mbr">maticni broj katastra</param>
		/// <returns></returns>
		Cadastre GetByMbr(string mbr);

		/// <summary>
		/// Dohvaca listu katastara za odredeni grad
		/// </summary>
		/// <param name="cityId">identifikator grada</param>
		/// <returns>lista katastara za odredeni grad</returns>
		IList<Cadastre> GetByCity(int cityId);
	}
}
