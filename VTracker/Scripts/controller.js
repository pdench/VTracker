var myApp = angular.module('myApp', []);

myApp.controller('activityCtrl', function ($scope, $http) {
    $scope.dataList = [];
    $scope.loading = true;
    //alert('in controller');
    getAllActivities();
    $scope.loading = false;

    function getAllActivities() {
        $http.get('/Activity/GetActivities').success(function (response) {
            if (response != null || response != 'undefined') {
                $scope.dataList = response;
            }
        })
    }


})

myApp.controller('vehicleCtrl', function ($scope, $http) {
    $scope.dataList = [];
    $scope.loading = true;
    getVehicles();
    $scope.loading = false;

    function getVehicles() {
        $http.get('/Vehicle/GetVehicles').success(function (response) {
            if (response != null || response != 'undefined') {
                $scope.dataList = response;

            }
    })
   
    }
}
)


myApp.controller('categoryCtrl', function getCategories($scope, $http) {
    $scope.dataList = [];
    $scope.loading = true;
    getCategories();
    $scope.loading = false;

    function getCategories() {
        $http.get('/Category/GetCategories').success(function (response) {
            if (response != null || response != 'undefined') {
                $scope.dataList = response;

            }
        })

    }
}
)

.filter("parseDate", function() {
    var re = /\/Date\(([0-9]*)\)\//;
    return function(x) {
        var m = x.match(re);
        if( m ) return new Date(parseInt(m[1]));
        else return null;
    };
});