window.registerClickOutside = function (element, dotnetHelper) {
    document.addEventListener("click", function (event) {
        if (!element.contains(event.target)) {
            dotnetHelper.invokeMethodAsync("CloseSuggestions");
        }
    });
};