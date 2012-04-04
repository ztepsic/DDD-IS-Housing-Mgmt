using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Tests.Abstractions {
	/// <summary>
	/// Razred bez business key
	/// </summary>
	class EntityFake : Entity {
		public string Username { get; private set; }

		public string Email { get; private set; }

		public short Age { get; set; }

		public EntityFake(string username, string email) {
			Username = username;
			Email = email;
		}
	}
}
