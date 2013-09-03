/**
* Copyright (c) 2012 David Campos
*/

(function ($) {
    if (typeof (Modernizr) == "undefined" || !Modernizr) {
        throw "This plugin requires Modernizr to be previously loaded.";
    } else {
        $.fn.extend({
            tap: function (fn) {

                if (typeof (fn) != "function" || !fn) {
                    throw "There must be a function object passed as a parameter into the tap function."; 
                }

                return this.each(function () {
                    var obj = $(this);

                    if (Modernizr.touch) {
                        obj.bind("touchstart", function () {
                            $(this).unbind("touchend");
                            $(this).unbind("touchmove");

                            $(this).bind("touchmove", function () {
                                $(this).unbind("touchend");
                            });

                            $(this).bind("touchend", function (event) {
                                fn.call(this, event);
                            });
                        });
                    }
                    else {
                        obj.bind("click", function (event) {
                            fn.call(this, event);
                        });
                    }

                });
            }
        });
    }
})(jQuery);