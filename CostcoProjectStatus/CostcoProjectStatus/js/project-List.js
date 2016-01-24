// project-List.js
/*function projectListController() {
    alert("hello world 3!");
}
*/


angular.module('app').controller('projectListController', ['$scope', function ($scope) {
    // this code is never called
    alert('controller initializing');
    $scope.name = "Test";
    $scope.project = "";
}]);
