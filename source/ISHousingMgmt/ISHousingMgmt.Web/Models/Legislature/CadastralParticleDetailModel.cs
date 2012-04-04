using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISHousingMgmt.Web.Models.Legislature {
	public class CadastralParticleDetailModel {

		/// <summary>
		/// Dohvaca katastar
		/// </summary>
		public CadastreDetailModel Cadastre { get; set; }

		/// <summary>
		/// Dohvaca katastarske cestice
		/// </summary>
		[Display(Name = "Broj katastarske čestice")]
		public string NumberOfCadastralParticle { get; set; }


		/// <summary>
		/// Dohvaca povrsinu katastarske cestice
		/// </summary>
		[Display(Name = "Površina")]
		public decimal SurfaceArea { get; set; }

		/// <summary>
		/// Dohvaca opis katastaske cestice
		/// </summary>
		[Display(Name = "Opis")]
		public string Description { get; set; }

	}
}