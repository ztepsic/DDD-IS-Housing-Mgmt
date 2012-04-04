using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISHousingMgmt.Domain.Abstractions {

	/// <summary>
	/// Opceniti repozitorij koji omogucuje osnovne CRUD operacije.
	/// Za entitete sa identifikatorom tipa int
	/// </summary>
	/// <typeparam name="TEntity">Repozitorij samo za rad sa Entitetima/Agregatima</typeparam>
	public interface ICrudRepository<TEntity> : ICrudRepository<TEntity, int> 
		where TEntity : Entity { }

	/// <summary>
	/// Opceniti repozitorij koji omogucuje osnovne CRUD operacije
	/// </summary>
	/// <typeparam name="TEntity">Repozitorij samo za rad sa Entitetima/Agregatima</typeparam>
	/// <typeparam name="TId">Tip identifikatora entiteta</typeparam>
	public interface ICrudRepository<TEntity, TId> : IReadOnlyRepository<TEntity, TId>
	where TEntity : Entity<TId> {
		/// <summary>
		/// Sprema novi ili azurira postojeci entitet/agregat iz repozitorija
		/// </summary>
		/// <param name="entity">entitet/agregat koji se sprema ili azurira</param>
		void SaveOrUpdate(TEntity entity);

		/// <summary>
		/// Brise entitet/agregat iz repozitorija
		/// </summary>
		/// <param name="entity">enitet/agregat koji se brise</param>
		void Delete(TEntity entity);
	}
}
