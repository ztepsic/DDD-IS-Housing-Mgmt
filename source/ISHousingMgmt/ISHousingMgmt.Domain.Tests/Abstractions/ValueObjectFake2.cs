using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISHousingMgmt.Domain.Tests.Abstractions {
	/// <summary>
	/// Razred koji predstavlja vrijednosni objekt na temelju <see cref="ValueObjectFake"/>
	/// </summary>
	class ValueObjectFake2 : ValueObjectFake {

		public string Country { get; private set; }

		public ValueObjectFake2(string address, string city, string country) : base(address, city) {
			Country = country;
		}
	}
}
