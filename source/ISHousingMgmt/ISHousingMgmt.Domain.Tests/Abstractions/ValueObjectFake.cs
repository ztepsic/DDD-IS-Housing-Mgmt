using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISHousingMgmt.Domain.Abstractions;

namespace ISHousingMgmt.Domain.Tests.Abstractions {
	/// <summary>
	/// Razred koji predstavlja lazni vrijednosni objekt
	/// </summary>
	class ValueObjectFake : ValueObject {

		public string Address { get; private set; }
		public string City { get; private set; }

		public ValueObjectFake(string address, string city) {
			Address = address;
			City = city;
		}


	}
}
