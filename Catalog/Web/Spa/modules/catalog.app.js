/// <reference path="~/Scripts/angular.js" />
/// <reference path="~/Scripts/angular-material.js" />

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

angular.module('catalog', ['ui.router','catalog.theme'])
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
                url: 'elastic',
                parent: 'layout.2col',
                onEnter: function () {
                    console.log("Entering Elastic Page");
                },
                views: {
                    'search': {
                        templateUrl: 'Spa/modules/elastic/search.html'
                    },
                    'grid': {
                        templateUrl: 'Spa/modules/elastic/grid.html'
                    }
                }
            })
            //.state('intro', {
            //    url: 'intro',
            //    parent: 'layout.2cl',
            //    onEnter: function () {
            //        console.log("Entering Intro Page");
            //    },
            //    views: {
            //        'sidebar@layout.2cl': {
            //            templateUrl: 'Spa/modules/nav/sidebar.html',
            //            controller: 'SideBarCtrl'
            //        },
            //        '@layout.2cl': {
            //            templateUrl: 'Spa/modules/nav/intro.html'
            //        }
            //    }
            //})
            //.state('elastic', {
            //    url: 'elastic',
            //    parent: 'layout.2side',
            //    onEnter: function () {
            //        console.log("Entering Elastic Page");
            //    },
            //    views: {
            //        'sidebar@layout.2cl': {
            //            templateUrl: 'Spa/modules/nav/sidebar.html',
            //            controller: 'SideBarCtrl'
            //        },
            //        'search': {
            //            templateUrl: 'Spa/modules/elastic/search.html'
            //        },
            //        'grid': {
            //            templateUrl: 'Spa/modules/elastic/grid.html'
            //        }
            //    }
            //})
        ;

        // For any unmatched url, redirect to HomePage
        $urlRouterProvider.otherwise('/intro');
    });

angular.module('catalog')
    .controller('ToggleController',
    [
        '$scope',
        '$rootScope',
        '$mdSidenav',
        function ($scope, $rootScope, $mdSidenav) {
            $scope.actions = {
                openLeftNav: function () {
                    $mdSidenav('left').toggle();
                }
            }
        }
    ]);
