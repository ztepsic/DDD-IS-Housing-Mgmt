using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.BuildingMaintenance {
	/// <summary>
	/// Repozitorij usluga popravka
	/// </summary>
	public interface IRepairServicesRepository : IReadOnlyRepository<RepairService> {
	}
}
