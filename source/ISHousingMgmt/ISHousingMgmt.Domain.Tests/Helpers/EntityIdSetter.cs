using System;
using System.Reflection;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Tests.Helpers {
	/// <summary>
	/// Razred omogucava rucno postavljanje identifikatora Entitetu pomocu reflectiona
	/// za svrhe testiranja.
	/// </summary>
	public static class EntityIdSetter {
		/// <summary>
		/// Postavlja identifikator Entitetu.
		/// </summary>
		/// <param name="entity">entitet kojemu se postavlja identifikator</param>
		/// <param name="id">identifikator</param>
		public static void SetIdOf<TId>(Entity<TId> entity, TId id) {
			PropertyInfo idProperty = entity.GetType().GetProperty("Id",
				BindingFlags.Public | BindingFlags.Instance);

			if(idProperty == null) {
				throw new MissingMemberException("No Id property");
			}

			idProperty.SetValue(entity, id, null);
		}

		/// <summary>
		/// Postavlja identifikator Entitetu.
		/// </summary>
		/// <param name="entity">entitet kojemu se postavlja identifikator</param>
		/// <param name="id">identifikator</param>
		public static Entity<TId> SetIdTo<TId>(this Entity<TId> entity, TId id) {
			SetIdOf(entity, id);
			return entity;
		}
	}
}
