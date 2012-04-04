(function () {

	$.validator.methods.number = function (value, element) {
		// string that separates number groups, as in 1,000,000 => ',': ",",
		var numberGroups = $.global.culture.numberFormat[","] == "." ? "\\." : ",";

		// string that separates a number from the fractional portion as in 1.99 => '.': ".",
		var fractionalPortion = $.global.culture.numberFormat["."] == "." ? "\\." : ",";

		var patt = new RegExp("^-?(?:\\d+|\\d{1,3}(?:" + numberGroups + "\\d{3})+)(?:" + fractionalPortion + "\\d+)?$");
		//var patt = new RegExp(/^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/);

		// return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:[\.,]\d+)?$/.test(value); 
		return this.optional(element) || patt.test(value);
	}

	$.validator.methods.range = function (value, element, param) {
		if ($.global.culture.numberFormat[","] == ".") {
			value = value.replace(",", ".");
		}

		return this.optional(element) || (value >= param[0] && value <= param[1]);
	}

})();