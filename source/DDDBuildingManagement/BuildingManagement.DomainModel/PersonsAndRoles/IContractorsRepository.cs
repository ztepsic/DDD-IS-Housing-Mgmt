using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Repozitorij izvodaca radova
	/// </summary>
	public interface IContractorsRepository : ICrudRepository<Contractor> {
	}
}
