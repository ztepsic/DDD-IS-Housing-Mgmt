using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Repozitorij za zemljisne knige
	/// </summary>
	public interface ILandRegistriesRepository : ICrudRepository<LandRegistry> {
		/// <summary>
		/// Dohvaca zemljisnu knjigu na temelju broja katastarske cestice
		/// </summary>
		/// <param name="numberOfCadastralParticle">katastarska cestica</param>
		/// <returns>zemljisna knjiga</returns>
		LandRegistry GetByNumberOfCadastralParticle(string numberOfCadastralParticle);
	}
}
