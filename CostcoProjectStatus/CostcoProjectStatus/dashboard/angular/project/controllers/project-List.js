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
    .config(function ($routeProvider) {
        $routeProvider
          .when('/ProjectList', {
              templateUrl: 'angular/project/views/ProjectList.html',
              controller: 'projectListCtrl'
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
        { id: '1', Vname: 'Warehouse Solutions' },
        { id: '2', Vname: 'Merchandising Solutions' },
        { id: '3', Vname: 'Membership Solutions' },
        { id: '4', Vname: 'Distribution Solutions' },
        { id: '5', Vname: 'International Solutions' },
        { id: '6', Vname: 'Ancillary Solutions' },
        { id: '7', Vname: 'eBusiness Solutions' },
        { id: '8', Vname: 'Corporate Solutions' }
    ];
    })
    .controller('projectListCtrl', ['$scope', '$http', function ($scope, $http) {
     $http({ method: 'GET', url: 'http://localhost:64747/ProjectList/Display' }).success(function (data)
     {
         $scope.projectListData = data;
     });
    }])

;