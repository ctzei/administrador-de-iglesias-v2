$(document).ready(function () {
    hideAddressBar();

    if (!Modernizr.input.placeholder) {
        $('input[placeholder], textarea[placeholder]').placeholder();
    }
});

function hideAddressBar() {
    var win = window;
    var doc = win.document;

    // If there's a hash, or addEventListener is undefined, stop here
    if (!location.hash && win.addEventListener) {

        //scroll to 1
        window.scrollTo(0, 1);
        var scrollTop = 1,
			    getScrollTop = function () {
			        return win.pageYOffset || doc.compatMode === "CSS1Compat" && doc.documentElement.scrollTop || doc.body.scrollTop || 0;
			    },

        //reset to 0 on bodyready, if needed
			    bodycheck = setInterval(function () {
			        if (doc.body) {
			            clearInterval(bodycheck);
			            scrollTop = getScrollTop();
			            win.scrollTo(0, scrollTop === 1 ? 0 : 1);
			        }
			    }, 15);

        win.addEventListener("load", function () {
            setTimeout(function () {
                //at load, if user hasn't scrolled more than 20 or so...
                if (getScrollTop() < 20) {
                    //reset to hide addr bar at onload
                    win.scrollTo(0, scrollTop === 1 ? 0 : 1);
                }
            }, 0);
        });
    }
}

/*#### QUERYSTRING FUNCTIONS ####*/

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

/*#### DATE FUNCTIONS ####*/

Date.prototype.getMonthName = function (lang) {
    lang = lang && (lang in Date.locale) ? lang : 'en';
    return Date.locale[lang].month_names[this.getMonth()];
};

Date.prototype.getMonthNameShort = function (lang) {
    lang = lang && (lang in Date.locale) ? lang : 'en';
    return Date.locale[lang].month_names_short[this.getMonth()];
};

Date.prototype.getDayName = function (lang) {
    lang = lang && (lang in Date.locale) ? lang : 'en';
    return Date.locale[lang].day_names[this.getDay()];
};

Date.locale = {
    en: {
        month_names: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        month_names_short: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        day_names: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]
    },
    es: {
        month_names: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        month_names_short: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        day_names: ["Domingo", "Lunes", "Martes", "Mi\xe9rcoles", "Jueves", "Viernes", "S\xe1bado"]
    }
};

/*#### GLOBAL FUNCTIONS ####*/

function mask() {
    $('html').mask("Cargando...", 100);
}

function unmask() {
    $('html').unmask();
}

function processOnEnter(id, destinationId) {
    el = $(id);
    if (el) {
        el.destinationEl = $(destinationId);

        $(el).keydown(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                e.stopPropagation();
                e.stopEvent();

                if (this.destinationEl.type == 'button') {
                    this.destinationEl.focus();
                    this.destinationEl.fireEvent('click');
                }
                else {
                    this.destinationEl.clear();
                    this.destinationEl.focus();
                }
            }
        });
    }
}