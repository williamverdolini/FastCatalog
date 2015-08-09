angular.module('catalog')
    .controller('SearchController', [
        '$scope', 'selectedValues', 'apiData','$state',
        function ($scope, selectedValues, apiData, $state) {
            /// private methods
            var addSelectedItem = function (item, selected) {
                selected[item.Key] = selected[item.Key] || [];
                selected[item.Key].push(item.Value)
                return selected;
            }
            var removeSelectedItem = function (item, selected) {
                var index = selected[item.Key].indexOf(item.Value);
                selected[item.Key].splice(index, 1);
                if (selected[item.Key].length == 0) {
                    delete selected[item.Key];
                }
                return selected;
            }
            var prepareSearchAttributes = function (item, selected) {
                var attr = [];
                var selectedKey = null;
                for (var prop in selected) {
                    var key = {};
                    key[prop] = selected[prop];
                    if (prop === item.Key)
                        selectedKey = JSON.stringify(key);
                    else
                        attr.push(JSON.stringify(key))
                }
                if (selectedKey != null) {
                    attr.push(selectedKey);
                }
                return attr;
            }
            var getChipsFrom = function (selected) {
                var chips = [];
                for (var prop in selected) {
                    selected[prop].forEach(function (item) {
                        var chip = {
                            key: prop,
                            value: item
                        };
                        chips.push(chip)
                    });
                }
                return chips;
            }

            /// public model
            $scope.local = {
                aggregations: apiData.Aggregations,
                selected: selectedValues,
                chips: getChipsFrom(selectedValues)
            };

            /// public methods
            $scope.actions = {
                isSelected: function (key, value) {
                    return $scope.local.selected[key] !== undefined && $scope.local.selected[key].indexOf(value) > -1;
                    },
                toggle: function (item) {
                    if ($scope.actions.isSelected(item.Key, item.Value)) {
                        $scope.local.selected = removeSelectedItem(item, $scope.local.selected);
                    }
                    else {
                        $scope.local.selected = addSelectedItem(item, $scope.local.selected);
                    }
                    var attr = prepareSearchAttributes(item, $scope.local.selected);

                    $state.go($state.current.name, { attribute: attr }, { reload: true, location: true });
                }
            }
        }
    ]);