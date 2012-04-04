using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildingManagement.DomainModel.Abstractions;

namespace BuildingManagement.DomainModel.Finances {
	public interface IReservesRepository : ICrudRepository<Reserve> {
	}
}
