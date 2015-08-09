angular.module('catalog')
    .controller('MongoGridController', [
        '$scope', 'apiData',
        function ($scope, apiData) {
            $scope.local = {
                products: apiData.Results
            };
        }
    ]);