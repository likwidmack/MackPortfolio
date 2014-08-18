/// <reference path="_references.js" />
/// <reference path="global.js" />

window.requestAnimFrame = (function (callback) {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (callback) {
            window.setTimeout(callback, 1000 / 60);
        };
})();

function MainModel() {
    var self = this;
    self.canvas = new canvasModel();
    self.sliders = new slidersModel();
    self.activeCanvas = ko.observable(true);
    self.disableCanvas = $('html').hasClass('no-canvas');

    self.activeCanvas.subscribe(function (_bool) {
        if (!self.disableCanvas) {
            if (_bool) {
                self.canvas.createObjects();
            } else {
                self.canvas.clearCanvas();
            }
        }
    });

    self.init = function () {
        if (self.disableCanvas) {
            $('.objectCounter').text('Internet Explorer 8 or less version does not support HTML5 canvas.');
            self.activeCanvas(false);
            return false;
        }
        if (self.activeCanvas()) {
            self.canvas.createCanvas();
        }
    };
}

function canvasModel() {
    var self = this;
    self.name = ko.observable('canvas');
    self.context = ko.observable(null);
    self.canvasWidth = ko.observable(0);
    self.canvasHeight = ko.observable(0);
    self.maxNum = ko.observable(150);
    self.objects = ko.observableArray([]);
    self.dimensions = ko.observableArray([]);
    self.variance = ko.observable(5);
    self.isRandom = ko.observable(false);
    self.context.subscribe(function (ctx) {
        //console.log(ko.toJS(self));
    });
    self.canvasHeight.subscribe(function (h) {
        document.getElementById(self.name()).height = h;
    });
    self.canvasWidth.subscribe(function (w) {
        document.getElementById(self.name()).width = w;
    });

    self.clearCanvas = function () {
        var canvas = document.getElementById(self.name()),
            width = self.canvasWidth(),
            height = self.canvasHeight(),
            ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, width, height);
    };
    self.createCanvas = function () {
        var canvas = document.getElementById(self.name()),
            ctx = canvas.getContext('2d');
        main.mainapp.prepend($('canvas'));
        self.canvasWidth(main.mainapp.width());
        self.canvasHeight(main.mainapp.height());
        self.context(ctx);
        self.mapCanvas();
    };
    self.mapCanvas = function () {
        var rdm = main.colors.getRandom,
            canvas = document.getElementById(self.name()),
            objArray = [],
            dimArray = [],
            csArray = [],
            ctx = self.context(),
            width = self.canvasWidth(),
            height = self.canvasHeight(),
            count = self.isRandom() ? rdm(self.maxNum()) : Math.abs(self.maxNum() - 1);
        for (var i = 0; i < count + 1; i++) {
            var dim = {};
            dim.x = rdm(width);
            dim.y = rdm(height);
            dim.w = rdm(width - dim.x);
            dim.h = rdm(height - dim.y);
            dim.w2 = Math.ceil(Math.sqrt(rdm(dim.h + dim.w)*rdm(10)));

            dimArray.push(dim);
            csArray.push({
                fill: new colorSetItem(),
                stroke: new colorSetItem()
            });
        }
        self.objects(csArray);
        self.dimensions(dimArray);
        self.createObjects();
    };
    self.refreshCanvas = function () {
        self.canvasWidth(main.mainapp.width());
        self.canvasHeight(main.mainapp.height());
        main.throttle('canvasResizeToggle', 200, function () {
            var rdm = main.colors.getRandom,
                width = self.canvasWidth(),
                height = self.canvasHeight();
            for (var i = 0; i < self.objects().length; i++) {
                var dim = {};
                dim.x = rdm(width);
                dim.y = rdm(height);
                dim.w = rdm(width - dim.x);
                dim.h = rdm(height - dim.y);
                dim.w2 = Math.ceil(Math.sqrt(dim.w * 2));

                self.dimensions()[i] = dim;
            }
            self.createObjects();
        });
    };
    self.shiftObjects = function () {
        var item = self.objects.shift();
        self.objects.push(item);
        self.createObjects();
    };
    self.createObjects = function () {
        if (!main.isLegacyIE && main.model.activeCanvas()) {
            var canvas = document.getElementById(self.name()),
                width = self.canvasWidth(),
                height = self.canvasHeight(),
                ctx = canvas.getContext('2d');
            ctx.clearRect(0, 0, width, height);
            //console.log(self.objects());
            console.log(self.dimensions());
            for (var i = 0; i < self.objects().length; i++) {
                self.drawObject(ctx, i);
            }
            self.context(ctx);
        }
    };

    self.drawObject = function (ctx, i) {
        var vr = self.variance(),
            gLin = main.colors.getLinear,
            gRad = main.colors.getRadial,
            dim = self.dimensions()[i],
            x = dim.x,
            y = dim.y,
            w = dim.w,
            h = dim.h,
            w2 = dim.w2,
            item = self.objects()[i];

        ctx.beginPath();
        if (i % vr) {
            //d.x1, d.y1, d.r1, d.x2, d.y2, d.r2
            ctx.fillStyle = gRad(ctx, {
                x1: x,
                y1: y,
                r1: w2,
                x2: x,
                y2: y,
                r2: Math.ceil(w2 / 6)
            }, 'fill', item);
            ctx.strokeStyle = gLin(ctx, {
                x1: x - w2,
                y1: y - w2,
                x2: x + w2 * 2,
                y2: y + w2 * 2
            }, 'stroke', item);
            ctx.arc(x, y, w2, 0, Math.PI * 2, true);
        } else {
            var obj = {
                x1: x,
                y1: y,
                x2: w + x,
                y2: h + y
            };
            ctx.fillStyle = gLin(ctx, obj, 'fill', item);
            ctx.strokeStyle = gLin(ctx, obj, 'stroke', item);
            ctx.rect(x, y, w, h);
        }
        ctx.stroke();
        ctx.fill();
    };
}

function colorSetItem() {
    var mc = main.colors;
    this.acsOpac = mc.getOpacity();
    this.acs1 = mc.getRandomRGBA();
    this.acs2 = mc.getRandomRGBA();
    this.acs3 = mc.getRandomRGBA();
}

function slidersModel() {
    var self = this, _clr = 'indigo';
    self.input = ko.observable(_clr)
    self.color = ko.observable($.Color(_clr));

    //RGB Computed KO
    self.red = ko.computed({
        read: function () {
            return self.color().red();
        },
        write: function (value) {
            var _clr = self.color();
            self.color(_clr.red(value));
        },
        owner: self
    });
    self.green = ko.computed({
        read: function () {
            return self.color().green();
        },
        write: function (value) {
            var _clr = self.color();
            self.color(_clr.green(value));
        },
        owner: self
    });
    self.blue = ko.computed({
        read: function () {
            return self.color().blue();
        },
        write: function (value) {
            var _clr = self.color();
            self.color(_clr.blue(value));
        },
        owner: self
    });

    //HSL Computed KO
    self.hue = ko.computed({
        read: function () {
            return self.color().hue();
        },
        write: function (value) {
            var _clr = self.color();
            self.color(_clr.hue(value));
        },
        owner: self
    });
    self.saturation = ko.computed({
        read: function () {
            return self.color().saturation();
        },
        write: function (value) {
            var _clr = self.color();
            self.color(_clr.saturation(value));
        },
        owner: self
    });
    self.lightness = ko.computed({
        read: function () {
            return self.color().lightness();
        },
        write: function (value) {
            var _clr = self.color();
            self.color(_clr.lightness(value));
        },
        owner: self
    });

    //Object to String convert Computed KO
    self.rgb = ko.computed(function () {
        var _clr = self.color();
        return _clr.toRgbaString();
    }).extend({ rateLimit: 200 });
    self.hsl = ko.computed(function () {
        var _clr = self.color();
        return _clr.toHslaString();
    }).extend({ rateLimit: 200 });
    self.hex = ko.computed(function () {
        var _clr = self.color();
        return _clr.toHexString();
    }).extend({ rateLimit: 200 });

    self.input.subscribe(function (_colr) {
        self.getInput();
    });
    self.color.subscribe(function (_colr) {
        //console.log(ko.toJS(self));
    });
    self.rgb.subscribe(function (rgb) {
        $('.side-canvas-panel').animate({
            'background-color': (self.color().alpha(0.5)).toRgbaString()
        }, 400)
    });

    self.complementary = new complementColors(self);

    self.invertColor = function () {
        var ic = main.colors.invertColor(self.hex());
        self.color($.Color(ic));
        self.updateInput();
    };
    self.getInput = function () {
        self.color($.Color(self.input()));
    };
    self.updateInput = function () {
        //console.log('test stop color');
        self.input(self.color().toHexString());
    };

    //jQuery Slider Options
    self.rgbParam = {
        orientation: "horizontal",
        animate: true,
        range: "min",
        max: 255,
        stop: self.updateInput
    };
    self.hslParam = {
        orientation: "horizontal",
        animate: true,
        range: "min",
        step: 0.01,
        min: 0.00,
        max: 1.00,
        stop: self.updateInput
    };
    self.hslHueParam = {
        orientation: "horizontal",
        animate: true,
        range: "min",
        step: 1,
        min: 0,
        max: 359,
        stop: self.updateInput
    };
}

function complementColors($this) {
    var self = this;
    self.swap1 = ko.computed(function () {
        var color = $this.color();
        return $.Color(color.green(), color.blue(), color.red());
    }).extend({ rateLimit: 200 });
    self.swap1hex = ko.computed(function () {
        return (self.swap1()).toHexString();
    });
    self.swap1rgba = ko.computed(function () {
        return (self.swap1().alpha(0.75)).toRgbaString();
    });
    self.swap1hsl = ko.computed(function () {
        return (self.swap1()).toHslaString();
    });
    self.swap1text = ko.computed(function () {
        return main.colors.invertColor(self.swap1hex());
    });

    self.swap2 = ko.computed(function () {
        var color = $this.color();
        return $.Color(color.blue(), color.red(), color.green());
    }).extend({ rateLimit: 200 });
    self.swap2hex = ko.computed(function () {
        return (self.swap2()).toHexString();
    });
    self.swap2rgba = ko.computed(function () {
        return (self.swap2().alpha(0.75)).toRgbaString();
    });
    self.swap2hsl = ko.computed(function () {
        return (self.swap2()).toHslaString();
    });
    self.swap2text = ko.computed(function () {
        return main.colors.invertColor(self.swap2hex());
    });

    self.invert = ko.computed(function () {
        var color = $this.hex();
        return $.Color(main.colors.invertColor(color));
    });
    self.inverthex = ko.computed(function () {
        return (self.invert()).toHexString();
    });
    self.invertrgba = ko.computed(function () {
        return (self.invert().alpha(0.75)).toRgbaString();
    });
    self.inverthsl = ko.computed(function () {
        return (self.invert()).toHslaString();
    });
    self.inverttext = ko.computed(function () {
        return main.colors.invertColor(self.inverthex());
    });
}

main.colors = {
    toHex: function (r, g, b) {
        var red = parseInt(r),
            green = parseInt(g),
            blue = parseInt(b),
            rgb = blue | (green << 8) | (red << 16);
        return '#' + rgb.toString(16);
    },
    toRgb: function (hex) {
        var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex),
            rgb = '';

        if (result) {
            var r = parseInt(result[1], 16),
                g = parseInt(result[2], 16),
                b = parseInt(result[3], 16);

            rgb = 'rgb(' + r + ',' + g + ',' + b + ')';
        }
        return result ? {
            rgb: rgb,
            r: r,
            g: g,
            b: b
        } : null;
    },
    contrastColor:function(_this){
        var r = _this._rgba[0], g = _this._rgba[1], b = _this._rgba[2];
        return (((r * 299) + (g * 587) + (b * 144)) / 1000) >= 131.5 ? (_this.transition('#ffffff', 0.85)).toHexString() : (_this.transition('#000000', 0.75)).toHexString();
    },
    invertColor:function(color){
        color = color.substring(1);
        color = parseInt(color, 16);
        color = 0xFFFFFF ^ color;
        color = color.toString(16);
        color = ("000000" + color).slice(-6);
        color = "#" + color;
        return color;
    },
    getNameArray: function () {
        var arr = [];
        for (var prop in $.Color.names) {
            arr.push(prop);
        }
        return arr;
    },
    getRandom: function (num) {
        var rnum = Math.floor(Math.random() * num);
        return Math.abs(rnum);
    },
    getRandom256: function () {
        var rnum = Math.floor(Math.random() * 256);
        return rnum;
    },
    getOpacity: function () {
        var ropac = Math.round(Math.random(), 2);
        return ropac;
    },
    getRandomRGBA: function () {
        var r = this.getRandom256(),
            g = this.getRandom256(),
            b = this.getRandom256(),
            a = this.getOpacity();
        return 'rgba(' + r + ',' + g + ',' + b + ',' + a + ')';
    },
    getRandomRGB: function () {
        var r = this.getRandom256(),
            g = this.getRandom256(),
            b = this.getRandom256();
        return 'rgb(' + r + ',' + g + ',' + b + ')';
    },
    getLinear: function (ctx, d, acs, item) {
        var ling = ctx.createLinearGradient(d.x1, d.y1, d.x2, d.y2);
        if (typeof acs == 'string') {
            ling.addColorStop(0, item[acs].acs1);
            ling.addColorStop(item[acs].acsOpac, item[acs].acs2);
            ling.addColorStop(1, item[acs].acs3);
        } else {
            ling.addColorStop(0, acs.acs1);
            ling.addColorStop(acs.acsOpac, acs.acs2);
            ling.addColorStop(1, acs.acs3);
        }
        return ling;
    },
    getRadial: function (ctx, d, acs, item) {
        var radg = ctx.createRadialGradient(d.x1, d.y1, d.r1, d.x2, d.y2, d.r2);
        if (typeof acs == 'string') {
            radg.addColorStop(0, item[acs].acs1);
            radg.addColorStop(item[acs].acsOpac, item[acs].acs2);
            radg.addColorStop(1, item[acs].acs3);
        } else {
            radg.addColorStop(0, acs.acs1);
            radg.addColorStop(acs.acsOpac, acs.acs2);
            radg.addColorStop(1, acs.acs3);
        }
        return radg;
    }
};

main.colors.round = Math.round;
Math.round = function (number, decimals /* optional, default 0 */) {
    if (arguments.length == 1)
        return main.colors.round(number);

    multiplier = Math.pow(10, decimals);
    return main.colors.round(number * multiplier) / multiplier;
};

ko.bindingHandlers.colorSlider = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var value = valueAccessor(),
            bindings = allBindings(),
            options = bindings.Opt;

        $.extend(options, {
            value: value(),
            slide: function (event, ui) {
                value(ui.value);
            }
        });

        $(element).slider(options);
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).slide("destroy");
        });
    },
    update: function (element, valueAccessor, allBindings, viewModel) {
        var value = valueAccessor();
        $(element).slider('value', value());
    }
};

ko.bindingHandlers.autocompleteColor = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var value = valueAccessor(),
            bindings = allBindings() || {};

        $(element).autocomplete({
            source: main.colors.getNameArray(),
            select: function (e, ui) {
                value(ui.item.value);
            }
        });

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).autocomplete("destroy");
        });
    }
}

ko.bindingHandlers.windowResize = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var onResized = valueAccessor();
        //$(element).resizable(options);
        $(window).resize(onResized);
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(window).resize({});
        });
    }
};