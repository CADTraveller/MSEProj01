﻿var dashboardModule = angular.module('dashboardApp', [
    'ngAnimate',
    'ngAria',
    'ngCookies',
    'ngMessages',
    'ngResource',
    'ngRoute',
    'ngSanitize',
    'ngTouch'
])
    .constant('VerticalEnum', {
        0: 'Warehouse Solutions',
        1: 'Merchandising Solutions',
        2: 'Membership Solutions',
        3: 'Distribution Solutions',
        4: 'International Solutions',
        5: 'Ancillary Solutions',
        6: 'eBusiness Solutions',
        7: 'Corporate Solutions'
    })
    .constant('PhaseEnum', {
        0: 'Start Up',
        1: 'Solution Outline',
        2: 'Macro Design',
        3: 'Micro Design',
        4: 'Build and Test',
        5: 'Deploy',
        6: 'Transition & Close'
    })
    .config(function ($routeProvider) {
        $routeProvider
           .when('/Login', {
                templateUrl: 'angular/project/views/Login.html',
                controller: 'loginCtrl'
           })
            .when('/Login/:loginId/:password', {
                templateUrl: 'angular/project/views/Login.html',
                controller: 'loginResultCtrl'
            })
          .when('/ProjectList/:vId', {
              templateUrl: 'angular/project/views/ProjectList.html',
              controller: 'projectListCtrl'
           })
            .when('/ProjectUpdates/:projectId', {
                templateUrl: 'angular/project/views/ProjectUpdates.html',
                controller: 'statusUpdatesCtrl'
            })
            .when('/ProjectData/:projectId/:phaseId/:statusSequence', {
                templateUrl: 'angular/project/views/ProjectData.html',
                controller: 'statusDataCtrl'
            })
            .when('/DashboardCtrl', {
                templateUrl: 'angular/project/views/Verticals.html',
                controller: 'dashboardCtrl'
            })
      .otherwise({
          redirectTo: '/Login'
      });
    })
    .controller('dashboardCtrl', function ($scope, VerticalEnum) {
        console.log(VerticalEnum[0]);
        $scope.VEnum = VerticalEnum;
    })
    .controller('loginCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {

        $scope.login = function () {
            /* var req = {
                 method: 'POST',
                 url: '../Account/ExternalLogin',
                 headers: {
                     'Content-Type': undefined
                 },
                 data: {
                     provider: 'google',
                     returnURL: '#/DashboardCtrl',
                     name: '__RequestVerificationToken',
                     type: 'hidden', value: 'L62szysgS5xEV4Aos8ZwX1wQaG4m4TaIwhYOae4smn5KD8XMK3_Z2gh7qu4rI1cIusSJmneKMiJXFxLcWXylkL0Nuc4oXCiyngvocvpKXvU1'
                 }
             };
             $http(req).then(function () {
                 console.log(data);
             });
         } */
            var postData = {
                provider: 'Google',
                returnURL: ''
            }
            $http.post('../Account/ExternalLogin', postData)
            .then(function (result) {
                alert("coool!");
                console.log(result.data);

            });
        }
    }])
    .controller('projectListCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
        console.log($routeParams.vId);
        $http({ method: 'GET', url: '../ProjectList/GetStatusUpdates' }).success(function (data)
        {
            console.log(data);
            console.log($routeParams.vId);
            $scope.vId = $routeParams.vId;
            $scope.vName = VerticalEnum[$routeParams.vId];
            $scope.projectList = data;
            $scope.phaseEnum = PhaseEnum; 
            console.log($scope.phaseEnum);
        }).error(function(data, status, headers, config) {
            console.log(status);
            console.log(data);
            console.log(headers);
            console.log(config);
        })
    }])
    .controller('statusUpdatesCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum','PhaseEnum',function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
        console.log($routeParams.projectId);
        $http({ method: 'GET', url: '../ProjectList/GetProjectUpdates/' + $routeParams.projectId }).success(function (data) {
            console.log(data);
            console.log($routeParams.projectId);
            $scope.statusUpdateList = data;
            $scope.vId = $scope.statusUpdateList[0].VerticalID;
            $scope.vName = VerticalEnum[$scope.vId];
            $scope.phaseEnums = PhaseEnum;
            $scope.pId = $routeParams.projectId;
            $scope.inProgressPhases = [];
            angular.forEach($scope.statusUpdateList, function (value, key) {
                console.log($scope.statusUpdateList[key].PhaseID);

                this.push($scope.statusUpdateList[key].PhaseID);

            }, $scope.inProgressPhases);
            console.log($scope.inProgressPhases);
        }).error(function (data, status, headers, config) {
            console.log(status);
            console.log(data);
            console.log(headers);
            console.log(config);
        })
    }])
        .controller('statusDataCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum','PhaseEnum',function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
            console.log($routeParams.projectId);
            console.log($routeParams.phaseId);
            console.log($routeParams.statusSequence);
            $http({ method: 'GET', url: '../ProjectList/GetProjectUpdates/'+$routeParams.projectId+"/"+$routeParams.phaseId+"/"+$routeParams.statusSequence }).success(function (data)
            {
                console.log(data);
                console.log($routeParams.projectId);
                $scope.statusUpdateList = data;
                $scope.date = $scope.statusUpdateList[0].RecordDate;
                $scope.dataExtractionId = $scope.statusUpdateList[0].StatusSequence;
                $scope.vId = $scope.statusUpdateList[0].VerticalID;
                $scope.vName = VerticalEnum[$scope.vId];
                $scope.phase = PhaseEnum[$scope.statusUpdateList[0].PhaseID];
                $scope.pId = $routeParams.projectId;
            }).error(function(data, status, headers, config) {
                console.log(status);
                console.log(data);
                console.log(headers);
                console.log(config);
            });
        }])
 .controller('TabsDemoCtrl', function ($scope, $window) {
        $scope.tabs = [
          { title: 'Dynamic Title 1', content: 'Dynamic content 1' },
          { title: 'Dynamic Title 2', content: 'Dynamic content 2', disabled: true }
        ];

        $scope.alertMe = function () {
            setTimeout(function () {
                $window.alert('You\'ve selected the alert tab!');
            });
        };

        $scope.model = {
            name: 'Tabs'
        };
    });