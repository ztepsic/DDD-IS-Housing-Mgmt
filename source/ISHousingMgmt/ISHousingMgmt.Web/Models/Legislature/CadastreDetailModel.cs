using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class CadastreDetailModel {

		/// <summary>
		/// Grad katastra
		/// </summary>
		public CityModel City { get; set; }

		/// <summary>
		/// Maticni broj katastarske opcine
		/// </summary>
		public string Mbr { get; set; }

		/// <summary>
		/// Katastarska opcina
		/// </summary>
		public string CadastralDistrict { get; set; }

	}
}