using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.Abstractions {
	/// <summary>
	/// Predstavlja opcenito sucelje za repozitorije koji omogucuju dohvacanje podataka, odnosno citanje
	/// </summary>
	public interface IRepository<T> where T : EntityBase {
		T GetById(object id);
		IList<T> GetAll();
	}
}
