myApp.service("myService", function ($http) {
    // get all activities
    this.getActivities = function () {
        // debugger;
        alert('in service');
        return $http.get("/Activity/GetActivities");
    };


}
)