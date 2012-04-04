using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Legislature;
using ISHousingMgmt.Domain.PersonsAndRoles;
using ISHousingMgmt.Infrastructure.DataAccess.NHibernate;
using NHibernate;

namespace ISHousingMgmt.Infrastructure.DataAccess.Repositories.Legislature {
	public class PartitionSpacesNHRepository : NHibernateReadOnlyRepository<PartitionSpace>, IPartitionSpacesRepository {

		#region Members
		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor
		/// </summary>
		public PartitionSpacesNHRepository() { }


		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="sessionFactory">NHibernate tvornica sjednica</param>
		public PartitionSpacesNHRepository(ISessionFactory sessionFactory) : base(sessionFactory) { }

		#endregion

		#region Methods

		/// <summary>
		/// Dohvaca sve etaze na temelju katastarske cestice
		/// </summary>
		/// <param name="numberOfCadastralParticle">broj katastarske cestice</param>
		/// <returns>etaze za zadanu katastarsku cesticu</returns>
		public IList<IPartitionSpace> GetPartitionSpaces(string numberOfCadastralParticle) {
			return Session.QueryOver<PartitionSpace>()
				.Where(ps => ps.CadastralParticle.NumberOfCadastralParticle == numberOfCadastralParticle)
				.List() as List<IPartitionSpace>;
		}

		/// <summary>
		/// Dohvaca etazu na temelju broja katastarske cestice i rednog broja etaze
		/// </summary>
		/// <param name="numberOfCadastralParticle">broj katastarske cestice</param>
		/// <param name="ordinalNumberOfPartitionSpace">redni broj etaze</param>
		/// <returns>etaza za zadanu katastarksu cesticu i redni broj etaze</returns>
		public IPartitionSpace GetPartitionSpace(string numberOfCadastralParticle, int ordinalNumberOfPartitionSpace) {
			return Session.QueryOver<PartitionSpace>()
				.Where(ps => ps.CadastralParticle.NumberOfCadastralParticle == numberOfCadastralParticle)
				.And(ps => ps.OrdinalNumber == ordinalNumberOfPartitionSpace)
				.SingleOrDefault();
		}

		/// <summary>
		/// Dohvaca etazu na temelju broja uloška
		/// </summary>
		/// <param name="registryNumber">broj uloška</param>
		/// <returns>etaza za zadani broj uloška</returns>
		public IPartitionSpace GetPartitionSpace(string registryNumber) {
			return Session.QueryOver<PartitionSpace>()
				.Where(ps => ps.RegistryNumber == registryNumber)
				.SingleOrDefault();
		}

		/// <summary>
		/// Dohvaca etazu na temelju vlasnistva i zemljisne knjige
		/// </summary>
		/// <param name="owner">vlasnik</param>
		/// <param name="landRegistry">zemljisna knjiga</param>
		/// <returns>etaza</returns>
		public IPartitionSpace GetPartitionSpace(Person owner, LandRegistry landRegistry) {
			return Session.QueryOver<PartitionSpace>()
				.Where(ps => ps.Owner == owner)
				.And(ps => ps.LandRegistry == landRegistry)
				.SingleOrDefault();
		}

		/// <summary>
		/// Dohvaca etaze na temelju vlasnistva
		/// </summary>
		/// <param name="owner">vlasnik</param>
		/// <returns>etaze</returns>
		public IList<IPartitionSpace> GetPartitionSpaces(Person owner) {
			return Session.QueryOver<IPartitionSpace>()
				.Where(ps => ps.Owner == owner)
				.List();
		}

		/// <summary>
		/// Dohvaca etaze na temelju vlasnistva
		/// </summary>
		/// <param name="owner">vlasnik</param>
		/// <param name="limit">broj etaza koji se dohvaca</param>
		/// <returns>etaze</returns>
		public IList<IPartitionSpace> GetPartitionSpaces(Person owner, int limit) {
			return Session.QueryOver<IPartitionSpace>()
				.Where(ps => ps.Owner == owner)
				.Take(limit)
				.List();
		}

		#endregion

	}
}
