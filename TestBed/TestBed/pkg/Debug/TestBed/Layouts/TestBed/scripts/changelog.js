(function () {


    var app = angular.module('myApp', []);

    function Changelog() {
        this.load = function () {

            app.controller('changelog', function ($scope) {
                PageMethods.GetLog(function (response) {
                    $scope.$apply(function () {
                        $scope.changelogs = JSON.parse(response);
                    });
                });
            });
        }
    };
    var cl = new Changelog();

    cl.load();
    alert("hi");
}());