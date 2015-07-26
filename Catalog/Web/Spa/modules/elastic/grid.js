angular.module('catalog')
    .controller('ElasticGridController', [
        '$scope', 'apiData',
        function ($scope, apiData) {
            $scope.local = {
                products: apiData.Results
            };
        }
    ]);