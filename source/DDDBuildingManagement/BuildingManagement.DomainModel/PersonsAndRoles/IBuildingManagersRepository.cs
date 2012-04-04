using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.PersonsAndRoles {
	/// <summary>
	/// Repozitorij upravitelja zgrade
	/// </summary>
	public interface IBuildingManagersRepository : ICrudRepository<BuildingManager> {
	}
}
