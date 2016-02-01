/*angular.module('costcoapp').controller('ProjectCntrl', ['$scope', '$http', function ($scope, $http) {
    $scope.name = "Test";
    $scope.project = "";
   /* $http({ method: 'GET', url: 'http://localhost:64747/ProjectList/Display' }).success(function (data)
    {
        $scope.projectListData = data;
    });*/
//}]);
angular.module('dashboardApp', [
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
          redirectTo: '/DashboardCtrl'
      });
    })
    .controller('dashboardCtrl', function ($scope, VerticalEnum) {
        console.log(VerticalEnum[0]);
        $scope.VEnum = VerticalEnum;
    })
    .controller('loginCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {

        $scope.completedCheck = false;
        $scope.model = {};
        $scope.submit = function () {
            console.log($scope.model.username);
            console.log($scope.model.password);
            $http({ method: 'GET', url: 'https://localhost:44300/AccountController/Login/'+$scope.model.username }).success(function (data) {
                console.log(data);
                $scope.completedCheck = true;
                console.log($scope.phaseEnum);
            }).error(function (data, status, headers, config) {
                console.log(status);
                console.log(data);
                console.log(headers);
                console.log(config);
            })
        }
    }])
    .controller('loginResultCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
        console.log($routeParams.vId);
        $http({ method: 'GET', url: 'https://localhost:44300/ProjectList/GetStatusUpdates' }).success(function (data) {
            console.log(data);
            console.log($routeParams.vId);
            $scope.completedCheck = true;
            console.log($scope.phaseEnum);
        }).error(function (data, status, headers, config) {
            console.log(status);
            console.log(data);
            console.log(headers);
            console.log(config);
        })
    }])
    .controller('projectListCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
        console.log($routeParams.vId);
        $http({ method: 'GET', url: 'https://localhost:44300/ProjectList/GetStatusUpdates' }).success(function (data)
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
        $http({ method: 'GET', url: 'https://localhost:44300/ProjectList/GetProjectUpdates/'+$routeParams.projectId }).success(function (data)
        {
            console.log(data);
            console.log($routeParams.projectId);
            $scope.statusUpdateList = data;
            $scope.vId = $scope.statusUpdateList[0].VerticalID;
            $scope.vName = VerticalEnum[$scope.vId];
            $scope.phaseEnums = PhaseEnum;
            $scope.pId = $routeParams.projectId
        }).error(function(data, status, headers, config) {
            console.log(status);
            console.log(data);
            console.log(headers);
            console.log(config);
        })
    }])
        .controller('statusDataCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum','PhaseEnum',function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
            console.log($routeParams.projectId);
            $http({ method: 'GET', url: 'https://localhost:44300/ProjectList/GetProjectUpdates/'+$routeParams.projectId+"/"+$routeParams.phaseId+"/"+$routeParams.statusSequence }).success(function (data)
            {
                console.log(data);
                console.log($routeParams.projectId);
                $scope.statusUpdateList = data;
                $scope.date = $scope.statusUpdateList[0].RecordDate;
                $scope.dataExtractionId = $scope.statusUpdateList[0].StatusSequence;
                $scope.vId = $scope.statusUpdateList[0].VerticalID;
                $scope.vName = VerticalEnum[$scope.vId];
                $scope.phase = PhaseEnum[$scope.statusUpdateList[0].PhaseID];
                $scope.pId = $routeParams.projectId
            }).error(function(data, status, headers, config) {
                console.log(status);
                console.log(data);
                console.log(headers);
                console.log(config);
            });
        }]);