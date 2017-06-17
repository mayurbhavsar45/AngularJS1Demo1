app.controller('AccountController', ['$rootScope', '$scope', '$http', 'toaster', function ($rootScope, $scope, $http, toaster) {
    $scope.AccountInfo = {};
    $scope.TransferAccountInfo = {AccountNo:""};
    $scope.AccountTransactions = {};
    $scope.Isprocess = true;
    $scope.init = function () {
        $scope.Account.Methods.GetAccountInfo();
    };
    $scope.toastmessage = function (msg, color) {
        toaster.pop(color, msg);
    };
    $scope.Account = {
        Deposit: {
            UserID: "",
            DepositAmount: "0.0000",
            TransactionPassword: "",
            Remark: "",
            Transfer_UserID:""
        },
        Withdraw: {
            UserID: "",
            WithdrawAmount: "0.0000",
            TransactionPassword: "",
            Remark: ""
        },      
        Methods: {
            ClearData: function () {
                try{
                    $scope.Account.Withdraw.UserID = "";
                    $scope.Account.Withdraw.TransactionPassword = "";
                    $scope.Account.Withdraw.Remark = "";
                }catch(e){}
                try{
                    $scope.Account.Deposit.UserID = "";
                    $scope.Account.Deposit.TransactionPassword = "";
                    $scope.Account.Deposit.Remark = "";
                } catch (e) { }
                $scope.TransferAccountInfo.AccountNo = "";
            },
            GetAccountInfo: function () {
                ShowLoader();
                var obj = JSON.parse(GetValueFromLocalStorage("CurrentUser"));
                $scope.Account.Services.GetAccountInfo(obj.ID);
            },
            SetAccountInfo: function (data) {
                if (data == null || data == '') {
                    $scope.nodata = true;
                }
                else {
                    $scope.nodata = false;
                    $scope.AccountInfo = JSON.parse(data);
                }
                $scope.Isprocess = false;
                HideLoader();
            },
            CashDeposit: function () {
                if (validateData()) {
                    if (parseFloat($scope.Account.Deposit.DepositAmount) > 0) {
                        ShowLoader();
                        var obj = JSON.parse(GetValueFromLocalStorage("CurrentUser"));
                        $scope.Account.Deposit.UserID = obj.ID;
                        $scope.Account.Services.CashDeposit($scope.Account.Deposit);
                    } else {
                        $scope.toastmessage("Please enter more than 0 values", "error");
                    }
                }
            },
            checkAccountAvailability: function () {                      
                if ($scope.TransferAccountInfo.AccountNo!="") {
                $scope.Account.Services.checkAccountAvailability($scope.TransferAccountInfo.AccountNo);
            }
            },
            SetCashDepositInfo: function (data) {
                HideLoader();
                if (data == "1") {
                    $scope.toastmessage("Successfully deposit your amount.", "success");
                    window.location.href = "/Admin/Dashboard";
                } else if (data == "0") {
                    $scope.toastmessage("Something went wrong with deposit.", "error");
                } else if (data == "-1") {
                    $scope.toastmessage("Your transaction password invalid.", "error");
                } else if (data == "-2") {
                    $scope.toastmessage("Your Bank account not found.", "error");
                }
            },
            CashWithdraw: function () {               
                if (validateData()) {                    
                    if (parseFloat($scope.Account.Withdraw.WithdrawAmount) > 0) {
                        ShowLoader();
                        var obj = JSON.parse(GetValueFromLocalStorage("CurrentUser"));
                        $scope.Account.Withdraw.UserID = obj.ID;
                        $scope.Account.Services.CashWithdraw($scope.Account.Withdraw);
                    } else {
                        $scope.toastmessage("Please enter more than 0 values", "error");
                    }
                }
            },
            CashTransfer: function () {              
                if (parseFloat($scope.Account.Withdraw.WithdrawAmount) > 0) {
                    ShowLoader();
                    var obj = JSON.parse(GetValueFromLocalStorage("CurrentUser"));
                    $scope.Account.Withdraw.UserID = obj.ID;
                    $scope.Account.Deposit.Transfer_UserID = $scope.TransferAccountInfo.UserID;
                    $scope.Account.Deposit.UserID= $scope.TransferAccountInfo.UserID;
                    $scope.Account.Deposit.DepositAmount = $scope.Account.Withdraw.WithdrawAmount;
                    $scope.Account.Services.CashTransfer($scope.Account.Withdraw, $scope.Account.Deposit);                   
                } else {
                    $scope.toastmessage("Please enter more than 0 values", "error");
                }
            },
            SetCashWithdrawInfo: function (data) {
                HideLoader();
                if (data == "1") {
                    $scope.toastmessage("Successfully withdraw your amount.", "success");
                    window.location.href = "/Admin/Dashboard";
                } else if (data == "0") {
                    $scope.toastmessage("Something went wrong with withdraw.", "error");
                } else if (data == "-1") {
                    $scope.toastmessage("Your transaction password invalid.", "error");
                } else if (data == "-2") {
                    $scope.toastmessage("Your Bank account is not found.", "error");
                } else if (data == "-3") {
                    $scope.toastmessage("You have not enough balance to withdraw amount.", "error");
                }
            },
            GetAccountTransaction: function () {
                ShowLoader();
                var obj = JSON.parse(GetValueFromLocalStorage("CurrentUser"));
                $scope.Account.Services.GetAccountStatements(obj.ID);
            },
            SetAccountTransaction: function (data) {
                if (data == null || data == '') {
                    $scope.nodata = true;
                }
                else {
                    $scope.nodata = false;
                    $scope.AccountTransactions = JSON.parse(data);
                }
                $scope.Isprocess = false;
                HideLoader();
            },
            SetCashTransfer: function (data) {
                HideLoader();
                if (data == null || data == '') {
                    $scope.toastmessage("Money Transfer Error!!!!", "error");
                } else {
                    $scope.TransferAccountInfo.AccountNo = "";
                    $scope.Account.Withdraw.WithdrawAmount = "";
                    $scope.Account.Withdraw.TransactionPassword = "";
                    $scope.toastmessage("Money Successfully Transfer!!!!", "success");
                }
            },
            setAccountAvailability: function (data) {              
                if (data == null || data == '') {
                    $scope.toastmessage("User Not Available on This Account No.", "error");
                }else{                   
                    $scope.TransferAccountInfo=JSON.parse(data);              
                 }
            }
        },
        Services: {
            GetAccountInfo: function (userid) {
                $http.get('/api/api/Account/Getinformation/' + userid).success($scope.Account.Methods.SetAccountInfo);
            },
            GetAccountStatements: function (userid) {
                $http.get('/api/api/Account/GetTransactions/' + userid).success($scope.Account.Methods.SetAccountTransaction);
            },
            CashDeposit: function (deposit) {
                $http.post('/api/api/Account/Deposit', deposit).success($scope.Account.Methods.SetCashDepositInfo);
            },
            CashWithdraw: function (withdraw) {                
                $http.post('/api/api/Account/Withdraw', withdraw).success($scope.Account.Methods.SetCashWithdrawInfo);
            },          
            CashTransfer: function (withdraw, deposit) {
                $http.post('/api/api/Account/Withdraw', withdraw).success();
                $http.post('/api/api/Account/TransferDeposit', deposit).success($scope.Account.Methods.SetCashTransfer);
            },
            checkAccountAvailability: function (AccountNo) {              
                $http.get('/api/api/Account/GetcheckAccountAvailability/' + AccountNo).success($scope.Account.Methods.setAccountAvailability);
             },
        }
    };
    $scope.toastmessage = function (msg, color) {
        toaster.pop(color, msg);
    };
    $scope.init();
}]).filter('jsonDate', ['$filter', function ($filter) {
    return function (input, format) {
        return (input) ? $filter('date')(parseInt(input.substr(6)), format) : '';
    };
}]);
