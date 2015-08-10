angular.module('catalog')
    .factory('ElasticCatalogService', [
        'AbstractCatalogService',
        'ElasticAPI',
        function (AbstractCatalogService, ElasticAPI) {
            return new AbstractCatalogService(ElasticAPI.service('Catalog/Search'));
        }
    ]);