angular.module('catalog')
    .factory('AbstractCatalogService', [
        function () {
            // private methods
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

            // Service Constructor
            function AbstractCatalogService(restangularService) {
                this.service = restangularService;
            }

            // public methods
            AbstractCatalogService.prototype = {
                Search: function (input) {
                    input = parseParams(input);
                    var dataInput = { Attributes: [] };
                    input.forEach(function (item) {
                        for (var prop in item) {
                            dataInput.Attributes.push({ "Key": prop, "Values": item[prop] })
                        }
                    });
                    return this.service.post(dataInput);
                },
                SelectedValues: function (param) {
                    param = parseParams(param);

                    var selected = {};
                    if (angular.isDefined(param)) {
                        if (angular.isArray(param)) {
                            param.forEach(function (item) {
                                for (var prop in item) {
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

            return AbstractCatalogService;
        }
    ]);