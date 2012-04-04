using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISHousingMgmt.Domain.Abstractions {

	/// <summary>
	/// Razred predstavlja bazni razred za NHibernate enitete koji ce se perzistirati u bazi podataka.
	/// Temelji se na <see cref="Entity"/> razredu te uvodi svojstvo za verzioniranje koje omogucuje
	/// NHibernateu optimistic lock.
	/// Za koristenje nekog drugog tipa za identitet koristiti razred <see cref="NHibernateEntity{TId}"/>
	/// </summary>
	public abstract class NHibernateEntity : Entity {
		#region Members

		/// <summary>
		/// Sluzi za verzioniranje stanja entiteta (omogucuje NHibernate optimistic concurrency)
		/// </summary>
		protected virtual int Version { get; set; }

		#endregion
	}

	/// <summary>
	/// Razred predstavlja bazni razred za NHibernate enitete koji ce se perzistirati u bazi podataka.
	/// Temelji se na <see cref="Entity<TId>"/> razredu te uvodi svojstvo za verzioniranje koje omogucuje
	/// NHibernateu optimistic lock.
	/// </summary>
	public abstract class NHibernateEntity<TId> : Entity<TId> {

		#region Members

		/// <summary>
		/// Sluzi za verzioniranje stanja entiteta (omogucuje NHibernate optimistic concurrency)
		/// </summary>
		protected virtual int Version { get; set; }

		#endregion

	}
}
