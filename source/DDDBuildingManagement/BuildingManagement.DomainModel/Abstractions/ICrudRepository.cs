using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingManagement.DomainModel.Abstractions {
	/// <summary>
	/// Predstavlja opcenito sucelje za repozitorije koji omogucuju osnovne CRUD operacije
	/// </summary>
	public interface ICrudRepository<T> : IRepository<T> where T : EntityBase {
		void Save(T entity);
		void Delete(T entity);
	}
}
