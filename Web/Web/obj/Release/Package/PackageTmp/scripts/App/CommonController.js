app.controller('CommonController', ['$rootScope', '$scope', '$http', 'toaster', function ($rootScope, $scope, $http, toaster) {
    $scope.Username = '';
    $scope.init = function () {
       
        var obj = JSON.parse(GetValueFromLocalStorage("CurrentUser"));
        try {
            if (obj != undefined && obj != null) {
                $scope.Username = obj.Username;

            }
            else {
                window.location.href = "/Home/Login"
            }
            
            var msg = GetValueFromLocalStorage("statusmessage");
            if (msg != '' && msg != undefined || msg != null) {
                $scope.toastmessage(msg);
                RemoveFromLocalStorage("statusmessage");
            }
        }
        catch (e) {
            alert(e.message);
        }
    }

    $scope.Logout = function () {
        RemoveFromLocalStorage("CurrentUser");
        window.location.href = "/";
    }
    $scope.toastmessage = function (msg) {
        toaster.pop('success', msg);
    };
}]);