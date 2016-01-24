// project-List.js
/*function projectListController() {
    alert("hello world 3!");
}
*/


angular.module('app').controller('projectListController', ['$scope', '$http', function ($scope, $http) {
    alert('controller initializing');
    $scope.name = "Test";
    $scope.project = "";
    $http({method: 'GET',url:'http://localhost:64747/ProjectList/Display'}).success(function(data)
    {
        $scope.projectListData = data;
    });
}]);
