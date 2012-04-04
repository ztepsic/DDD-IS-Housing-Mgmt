using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using ISHousingMgmt.Domain.BuildingManagement;
using ISHousingMgmt.Web.Models;

namespace ISHousingMgmt.Web.Helpers {
	public static class NavLinksGenerator {

		public static IList<NavLink> GetOwnerLinks(Building building) {
			return GetOwnerLinks(building, string.Empty);
		}

		public static IList<NavLink> GetOwnerLinks(Building building, string selectedText) {
			var links = new List<NavLink>();
			selectedText = selectedText ?? string.Empty;

			NavLink link = null;

			link = new NavLink() {
				Text = "Rad uprave",
				RouteValues = new RouteValueDictionary(new { controller = "buildingmanagement", action = "votings", id = building.Id }),
				IsSelected = selectedText == "Rad uprave"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Moji kvarovi",
				RouteValues = new RouteValueDictionary(new { controller = "maintenance", action = "my", id = building.Id }),
				IsSelected = selectedText == "Moji kvarovi"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Kvarovi",
				RouteValues = new RouteValueDictionary(new { controller = "maintenance", action = "index", id = building.Id }),
				IsSelected = selectedText == "Kvarovi"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Moji računi",
				RouteValues = new RouteValueDictionary(new { controller = "owner", action = "bills", id = building.Id }),
				IsSelected = selectedText == "Moji računi"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Pričuva",
				RouteValues = new RouteValueDictionary(new { controller = "finances", action = "reserve", id = building.Id }),
				IsSelected = selectedText == "Pričuva"
			};

			links.Add(link);

			return links;
		}

		public static IList<NavLink> GetManagerLinks(Building building) {
			return GetManagerLinks(building, string.Empty);
		}

		public static IList<NavLink> GetManagerLinks(Building building, string selectedText) {
			var links = new List<NavLink>();
			selectedText = selectedText ?? string.Empty;

			NavLink link = null;

			if(building.LandRegistry != null) {
				link = new NavLink() {
					Text = "Zemljišna knjiga",
					RouteValues = new RouteValueDictionary(new { controller = "legislature", action = "landregistry", id = building.Id }),
					IsSelected = selectedText == "Zemljišna knjiga"
				};

				links.Add(link);	
			}
			
			link = new NavLink() {
				Text = "Rad uprave",
				RouteValues = new RouteValueDictionary(new { controller = "buildingmanagement", action = "votings", id = building.Id }),
				IsSelected = selectedText == "Rad uprave"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Kvarovi",
				RouteValues = new RouteValueDictionary(new { controller = "maintenance", action = "index", id = building.Id }),
				IsSelected = selectedText == "Kvarovi"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Upraviteljevi računi",
				RouteValues = new RouteValueDictionary(new { controller = "buildingmanager", action = "bills", id = building.Id }),
				IsSelected = selectedText == "Upraviteljevi računi"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Pričuva",
				RouteValues = new RouteValueDictionary(new { controller = "finances", action = "reserve", id = building.Id }),
				IsSelected = selectedText == "Pričuva"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Izdani računi pričuve",
				RouteValues = new RouteValueDictionary(new { controller = "finances", action = "reserveperiods", id = building.Id }),
				IsSelected = selectedText == "Izdani računi pričuve"
			};

			links.Add(link);

			return links;
		}

		public static IList<NavLink> GetRepresentativeLinks(Building building) {
			return GetRepresentativeLinks(building, string.Empty);
		}

		public static IList<NavLink> GetRepresentativeLinks(Building building, string  selectedText) {
			var links = new List<NavLink>();
			selectedText = selectedText ?? string.Empty;

			NavLink link = null;

			link = new NavLink() {
				Text = "Zemljišna knjiga",
				RouteValues = new RouteValueDictionary(new { controller = "legislature", action = "landregistry", id = building.Id }),
				IsSelected = (selectedText == "Zemljišna knjiga")
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Rad uprave",
				RouteValues = new RouteValueDictionary(new { controller = "buildingmanagement", action = "votings", id = building.Id }),
				IsSelected = selectedText == "Rad uprave"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Kvarovi",
				RouteValues = new RouteValueDictionary(new { controller = "maintenance", action = "index", id = building.Id }),
				IsSelected = selectedText == "Kvarovi"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Pričuva",
				RouteValues = new RouteValueDictionary(new { controller = "finances", action = "reserve", id = building.Id }),
				IsSelected = selectedText == "Pričuva"
			};

			links.Add(link);

			link = new NavLink() {
				Text = "Izdani računi pričuve",
				RouteValues = new RouteValueDictionary(new { controller = "finances", action = "reserveperiods", id = building.Id }),
				IsSelected = selectedText == "Izdani računi pričuve"
			};

			links.Add(link);

			return links;
		}

	}
}