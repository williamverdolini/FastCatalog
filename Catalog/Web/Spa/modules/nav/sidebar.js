/// <reference path="~/Scripts/angular.js" />
/// <reference path="~/Scripts/angular-material.js" />
angular.module('catalog')
    .controller('SideBarController', [
        '$scope',
        '$state',
        '$mdSidenav',
        function ($scope, $state, $mdSidenav) {
            $scope.actions = {
                navigateTo: function (stateName) {
                    $state.go(stateName);                    
                    $mdSidenav('left').toggle();
                }
            }
        }
    ]);

