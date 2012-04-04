using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;
using ISHousingMgmt.Domain.PersonsAndRoles;

namespace ISHousingMgmt.Domain.Legislature {
	/// <summary>
	/// Repozitorij koji dohvaca etaze pod zajednickim vlasnistvom
	/// </summary>
	public interface IPartitionSpacesRepository : IReadOnlyRepository<PartitionSpace> {

		/// <summary>
		/// Dohvaca sve etaze na temelju katastarske cestice
		/// </summary>
		/// <param name="numberOfCadastralParticle">broj katastarske cestice</param>
		/// <returns>etaze za zadanu katastarsku cesticu</returns>
		IList<IPartitionSpace> GetPartitionSpaces(string numberOfCadastralParticle);

		/// <summary>
		/// Dohvaca etazu na temelju broja katastarske cestice i rednog broja etaze
		/// </summary>
		/// <param name="numberOfCadastralParticle">broj katastarske cestice</param>
		/// <param name="ordinalNumberOfPartitionSpace">redni broj etaze</param>
		/// <returns>etaza za zadanu katastarksu cesticu i redni broj etaze</returns>
		IPartitionSpace GetPartitionSpace(string numberOfCadastralParticle, int ordinalNumberOfPartitionSpace);

		/// <summary>
		/// Dohvaca etazu na temelju broja uloška
		/// </summary>
		/// <param name="registryNumber">broj uloška</param>
		/// <returns>etaza za zadani broj uloška</returns>
		IPartitionSpace GetPartitionSpace(string registryNumber);

		/// <summary>
		/// Dohvaca etazu na temelju vlasnistva i zemljisne knjige
		/// </summary>
		/// <param name="owner">vlasnik</param>
		/// <param name="landRegistry">zemljisna knjiga</param>
		/// <returns>etaza</returns>
		IPartitionSpace GetPartitionSpace(Person owner, LandRegistry landRegistry);

		/// <summary>
		/// Dohvaca etaze na temelju vlasnistva
		/// </summary>
		/// <param name="owner">vlasnik</param>
		/// <returns>etaze</returns>
		IList<IPartitionSpace> GetPartitionSpaces(Person owner);

		/// <summary>
		/// Dohvaca etaze na temelju vlasnistva
		/// </summary>
		/// <param name="owner">vlasnik</param>
		/// <param name="limit">broj etaza koji se dohvaca</param>
		/// <returns>etaze</returns>
		IList<IPartitionSpace> GetPartitionSpaces(Person owner, int limit);
	}
}
