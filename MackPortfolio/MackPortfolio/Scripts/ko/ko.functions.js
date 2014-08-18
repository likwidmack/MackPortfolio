
function mapObj2KoModel(obj) {
    var model = this;
    for (var _prop in obj) {
        if (typeof model[_prop] !== 'undefined') {
            var value = obj[_prop];
            if (!(model[_prop] instanceof Function)) {
                if ($.isArray(value)) {
                    model[_prop] = ko.observableArray();
                } else {
                    model[_prop] = ko.observable();
                }
            }
            if (model[_prop]() instanceof Function) {
                model[_prop].map(value);
            } else {
                model[_prop](value);
            }
        }
    }
}


///http://odetocode.com/blogs/scott/archive/2007/07/05/function-apply-and-function-call-in-javascript.aspx
ko.mapObservable = function (callback, modelFunc, obj) {
    var _actual = null,
        _temp = obj,
        _model = modelFunc,
        isArray = $.isArray(_temp),
        _newModel = function (_temp) {

        };

    _actual;

    var mapped = ko.computed({
        read: function () {
        },
        write: function (newValue) {
        }
    });

    mapped.map = function (obj) {

    }

    return mapped;
};

ko.protectedObservable = function (initialValue) {
    //private variables
    var _temp = initialValue;
    var _actual = ko.observable(initialValue);

    var result = ko.computed({
        read: _actual,
        write: function (newValue) {
            _temp = newValue;
        }
    }).extend({ notify: "always" });

    //commit the temporary value to our observable, if it is different
    result.commit = function () {
        if (_temp !== _actual()) {
            _actual(_temp);
        }
    };

    //notify subscribers to update their value with the original
    result.reset = function () {
        _actual.valueHasMutated();
        _temp = _actual();
    };

    return result;
};

ko.mapProtectedObservable = function () {
};

ko.onDemandObservable = function (callback, target) {
    var _value = ko.observable();

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
        deferEvaluation: true,
        owner: target
    }).extend({
        rateLimit: {
            timeout: 500,
            method: "always"
        }
    });

    //expose the current state, which can be bound against
    result.loaded = ko.observable();

    result.refresh = function () {
        result.loaded(false);
    };

    return result;
};


ko.loadOnDemandData = function (callback, target) {
    var _value = ko.observable();

    var result = ko.computed({
        read: function () {
            //if it has not been loaded, execute the supplied function
            if (!result.loaded() && target.toggleVisible()) {
                callback.call(target);
            }
            //always return the current value
            return _value();
        },
        write: function (newValue) {
            //indicate that the value is now loaded and set it
            result.loaded(true);
            //console.log('LOAD on Demand', newValue);
            _value(newValue);
        },
        deferEvaluation: true,
        owner: target
    }).extend({
        rateLimit: {
            timeout: 500,
            method: "always"
        }
    });

    //expose the current state, which can be bound against
    result.loaded = ko.observable();

    result.refresh = function () {
        result.loaded(false);
    };

    return result;
};


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

