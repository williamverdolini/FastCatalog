angular.module('catalog')
    .factory('MongoCatalogService', [
        'AbstractCatalogService',
        'MongoAPI',
        function (AbstractCatalogService, MongoAPI) {
            return new AbstractCatalogService(MongoAPI.service('Catalog/Search'));
        }
    ]);

