
function koVisibleInit(element, valueAccessor) {
    var value = ko.unwrap(valueAccessor());
    $(element).toggle(value);
};

ko.bindingHandlers.slideVisible = {
    init: koVisibleInit,
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            valueUnwrapped = ko.unwrap(value),
            duration = allBindings.slideDuration || 400;

        if (valueUnwrapped == true) {
            $(element).slideDown(duration);
        } else {
            $(element).slideUp(duration);
        }
    }
};

ko.bindingHandlers.fadeVisible = {
    init: koVisibleInit,
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            valueUnwrapped = ko.unwrap(value),
            duration = allBindings.slideDuration || 400;

        if (valueUnwrapped) {
            $(element).fadeIn(duration / 2);
        } else {
            $(element).fadeOut(duration);
        }
    }
};

ko.bindingHandlers.visibleFX = {
    init: koVisibleInit,
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            valueUnwrapped = ko.unwrap(value),
            effect = allBindings.effect || 'blind',
            duration = allBindings.duration || 400,
            options = allBindings.options || {};

        if (valueUnwrapped) {
            $(element).show(effect, options, duration);
        } else {
            $(element).hide(effect, options, duration);
        }
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

ko.bindingHandlers.attrLink = {
    update: function (element, valueAccessor, allBindings, viewModel) {
        var link = ko.unwrap(valueAccessor()),
            bindings = allBindings(),
            timer = bindings.timer || 400;
        if (link && link.length) {
            $(element).prop('href', link);
        } else {
            $(element).removeProp('href');
        }
    }
};
