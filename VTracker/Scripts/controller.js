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
})


myApp.controller('categoryCtrl', function getCategories($scope, $http) {
    $scope.dataList = [];
    $scope.editing = false;
    $scope.loading = true;
    getCategories();
    $scope.loading = false;
    //alert($scope.loading);

    function getCategories() {
        $http.get('/Category/GetCategories').success(function (response) {
            if (response != null || response != 'undefined') {
                $scope.dataList = response;

            }
        })
        .error(function () {
            alert('An Error occurred.');
        })
    }
    $scope.editCategory = function (Id, Description) {
        $scope.editing = true;
        $scope.Id = Id;
        $scope.Description = Description;
    }

    $scope.saveData() = function() {
        if ($scope.Id == 0) {
            // insert new record
        }
        else {
            // update record
            $http.get('/Category/Edit/' + $scope.Id, { params: { Description: $scope.Description } }).success(function (data) {
                $scope.StudentsUpdated = data;
                alert($scope.StudentsUpdated);
            })
        .error(function () {
            $scope.error = "An Error has occured while saving";
        });

        }
    }
})

.filter("parseDate", function() {
    var re = /\/Date\(([0-9]*)\)\//;
    return function(x) {
        var m = x.match(re);
        if( m ) return new Date(parseInt(m[1]));
        else return null;
    };
});