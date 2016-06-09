
var myApp = angular.module('myApp', ['ngGrid', 'ngRoute']);

// routing
myApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/Contact', {
            templateUrl: 'Contact.html',
        }).
        otherwise({
            redirectTo: '/'
        });
  }]);



myApp.controller("ContactController", ContactController);


