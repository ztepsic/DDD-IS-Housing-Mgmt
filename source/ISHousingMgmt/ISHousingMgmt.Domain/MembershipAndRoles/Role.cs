using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.MembershipAndRoles {
	public class Role : NHibernateEntity {

		#region Members

		/// <summary>
		/// Naziv uloge
		/// </summary>
		[BusinessKeyOfEntity]
		public virtual string Name { get; set; }

		#endregion

		#region Constructors and Init

		/// <summary>
		/// Defaultni kontruktor za podrsku NHibernateovom lazy loadingu
		/// </summary>
		protected Role() { }

		public Role(string name) {
			Name = name;
		}

		#endregion

		#region Methods
		#endregion

	}
}
