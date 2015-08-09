angular.module('catalog')
    .factory('ElasticCatalogService', [
        'ElasticAPI',
        function (ElasticAPI) {
            var parseParams = function (param) {
                var newParams = []
                if (angular.isDefined(param)) {
                    if (angular.isArray(param)) {
                        param.forEach(function (entry) {
                            newParams.push(JSON.parse(entry));
                        })
                    }
                    else {
                        newParams.push(JSON.parse(param));
                    }
                }
                return newParams;
            }

            var service = ElasticAPI.service('Catalog/Search');

            return {
                Search: function (input) {
                    input = parseParams(input);
                    var dataInput = { Attributes: [] };
                    input.forEach(function (item) {
                        for (var prop in item) {
                            dataInput.Attributes.push({ "Key": prop, "Values": item[prop] })
                        }
                    });
                    return service.post(dataInput);
                },
                SelectedValues: function (param) {
                    param = parseParams(param);

                    var selected = {};
                    if (angular.isDefined(param)) {
                        if (angular.isArray(param)) {
                            param.forEach(function (item) {
                                for(var prop in item){
                                    selected[prop] = item[prop];
                                }
                            });
                        }
                        else {
                            selected = param;
                        }
                    }
                    return selected;
                }
            }
        }
    ]);