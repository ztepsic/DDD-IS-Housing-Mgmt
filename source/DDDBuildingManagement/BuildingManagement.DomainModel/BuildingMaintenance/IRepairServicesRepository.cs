﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.BuildingMaintenance {
	/// <summary>
	/// Repozitorij usluga popravka
	/// </summary>
	public interface IRepairServicesRepository : ICrudRepository<RepairService> {
	}
}
