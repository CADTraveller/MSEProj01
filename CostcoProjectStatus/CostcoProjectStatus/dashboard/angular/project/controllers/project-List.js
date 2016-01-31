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
          .when('/ProjectList/:vId', {
              templateUrl: 'angular/project/views/ProjectList.html',
              controller: 'projectListCtrl'
          })
            .when('/ProjectUpdates/:projectId', {
                templateUrl: 'angular/project/views/ProjectUpdates.html',
                controller: 'statusUpdatesCtrl'
            })
            .when('/DashboardCtrl', {
                templateUrl: 'angular/project/views/Verticals.html',
                controller: 'dashboardCtrl'
            })
      .otherwise({
          redirectTo: '/DashboardCtrl'
      });
    })
    .controller('dashboardCtrl', function ($scope) {
    $scope.names = [
        { id: '0', Vname: 'Warehouse Solutions' },
        { id: '1', Vname: 'Merchandising Solutions' },
        { id: '2', Vname: 'Membership Solutions' },
        { id: '3', Vname: 'Distribution Solutions' },
        { id: '4', Vname: 'International Solutions' },
        { id: '5', Vname: 'Ancillary Solutions' },
        { id: '6', Vname: 'eBusiness Solutions' },
        { id: '7', Vname: 'Corporate Solutions' }
    ];
    })
    .controller('projectListCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
        console.log($routeParams.vId);
        $http({ method: 'GET', url: 'https://localhost:44300/ProjectList/GetStatusUpdates' }).success(function (data)
        {
            console.log(data);
            console.log($routeParams.vId);
            $scope.vId = $routeParams.vId;
            $scope.vName = VerticalEnum[$routeParams.vId];
            $scope.projectIdList = data.ProjectId;
            $scope.projectVIdList = data.ProjectVertical;
            $scope.projectPhaseList = data.ProjectLastPhase;
            $scope.projectUpdateList = data.ProjectLastUpdate;
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
            //console.log($routeParams.projectId);
            //$scope.vId = data.vId;
            //$scope.vName = VerticalEnum[$scope.vId];
            //$scope.phaseEnums = PhaseEnum;
            //$scope.pId = $routeParams.projectId
            //$scope.descriptionList = data.ProjectUpdateDescriptions;
            //$scope.phasesList = data.ProjectUpdatePhases;
            //$scope.datesList = data.ProjectDates;
            //$scope.updateKey = data.ProjectUpdateKey;
        }).error(function(data, status, headers, config) {
            console.log(status);
            console.log(data);
            console.log(headers);
            console.log(config);
        });
    }])

;