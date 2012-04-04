using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Finances {
	public interface IReservesRepository : IReadOnlyRepository<Reserve> {
	}
}
