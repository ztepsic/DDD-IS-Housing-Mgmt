using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Tests.Abstractions {
	/// <summary>
	/// Razred sa business key
	/// </summary>
	class EntityFakeWithBusinessKey : Entity {

		[BusinessKeyOfEntity]
		public string Username { get; private set; }

		[BusinessKeyOfEntity]
		public string Email { get; private set; }

		public short Age { get; set; }

		public EntityFakeWithBusinessKey(string username, string email) {
			Username = username;
			Email = email;
		}

	}
}
