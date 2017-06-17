app.controller('LoginController', ['$rootScope', '$scope', '$http', 'toaster', function ($rootScope, $scope, $http, toaster) {
    $scope.EmailID = '';
    $scope.Password = '';
    $scope.CurrentUser = [];
    $scope.init = function () {
        var currentUser = GetValueFromLocalStorage("CurrentUser");
        if (currentUser != '' && currentUser != undefined) {
            window.location.href = "/Admin/Dashboard";
        }

    };

    $scope.User = {
        Methods: {
            ValidateLogin: function (form) {
                if (!form.$valid) {
                    $scope.toastmessage("Fill required field(s).", "error")
                    return;
                }

                ShowLoader();
                $scope.User.Services.ValidateLogin($scope.EmailID, $scope.Password);
            },
            SetUserInfo: function (data) {
                if (data == "") {
                    $scope.Password = '';
                    SetFocusToControl("form-password");
                    $scope.toastmessage("Invalid user name And password Or Your Account Is Not Verified", "error");
                    HideLoader();
                }
                else {
                    SaveToLocalStorage("CurrentUser", data);
                    setTimeout(function () { window.location.href = "/Admin/Dashboard"; HideLoader(); }, 5000);
                }

            }
        },
        Services: {
            ValidateLogin: function (Email, Password) {
                $http.get('/api/api/Login/ValidateLogin/' + Email + '/' + Password).success($scope.User.Methods.SetUserInfo);
            }
        }

    };
    $scope.toastmessage = function (msg, color) {
        toaster.pop(color, msg);
    };
    $scope.init();
}]);