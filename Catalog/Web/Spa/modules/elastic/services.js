angular.module('catalog')
    //Elastic Restangular Service
    .factory('ElasticAPI', [
        'Restangular',
        function (Restangular) {
        return Restangular.withConfig(function (RestangularConfigurer) {
            RestangularConfigurer.setBaseUrl('Elastic/api');
        });
    }])
    .factory('ElasticCatalogService', [
        'AbstractCatalogService',
        'ElasticAPI',
        function (AbstractCatalogService, ElasticAPI) {
            return new AbstractCatalogService(ElasticAPI.service('Catalog/Search'));
        }
    ]);