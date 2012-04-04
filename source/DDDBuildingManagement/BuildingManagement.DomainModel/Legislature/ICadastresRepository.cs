using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Repozitorij katastra
	/// </summary>
	public interface ICadastresRepository : ICrudRepository<Cadastre> {
		/// <summary>
		/// Dohvaca katastar preko maticnog broja
		/// </summary>
		/// <param name="mbr">maticni broj katastra</param>
		/// <returns></returns>
		Cadastre GetByMbr(string mbr);
	}
}
