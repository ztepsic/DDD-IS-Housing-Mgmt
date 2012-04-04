using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.BuildingMaintenance {
	public class MaintenanceRemark : ValueObject {

		#region Members

		/// <summary>
		/// Datum i vrijeme postavljanja napomene
		/// </summary>
		private readonly DateTime remarkDateTime;

		/// <summary>
		/// Dohvaca datum i vrijeme postavljanja napomene
		/// </summary>
		public virtual DateTime RemarkDateTime { get { return remarkDateTime; } }

		/// <summary>
		/// Napomena
		/// </summary>
		private readonly string remark;

		/// <summary>
		/// Dohvaca napomenu
		/// </summary>
		public virtual string Remark { get { return remark; } }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Konstruktor za lijenu evaluaciju NHibernate
		/// </summary>
		private MaintenanceRemark() : this(string.Empty) { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="remark">napomena održavanja/popravka</param>
		internal MaintenanceRemark(string remark) {
			this.remark = remark;
			remarkDateTime = DateTime.Now;
		}

		#endregion

		#region Methods

		#endregion

	}
}
