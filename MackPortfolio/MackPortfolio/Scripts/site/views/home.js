/// <reference path="global.js" />
/// <reference path="_references.js" />

function MainModel() {
    var self = this;
    self.location = new GoogleMapModel();
    self.connection = new ConnectionTestModel();
    self.monitor = new MonitorSpecsModel();
    self.system = new UserSystemModel();
    self.browser = new BrowserSpecsModel();

    self.init = function () {
        self.connection.refresh();
        self.location.getCurrentLocation();
    };
}

function GoogleMapModel() {
    var self = this, geo = main.geoLocationCode;
    self.lat = ko.observable(geo.lat);
    self.lng = ko.observable(geo.lng);
    self.msg = ko.observable(geo.results);
    self.address = ko.observable('');
    self.width = ko.observable(screen.width);
    self.height = ko.observable(screen.height);
    self.calculatedRatio = ko.computed(function () {
        return self.width() / self.height();
    });
    self.boxWidth = ko.observable($('#map').parent().width());
    self.boxHeight = ko.computed(function () {
        return (self.boxWidth() / self.calculatedRatio());
    });
    self.map = function () {
        showMap(self.lat(), self.lng(), self.msg());
    }
    self.getAddressLocation = function () {
        if (self.address().length) {
            main.geoLocationCode.fromAddress(self.address(), function (data) {
                self.msg(data.results);
                self.lat(data.lat); self.lng(data.lng);
                self.map();
                console.log(ko.toJS(data));
            });
        }
    };
    self.getCurrentLocation = function () {
        self.address('');
        main.geoLocationCode.init(function (data) {
            self.msg(data.results);
            self.lat(data.lat); self.lng(data.lng);
            self.map();
            console.log(ko.toJS(data));
        });
    };
}

function ConnectionTestModel() {
    var self = this;
    self.objData = ko.observable({});
    self.starttime = ko.observable(new Date());
    self.endtime = ko.observable(new Date());
    self.duration = ko.observable(0);
    self.bitsloaded = ko.observable(0);
    self.speedBps = ko.observable(0.00);
    self.speedKbps = ko.computed(function () {
        return (self.speedBps() / 1024).toFixed(2);
    });
    self.speedMbps = ko.computed(function () {
        return (self.speedKbps() / 1024).toFixed(2);
    });
    self.message = ko.computed(function () {
        var msg;
        if (self.speedMbps() > 8) {
            msg = 'Internet Speed is Excellent.';
        } else if (self.speedKbps() > 600) {
            msg = 'Internet Speed is Fair.';
        } else {
            msg = 'Internet Speed is Poor.';
        }
        return msg + '<br/><small><em>This will not read properly in localhost.</em></small>';
    });
    self.endtime.subscribe(function (_date) {
        self.duration((_date.getTime() - self.starttime().getTime()) / 1000);
    });
    self.objData.subscribe(function (data) {
        self.bitsloaded(('undefined' === typeof data.size) ? 0 : (data.size * 8));
    });
    self.bitsloaded.subscribe(function (_size) {
        self.speedBps((_size / self.duration()).toFixed(2));
    });
    self.refresh = function () {
        self.starttime(new Date());
        $.get(main.inetUrl, { n: self.starttime().getTime() }, function (data) {
            console.log(data);
            self.endtime(new Date());
            self.objData(data);
        });
    };
}

function MonitorSpecsModel() {
    var self = this;
    self.width = ko.observable(screen.width);
    self.height = ko.observable(screen.height);
    self.availWidth = ko.observable(screen.availWidth);
    self.availHeight = ko.observable(screen.availHeight);
    self.colorDepth = ko.observable(screen.colorDepth);
    self.pixelDepth = ko.observable(screen.pixelDepth);
    self.calculatedRatio = ko.computed(function () {
        return (self.availWidth() / self.availHeight()).toFixed(1);
    });
    self.reducedRatio = ko.computed(function () {
        return reduceRatio(self.availWidth(), self.availHeight());
    });
    self.width.subscribe(function (_width) {
        console.log(_width);
    });
}

function UserSystemModel() {
    var self = this, dt = new Date();
    self.date = ko.observable(dt);
    self.timezoneOffset = ko.observable(dt.getTimezoneOffset());
    self.gmt = ko.observable(dt.toGMTString());
    self.timezoneOffsetHours = ko.computed(function () {
        return self.timezoneOffset() / 60;
    });
    self.modifiedDate = ko.computed(function () {
        var dto = self.timezoneOffset(),
            utc = self.date().getTime() + (dto * 60000);
        return new Date(utc + (3600000 * (dto / 60 * -1)));
    });
    self.date.subscribe(function (_date) {
        self.timezoneOffset(_date.getTimezoneOffset());
        self.gmt(_date.toGMTString());
    });
    self.refresh = function () {
        self.date(new Date());
    };
}

function BrowserSpecsModel() {
    var self = this,
        bd = main.BrowserDetect,
        nav = navigator;
    self.name = bd.browser;
    self.nameFormal = nav.appName;
    self.version = bd.version;
    self.versionFormal = nav.appVersion;
    self.platform = bd.OS;
    self.platformFormal = nav.platform;
    self.agent = nav.userAgent;
}

function showMap(latitude, longitude, msg) {
    var map_convas = document.getElementById("map"),
        myLatLng = new google.maps.LatLng(latitude, longitude),
        myOptions = {
            center: myLatLng,
            zoom: 10,
            mapTypeId: google.maps.MapTypeId.HYBRID
        },
        map = new google.maps.Map(map_convas, myOptions);
    console.dir(map);
    var marker = new google.maps.Marker({
        map: map,
        position: myLatLng,
        animation: google.maps.Animation.Drop,
        title: msg || 'Testing Location'
    });
    marker.setMap(map);
}

ko.bindingHandlers.windowResize = {
    init: function (element, valueAccessor, allBindings, viewModel) {
        var options = valueAccessor(),
            bindings = allBindings(),
            updateWidth = bindings.updateWidth;

        //$(element).resizable(options);
        var $parent = $(element).parent();
        $(window).resize(function (evt, ui) {
            if ($parent.width() > 0) {
                updateWidth($parent.width());
            } else {
                updateWidth(screen.width / 4);
            }
        });

    }
};