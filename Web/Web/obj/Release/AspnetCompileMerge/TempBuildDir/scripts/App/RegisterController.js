app.controller('RegisterController', ['$rootScope', '$scope','$http', 'toaster','$location', function ($rootScope, $scope, $http,toaster, $location) {
    $scope.isvarified = false;
    $scope.isvarifiedsuccess = false;
    $scope.init = function () {
        var currentUser = GetValueFromLocalStorage("CurrentUser");
        if (currentUser != '' && currentUser != undefined) {
            //window.location.href = "/Admin/ManageBooking";
        }

    };

    $scope.validateTokenInit = function () {      
        ShowLoader();        
        var Token = location.search.substr(location.search.indexOf("=") + 1);
        $scope.User.Services.ValidateToken(Token);
    };
       
    $scope.User = {
        Item: {
            FirstName: "",
            LastName: "",
            Address: "",
            EmailID: "",
            Mobile: "",
            Password: "",
            TransactionPassword : ""
        },
        Methods: {
            NewRegister: function () {              
                if ($scope.User.Item.Password != $scope.User.Item.TransactionPassword) {
                    if (validateData()) {
                        ShowLoader();
                        $scope.User.Services.NewRegister($scope.User.Item);
                        var nextStepWizard = $('div.setup-panel div a[href="#step-2"]').parent().next().children("a");
                        nextStepWizard.removeAttr('disabled').trigger('click');
                        HideLoader();
                    }
                } else {
                    $scope.toastmessage("Can not Use LoginPassword and TransactionPassword same!!!", "error");
                }                
            },          
            SetUserInfo: function (data) {
                if (data == "") {
                    //$scope.Password = '';
                    //SetFocusToControl("form-password");
                    //$scope.toastmessage("Invalid user name or password.", "error");
                }
                else {
                    var nextStepWizard = $('div.setup-panel div a[href="#step-1"]').parent().next().children("a");
                    nextStepWizard.removeAttr('disabled').trigger('click');

                }
                HideLoader();
            },
            CheckEmailAvailabitity: function ()
            {               
                if ($scope.User.Item.EmailID != "") {
                    $scope.User.Services.CheckEmailAvailabitity($scope.User.Item.EmailID);
                }
            },
            EmailSuccess: function (data) {               
                if (JSON.parse(data) == 1) {
                    $scope.User.Item.EmailID = "";
                    $scope.toastmessage("Email Id With same Name Exist!!!", "error");
                }

            },
            ValidateSuccess: function (data) {              
                if (data.toString() == "True") {
                    $scope.isvarifiedsuccess = true;
                } else {
                    $scope.isvarifiedsuccess = false;
                }
                $scope.isvarified = true;
                HideLoader();
            }
        },
        Services: {
            NewRegister: function (user) {
                $http.post('/api/api/Register', user).success($scope.User.Methods.SetUserInfo);
            },
            ValidateToken: function (token) {              
                $http.get('/api/api/Register/ValidateToken/' + token).success($scope.User.Methods.ValidateSuccess);
            },
            CheckEmailAvailabitity: function (id) {
                $http.get('/api/api/Register/EmailAvailability/' + id+'/').success($scope.User.Methods.EmailSuccess);
            }
        }
    };
    $scope.toastmessage = function (msg, color) {
        toaster.pop(color, msg);
    };
    $scope.init();    
}]);