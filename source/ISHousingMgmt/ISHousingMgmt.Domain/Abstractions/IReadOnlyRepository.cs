using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ISHousingMgmt.Domain.Abstractions {


	/// <summary>
	/// Opceniti repozitorij koji omogucuje samo operacije citanja.
	/// Za entitete sa identifikatorom tipa int
	/// </summary>
	/// <typeparam name="TEntity">Repozitorij samo za rad sa Entitetima/Agregatima</typeparam>
	public interface IReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity, int> 
		where TEntity : Entity { }

	/// <summary>
	/// Opceniti repozitorij koji omogucuje samo operacije citanja
	/// </summary>
	/// <typeparam name="TEntity">Repozitorij samo za rad sa Entitetima/Agregatima</typeparam>
	/// <typeparam name="TId">Tip identifikatora entiteta</typeparam>
	public interface IReadOnlyRepository<TEntity, TId> where TEntity : Entity<TId> {
		/// <summary>
		/// Dohvaca sve perzistirane entitete/agregate
		/// </summary>
		/// <returns>Svi perzistirani entiteti/agregati</returns>
		IEnumerable<TEntity> GetAll();

		/// <summary>
		/// Dohvaca entitet/agregat na temelju njegovog identifikatora
		/// </summary>
		/// <param name="id">identifikator entiteta/agregata</param>
		/// <returns>entitet/agregat</returns>
		TEntity GetById(TId id);
	}
}
