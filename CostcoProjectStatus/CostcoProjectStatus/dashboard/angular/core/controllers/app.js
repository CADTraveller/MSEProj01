/// <reference path="../../project/views/ProjectList.html" />
'use strict';

/**
 * @ngdoc overview
 * @name To Do App
 * @description
 * # To do Application
 *
 * Main module of the application.
 */
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
              controller: 'dashboardCtrl'
          })
      .otherwise({
          redirectTo: 'angular/project/views/ProjectList.html'
      });
})
;

/*
var app = angular.module('dashboardApp', []);
 *   
 */