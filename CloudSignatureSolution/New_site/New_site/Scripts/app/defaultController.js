angular.module("defaultController", [])
.controller("DefaultController",

function ($scope) {
    $scope.takeValues = function (a, b) {
        var request = $.ajax(
            {
                url: "/api/takeValues/" + a + "/" + b,
                type: "get"
            });

        request.done(function (data) {
            $scope.suma = { data };
            $scope.$apply();
        });
    }
});