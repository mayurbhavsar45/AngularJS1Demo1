app.controller('MessageController', ['$rootScope', '$scope', '$http', 'toaster', function ($rootScope, $scope, $http, toaster) {
    $scope.TelephoneNo = '';
    $scope.TelephoneLoginID = '';
    $scope.Password = '';
    $scope.nodata = false;
    $scope.MessageItems = [];
    $scope.BookingObject;
    $scope.ReasonForDelay = {
        Message: '',
        ETATime: '',
        Reason: '',
        BookingID: '',
        MessageType: '',
        CreatedDate: ''
    };

    $scope.init = function () {
        $scope.Message.Methods.GetBookings();
    };
    $scope.toastmessage = function (msg, color) {
        toaster.pop(color, msg);
    };
    $scope.Message = {
        Methods: {
            GetBookings: function () {
                ShowLoader();
                var obj = JSON.parse(GetValueFromLocalStorage("CurrentUser"));
                var value = $scope.Message.Methods.GetParameterByName("booking");
                if (value == undefined || value == '') {
                    alert("Missing parameter");
                }
                else {
                    $scope.TelephoneLoginID = obj.ID;
                    $scope.Message.Services.GetBookingById(obj.ID, value);
                }
            },
            SetBooking: function (data) {
                if (data != '') {
                    $scope.BookingObject = JSON.parse(data);
                    var transdate = new Date(parseInt($scope.BookingObject.TransactionDate.substr(6)));
                    $scope.BookingObject.TransactionDate = moment(transdate).format('YYYY.MM.DD HH:MM tt');
                    $scope.Message.Methods.GetMessages();
                }
                else {
                    alert("no data found")
                }
            },
            GetMessages: function () {
                $scope.Message.Services.GetReasonForDelayMessagesByBookingID($scope.TelephoneLoginID, $scope.BookingObject.ID);
            },
            SetMessages: function (data) {
                if (data != '') {
                    var obj = JSON.parse(data);
                    for (var i = 0; i < obj.length; i++) {
                        if (obj[i].MessageType == 3) {
                            var transdate = new Date(parseInt(obj[i].CreatedDate.substr(6)));
                            obj[i].CreatedDate = moment(transdate).format('YYYY.MM.DD hh:mma')
                            $scope.MessageItems.push(obj[i]);
                        }
                    }
                }
                else {
                    $scope.nodata = true;
                }
            },
            SelectMessage: function (bookingId) {
                window.location.href = "/Admin/Booking?booking=" + bookingId;
            },
            GetParameterByName: function (name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            },
            SaveReportMessage: function (messageType) {
                if (validateData() == false) {
                    return;
                }
                $scope.ReasonForDelay.BookingID = $scope.BookingObject.ID;
                $scope.ReasonForDelay.MessageType = messageType;
                $scope.ReasonForDelay.ETATime = '';
                $scope.ReasonForDelay.Message = '';
                $scope.Message.Services.SaveReportMessage($scope.ReasonForDelay);
            },
            SetReportDelay: function (data) {
                if (data == '') {
                    $scope.toastmessage("There is some problem in saving data.", 'error');
                }
                else {
                    SaveToLocalStorage("statusmessage", "Message has been saved");
                    window.location.href = "/Admin/ManageBooking";
                }
                HideLoader();
            }
        },
        Services: {
            GetBookingById: function (telephoneId, bookingId) {
                $http.get('/api/Booking/GetBookingById/' + telephoneId + '/' + bookingId).success($scope.Message.Methods.SetBooking);
            },
            GetReasonForDelayMessagesByBookingID: function (telephoneId, bookingId) {
                $http.get('/api/ReasonForDelay/GetReasonForDelayMessagesByBookingID/' + bookingId + '/' + telephoneId).success($scope.Message.Methods.SetMessages);
            },
            SaveReportMessage: function (obj) {
                $http.post('/api/ReasonForDelay/PutReasonForDelay', obj).success($scope.Message.Methods.SetReportDelay);
            }
        }
    };
    $scope.init();
}]);