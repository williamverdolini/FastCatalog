angular.module('catalog')
    //Mongo Restangular Service
    .factory('MongoAPI', [
        'Restangular',
        function (Restangular) {
        return Restangular.withConfig(function (RestangularConfigurer) {
            RestangularConfigurer.setBaseUrl('Mongo/api');
        });
    }])
    .factory('MongoCatalogService', [
        'AbstractCatalogService',
        'MongoAPI',
        function (AbstractCatalogService, MongoAPI) {
            return new AbstractCatalogService(MongoAPI.service('Catalog/Search'));
        }
    ]);

