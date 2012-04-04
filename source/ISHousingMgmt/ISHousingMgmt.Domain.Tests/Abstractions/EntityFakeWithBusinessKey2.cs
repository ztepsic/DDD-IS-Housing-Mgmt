using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Tests.Abstractions {
	/// <summary>
	/// Razred sa dodatnim business key-om za razliku od <see cref="EntityFakeWithBusinessKey"/>
	/// </summary>
	class EntityFakeWithBusinessKey2 : EntityFakeWithBusinessKey {

		[BusinessKeyOfEntity]
		public string Address { get; set; }

		public EntityFakeWithBusinessKey2(string username, string email) : base(username, email) {}
	}
}
