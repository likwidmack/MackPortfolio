/// <reference path="http://code.jquery.com/jquery-2.1.1.js" />
/// <reference path="http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.1/modernizr.js" />
/// <reference path="http://code.jquery.com/ui/1.10.4/jquery-ui.js" />
/// <reference path="http://code.jquery.com/color/jquery.color.plus-names-2.1.2.js" />
/// <reference path="http://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.js" />
/// <reference path="http://cdnjs.cloudflare.com/ajax/libs/knockout/3.1.0/knockout-debug.js" />
/// <reference path="http://cdnjs.cloudflare.com/ajax/libs/knockout-validation/1.0.2/knockout.validation.js" />
/// <reference path="http://cdnjs.cloudflare.com/ajax/libs/knockout.mapping/2.3.5/knockout.mapping.js" />
/// <reference path="_references.js" />

function reduceRatio(numerator, denominator) {
    var gcd, temp, divisor;

    gcd = function (a, b) {
        if (b === 0) return a;
        return gcd(b, a % b);
    }

    // take care of some simple cases
    if (!isInteger(numerator) || !isInteger(denominator)) return '? : ?';
    if (numerator === denominator) return '1 : 1';

    // make sure numerator is always the larger number
    if (+numerator < +denominator) {
        temp = numerator;
        numerator = denominator;
        denominator = temp;
    }

    divisor = gcd(+numerator, +denominator);

    var num = (numerator / divisor) + ' : ' + (denominator / divisor);
    var den = (denominator / divisor) + ' : ' + (numerator / divisor);

    return 'undefined' === typeof temp ? num : den;
};

/**** Remove Special Characters and Spaces from Text *****/
function removeSSC(txt) {
    if (typeof (txt) !== 'string') return txt;
    txt = txt.replace(/['`~!@#$%^&*()|+=?;:'",.<>\{\}\[\]\\\/]/gi, '');
    return txt.replace(/(\s|\r\n|\n|\r)/gm, '_');
}
function replaceUnderscore(txt) {
    if (txt == null || typeof (txt) !== 'string') return txt;
    return txt.replace(/(_|\r\n|\n|\r)/gm, ' ');
}

function isInteger(value) {
    return /^[0-9]+$/.test(value);
}

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}

/**** Clone of an Object *****/
function cloneObject(obj) {
    if (obj == null || typeof (obj) != 'object') return obj;
    var temp = obj.constructor();
    for (var key in obj) {
        if (typeof (obj[key]) == 'object' && obj[key] != null) {
            temp[key] = cloneObject(obj[key]);
        } else {
            temp[key] = obj[key];
        }
    }
    return temp;
}

/**** Structured Clone of an Object *****/
function structuredClone(obj) {
    if (typeof history === 'undefined' || $('html').hasClass('ie')) {
        return cloneObject(obj);
    }
    var oldState = history.state;
    history.replaceState(obj);
    var clonedObj = history.state;
    history.replaceState(oldState);
    return clonedObj;
}

//#region Main Properties : common functions and callbacks
if ('undefined' === typeof main) {
    var main = document.getElementById("mainapp");
}

main = {
    isIE: $('html').hasClass('ie'),
    isLegacyIE: $('html').hasClass('lt-ie9'),
    forEachKey: function (jsObject, func) {
        var keys = Object.keys(jsObject);
        for (var i = 0, max = keys.length; i < max; i++) {
            if (func != undefined) func(keys[i]);
        }
    },
    contains: function (a, obj) {
        if (a !== undefined) {
            var i = a.length;
            while (i--) {
                if (a[i] === obj) {
                    return true;
                }
            }
        }
        return false;
    },
    logTime: function (obj) {
        var startDt = new Date();
        console.log('start time: ' + startDt.getSeconds() + '.' + startDt.getMilliseconds());
        console.log(obj);
        var endDt = new Date();
        console.log('end time: ' + endDt.getSeconds() + '.' + endDt.getMilliseconds());
    },
    count: {
        throttleTimeout: {},
        createUniqueID: function () {
            return 't' + Math.floor((Math.random() * Math.random()) * (Math.exp(10) * 100000));
        },
        createTimeID: function () {
            var dt = new Date();
            return Date.UTC(dt.getFullYear(), dt.getMonth() + 1, dt.getDate(), dt.getHours(), dt.getMinutes(), dt.getSeconds(), dt.getMilliseconds());
        }
    }
};

main.throttle = function (unqId, time, func) {
    if (!isNaN(parseInt(main.count.throttleTimeout[unqId]))) {
        clearTimeout(main.count.throttleTimeout[unqId]);
    }
    main.count.throttleTimeout[unqId] = setTimeout(function () {
        console.log('throttleTime for ' + unqId + ':  ' + main.count.throttleTimeout[unqId]);
        if (func != undefined) func();
        delete main.count.throttleTimeout[unqId];
    }, time);
};

if ('undefined' !== typeof google
    && 'undefined' !== typeof google['maps']) {

    main.googleGeocoder = new google.maps.Geocoder();
    main.geoLocationCode = {
        init: function (func) {
            var _this = this;
            _this.lat = 0;
            _this.lng = 0;
            _this.results = '';
            console.log(navigator.geolocation);
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    _this.lat = position.coords.latitude;
                    _this.lng = position.coords.longitude;
                    _this.results = position.coords.latitude.toFixed(3) + ', ' + position.coords.longitude.toFixed(3);
                    if (func != undefined) func(_this);
                }, function (error) {
                    var msg;
                    switch (error.code) {
                        case error.TIMEOUT:
                            msg = 'Timeout';
                            break;
                        case error.POSITION_UNAVAILABLE:
                            msg = 'Position unavailable';
                            break;
                        case error.PERMISSION_DENIED:
                            msg = 'Permission denied';
                            break;
                        case error.UNKNOWN_ERROR:
                            msg = 'Unknown error';
                            break;
                    }
                    _this.results = 'Could not Find Location: ' + msg;
                    if (func != undefined) func(_this);
                }, this.positionOptions);
            } else {
                _this.results = "I'm sorry, but geolocation services are not supported by your browser.";
                if (func != undefined) func(_this);
            }
        },
        positionOptions: {
            timeout: 10000,
            enableHighAccuracy: true,
            maximumAge: Infinity
        },
        fromAddress: function (address, func) {
            var text, _this = this;
            address = address.replace(/([\r\n]+|\n|\r)/gm, ' ');
            address = address.replace(/(\s+)/g, ' ');

            main.googleGeocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        var position = results[0].geometry.location;
                        _this.lat = position.lat();
                        _this.lng = position.lng();
                        _this.results = results[0].formatted_address;
                        console.log(ko.toJSON(ko.toJS(results)));

                    } else {
                        text = "According to Google Maps, this location does not exist. \nPlease enter a valid location.";
                        console.log(text);
                        _this.results = text;
                    }
                } else {
                    text = 'Geocode was not successful for the following reason: ' + status;
                    console.log(text);
                    _this.results = text;
                }
                if (func != undefined) func(_this);
            });
        }
    }
}

main.BrowserDetect = {
    init: function () {
        this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
        this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
        this.OS = this.searchString(this.dataOS) || "an unknown OS";
    },
    searchString: function (data) {
        for (var i = 0; i < data.length; i++) {
            var dataString = data[i].string;
            var dataProp = data[i].prop;
            this.versionSearchString = data[i].versionSearch || data[i].identity;
            if (dataString) {
                if (dataString.indexOf(data[i].subString) != -1)
                    return data[i].identity;
            }
            else if (dataProp)
                return data[i].identity;
        }
    },
    searchVersion: function (dataString) {
        var index = dataString.indexOf(this.versionSearchString);
        if (index == -1) return;
        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    },
    dataBrowser: [
		{
		    string: navigator.userAgent,
		    subString: "Chrome",
		    identity: "Chrome"
		},
		{
		    string: navigator.userAgent,
		    subString: "OmniWeb",
		    versionSearch: "OmniWeb/",
		    identity: "OmniWeb"
		},
		{
		    string: navigator.vendor,
		    subString: "Apple",
		    identity: "Safari",
		    versionSearch: "Version"
		},
		{
		    prop: window.opera,
		    identity: "Opera",
		    versionSearch: "Version"
		},
		{
		    string: navigator.vendor,
		    subString: "iCab",
		    identity: "iCab"
		},
		{
		    string: navigator.vendor,
		    subString: "KDE",
		    identity: "Konqueror"
		},
		{
		    string: navigator.userAgent,
		    subString: "Firefox",
		    identity: "Firefox"
		},
		{
		    string: navigator.vendor,
		    subString: "Camino",
		    identity: "Camino"
		},
		{		// for newer Netscapes (6+)
		    string: navigator.userAgent,
		    subString: "Netscape",
		    identity: "Netscape"
		},
		{
		    string: navigator.userAgent,
		    subString: "MSIE",
		    identity: "Explorer",
		    versionSearch: "MSIE"
		},
		{
		    string: navigator.userAgent,
		    subString: "Gecko",
		    identity: "Mozilla",
		    versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
		    string: navigator.userAgent,
		    subString: "Mozilla",
		    identity: "Netscape",
		    versionSearch: "Mozilla"
		}
    ],
    dataOS: [
		{
		    string: navigator.platform,
		    subString: "Win",
		    identity: "Windows"
		},
		{
		    string: navigator.platform,
		    subString: "Mac",
		    identity: "Mac"
		},
		{
		    string: navigator.userAgent,
		    subString: "iPhone",
		    identity: "iPhone/iPod"
		},
		{
		    string: navigator.platform,
		    subString: "Linux",
		    identity: "Linux"
		}
    ]
};

//#endregion

//#region javascript compatibility safeguards

//Add Object.keys Compatibility to IE Browsers
if (!Object.keys) {
    Object.keys = function (o) {
        if (o !== Object(o))
            throw new TypeError('Object.keys called on a non-object');
        var k = [], p;
        for (p in o) if (Object.prototype.hasOwnProperty.call(o, p)) k.push(p);
        return k;
    }
}
if (!Array.prototype.filter) {
    Array.prototype.filter = function (fun /*, thisp*/) {
        "use strict";

        if (this == null)
            throw new TypeError();

        var t = Object(this);
        var len = t.length >>> 0;
        if (typeof fun != "function")
            throw new TypeError();

        var res = [];
        var thisp = arguments[1];
        for (var i = 0; i < len; i++) {
            if (i in t) {
                var val = t[i]; // in case fun mutates this
                if (fun.call(thisp, val, i, t))
                    res.push(val);
            }
        }

        return res;
    };
}
if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    };
}
if (!('contains' in String.prototype)) {
    String.prototype.contains = function (txt, startIndex) {
        return -1 !== String.prototype.indexOf.call(this.toLowerCase(), txt.toLowerCase(), startIndex);
    }
}

//#region knockoutjs extensions
//Generic Knockout Prototypes
if ('undefined' !== typeof ko) {

    //an observable that retrieves its value when first bound
    ko.onComputed = function (callback, target) {
        var _value = ko.observable();  //private observable

        var result = ko.computed({
            read: function () {
                //if it has not been loaded, execute the supplied function
                if (!result.loaded()) {
                    callback.call(target);
                }
                //always return the current value
                return _value();
            },
            write: function (newValue) {
                //indicate that the value is now loaded and set it
                result.loaded(true);
                _value(newValue);
            },
            deferEvaluation: true  //do not evaluate immediately when created
        });

        //expose the current state, which can be bound against
        result.loaded = ko.observable();
        //load it again
        result.refresh = function () {
            result.loaded(false);
        };

        return result;
    };

    ko.onComputedArray = function (callback, target) {
        var _value = ko.observableArray();  //private observable

        var result = ko.computed({
            read: function () {
                //if it has not been loaded, execute the supplied function
                if (!result.loaded()) {
                    callback.call(target);
                }
                //always return the current value
                return _value();
            },
            write: function (newValue) {
                //indicate that the value is now loaded and set it
                result.loaded(true);
                _value(newValue);
            },
            deferEvaluation: true  //do not evaluate immediately when created
        });

        //expose the current state, which can be bound against
        result.loaded = ko.observable();
        //load it again
        result.refresh = function () {
            result.loaded(false);
        };

        return result;
    };

    if ('undefined' !== typeof ko.bindingHandlers) {

        ko.bindingHandlers.onHover = {
            init:function (element, valueAccessor) {
                var expand = valueAccessor();

                $(element).hover(function () {
                    expand(true);
                }, function () {
                    expand(false);
                });
            }
        };

        ko.bindingHandlers.animateCSS = {
            update: function (element, valueAccessor, allBindings, viewModel) {
                var options = ko.unwrap(valueAccessor()),
                    bindings = allBindings(),
                    timer = bindings.timer || 400;
                $(element).stop(true, true)
                    .delay(200)
                    .animate(options, timer);
            }
        };

        ko.bindingHandlers.jqButton = {
            init: function (element, valueAccessor) {
                var options = valueAccessor() || {};
                $(element).button(options);
                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $(element).button("destroy");
                });
            }
        };

        ko.bindingHandlers.jqProgressBar = {
            init: function (element, valueAccessor) {
                var options = ko.unwrap(valueAccessor()) || {};

                $(element).progressbar({ value: options.value });
                $(element).find('.ui-progressbar-value').addClass(options.color);
            }
        };

        //custom AutoComplete box
        ko.bindingHandlers.jqAutoComplete = {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var unwrap = ko.utils.unwrapObservable;
                var ko_data = valueAccessor();
                $(element).autocomplete({
                    delay: 500,
                    minLength: 3,
                    autoFocus: true,
                    position: {
                        my: 'left bottom',
                        at: 'left top',
                        collision: 'flip fit',
                        of: '#' + ko_data.key
                    },
                    source: function (request, response) {
                        $.get('', { text: request.term }, function (data) {
                            response(data);
                        });
                    },
                    focus: function (event, ui) {
                    },
                    select: function (event, ui) {
                        if (ui.item) {
                        }
                        return false;
                    }
                });
                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $(element).autocomplete("destroy");
                });
            }
        };

        //custom buttom for Expand/Collapse
        ko.bindingHandlers.jqToggleButton = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                var value = ko.unwrap(valueAccessor()),
                    name = allBindingsAccessor().attr.id;
                $(element).button({
                    text: false,
                    icons: {
                        primary: (value) ? "ui-icon-minusthick" : "ui-icon-plusthick"
                    }
                });
            },
            update: function (element, valueAccessor, allBindingsAccessor) {
                var value = ko.unwrap(valueAccessor()),
                    name = allBindingsAccessor().attr.id;
                $(element).button('option', 'icons', { primary: (value) ? "ui-icon-minusthick" : "ui-icon-plusthick" });
            }
        };

        //custom binding handler a dialog
        ko.bindingHandlers.jqDialog = {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var showWin = valueAccessor(),
                    bindings = allBindingsAccessor(),
                    options = ko.unwrap(bindings.winData) || {};

                options = $.extend(options, {
                    close: function (e, ui) { showWin(false); }
                });
                $(element).dialog(options);

                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $(element).dialog('destroy');
                });
            },
            update: function (element, valueAccessor) {
                var showWin = ko.utils.unwrapObservable(valueAccessor());
                if (showWin) {
                    $(element).dialog("open");
                } else {
                    $(element).dialog("close");
                }
            }
        };

        ko.bindingHandlers.fadeVisible = {
            init: function (element, valueAccessor) {
                // Initially set the element to be instantly visible/hidden depending on the value
                var value = valueAccessor();
                // Use "unwrapObservable" so we can handle values that may or may not be observable
                $(element).toggle(ko.utils.unwrapObservable(value));
            },
            update: function (element, valueAccessor) {
                // Whenever the value subsequently changes, slowly fade the element in or out
                var value = valueAccessor();
                ko.utils.unwrapObservable(value) ? $(element).show() : $(element).fadeOut('slow');
            }
        };

        ko.bindingHandlers.slideVisible = {
            init: function (element, valueAccessor) {
                var value = ko.utils.unwrapObservable(valueAccessor()); // Get the current value of the current property we're bound to
                $(element).toggle(value); // jQuery will hide/show the element depending on whether "value" or true or false
            },
            update: function (element, valueAccessor, allBindingsAccessor) {
                // First get the latest data that we're bound to
                var value = valueAccessor(), allBindings = allBindingsAccessor();

                // Next, whether or not the supplied model property is observable, get its current value
                var valueUnwrapped = ko.utils.unwrapObservable(value);

                // Grab some more data from another binding property
                var duration = allBindings.slideDuration || 400; // 400ms is default duration unless otherwise specified

                // Now manipulate the DOM element
                if (valueUnwrapped == true)
                    $(element).slideDown(duration); // Make the element visible
                else
                    $(element).slideUp(duration);   // Make the element invisible
            }
        };
    }

    if ('undefined' !== typeof ko.observableArray) {

        ko.observableArray.fn.sortByProperty = function (prop) {
            this.sort(function (obj1, obj2) {
                if (obj1[prop] == obj2[prop])
                    return 0;
                else if (obj1[prop] < obj2[prop])
                    return -1;
                else
                    return 1;
            });
        };
    }
}
//#endregion

//#endregion 
