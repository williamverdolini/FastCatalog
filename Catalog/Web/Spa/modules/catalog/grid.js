angular.module('catalog')
    .controller('GridController', [
        '$scope', 'apiData',
        function ($scope, apiData) {
            $scope.local = {
                products: apiData.Results
            };
        }
    ]);