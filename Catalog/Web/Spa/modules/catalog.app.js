/// <reference path="~/Scripts/angular.js" />
/// <reference path="~/Scripts/angular-material.js" />
/// <reference path="~/Scripts/restangular.js" />

angular.module('catalog.theme', ['ngMaterial'])
    .config(function ($mdThemingProvider, $mdIconProvider) {
        $mdThemingProvider.theme('default')
            .accentPalette('pink')
            .primaryPalette('blue-grey')
            .backgroundPalette('grey')

        $mdIconProvider
                .iconSet("action", "/Content/angular-material-icons/action.svg")
                .iconSet("alert", "/Content/angular-material-icons/alert.svg")
                .iconSet("av", "/Content/angular-material-icons/av.svg")
                .iconSet("communication", "/Content/angular-material-icons/communication.svg")
                .iconSet("content", "/Content/angular-material-icons/content.svg")
                .iconSet("device", "/Content/angular-material-icons/device.svg")
                .iconSet("editor", "/Content/angular-material-icons/editor.svg")
                .iconSet("file", "/Content/angular-material-icons/file.svg")
                .iconSet("hardware", "/Content/angular-material-icons/hardware.svg")
                .iconSet("image", "/Content/angular-material-icons/image.svg")
                .iconSet("maps", "/Content/angular-material-icons/maps.svg")
                .iconSet("navigation", "/Content/angular-material-icons/navigation.svg")
                .iconSet("notification", "/Content/angular-material-icons/notification.svg")
                .iconSet("social", "/Content/angular-material-icons/social.svg")
                .iconSet("toggle", "/Content/angular-material-icons/toggle.svg")
                .iconSet("nosql", "/Content/angular-material-icons/nosql.svg");
    });

angular.module('catalog', ['ui.router', 'catalog.theme', 'restangular'])
    .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            //Default layout (Abstract States)
            .state('layout', {
                url: '/',
                abstract: true,
                templateUrl: 'Spa/layouts/layout.html'
            })
            // One Column Layout (Abstract States)
            .state('layout.1col', {
                url: '',
                abstract: true,
                parent: 'layout',
                templateUrl: 'Spa/layouts/1col.html'
            })
            // Two Columns Layout (Abstract States)
            .state('layout.2col', {
                url: '',
                abstract: true,
                parent: 'layout',
                templateUrl: 'Spa/layouts/2col.html'
            })
            // Concrete States
            .state('intro', {
                url: 'intro',
                parent: 'layout.1col',
                onEnter: function () {
                    console.log("Entering Intro Page");
                },
                templateUrl: 'Spa/modules/nav/intro.html'
            })
            .state('elastic', {
                url: 'elastic?:attribute',
                params: { attribute: { array: true } },
                parent: 'layout.2col',
                onEnter: function () {
                    console.log("Entering Elastic Page");
                },
                resolve: {
                    apiData: ['ElasticCatalogService', '$stateParams',
                        function (ElasticCatalogService, $stateParams) {
                            return ElasticCatalogService.Search($stateParams.attribute);
                        }],
                    selectedValues: ['ElasticCatalogService', '$stateParams',
                        function (ElasticCatalogService, $stateParams) {
                            return ElasticCatalogService.SelectedValues($stateParams.attribute);
                        }]
                },
                views: {
                    'search': {
                        templateUrl: 'Spa/modules/catalog/search.html',
                        controller: 'SearchController'
                    },
                    'grid': {
                        templateUrl: 'Spa/modules/catalog/grid.html',
                        controller: 'GridController'
                    }
                }
            })
            .state('mongo', {
                url: 'mongo?:attribute',
                params: { attribute: { array: true } },
                parent: 'layout.2col',
                onEnter: function () {
                    console.log("Entering MongoDB Page");
                },
                resolve: {
                    apiData: ['MongoCatalogService', '$stateParams',
                        function (MongoCatalogService, $stateParams) {
                            return MongoCatalogService.Search($stateParams.attribute);
                        }],
                    selectedValues: ['MongoCatalogService', '$stateParams',
                        function (MongoCatalogService, $stateParams) {
                            return MongoCatalogService.SelectedValues($stateParams.attribute);
                        }]
                },
                views: {
                    'search': {
                        templateUrl: 'Spa/modules/catalog/search.html',
                        controller: 'SearchController'
                    },
                    'grid': {
                        templateUrl: 'Spa/modules/catalog/grid.html',
                        controller: 'GridController'
                    }
                }
            });

        // For any unmatched url, redirect to HomePage
        $urlRouterProvider.otherwise('/intro');
    })
    //Elastic Restangular Service
    .factory('ElasticAPI', [ 'Restangular', function (Restangular) {
        return Restangular.withConfig(function (RestangularConfigurer) {
            RestangularConfigurer.setBaseUrl('Elastic/api');
        });
    }])
    //Mongo Restangular Service
    .factory('MongoAPI', [ 'Restangular', function (Restangular) {
        return Restangular.withConfig(function (RestangularConfigurer) {
            RestangularConfigurer.setBaseUrl('Mongo/api');
        });
    }])
    .run(function ($rootScope, Restangular) {
        // Restangular general configurations
        Restangular.addRequestInterceptor(function (element) {
            $rootScope.$loading = true;
            return element;
        });

        Restangular.addResponseInterceptor(function (data) {
            $rootScope.$loading = false;
            return data;
        });
    })

angular.module('catalog')
    .controller('ToggleController', [
        '$scope', '$rootScope', '$mdSidenav',
        function ($scope, $rootScope, $mdSidenav) {
            $scope.actions = {
                openLeftNav: function () {
                    $mdSidenav('left').toggle();
                }
            }
        }
    ]);
