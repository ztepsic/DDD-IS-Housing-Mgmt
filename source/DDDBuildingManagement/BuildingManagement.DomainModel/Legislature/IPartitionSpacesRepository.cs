using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.Legislature {
	/// <summary>
	/// Repozitorij koji dohvaca etaze pod zajednickim vlasnistvom
	/// </summary>
	public interface IPartitionSpacesRepository : IRepository<PartitionSpace> {
		IPartitionSpace GetByNumberOfCadastralParticle(string numberOfCadastralParticle);
	}
}
