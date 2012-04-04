using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISHousingMgmt.Domain.BuildingMaintenance {
	/// <summary>
	/// Status popravka
	/// </summary>
	public enum StatusOfMaintenance {
		NotStarted,
		InProgress,
		InConfirmation,
		Completed
	}
}
