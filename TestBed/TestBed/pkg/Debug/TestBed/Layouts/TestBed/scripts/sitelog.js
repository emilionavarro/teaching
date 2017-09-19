(function () {
    var app = angular.module('myApp', ['ui.bootstrap']);

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    function Sitelog() {
        this.load = function () {
            var guid = getParameterByName('guid');

            if (guid != undefined) {
                app.controller('mysitelog', function ($scope) {
                    PageMethods.GetLog(guid, function (response) {
                        $scope.$apply(function () {
                            $scope.changelogs = JSON.parse(response);
                        });
                    });
                });
            }
        }
    };

    var sl = new Sitelog();

    sl.load();
}());