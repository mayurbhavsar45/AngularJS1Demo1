app.controller('BookingController', ['$rootScope', '$scope', '$http', 'toaster', function ($rootScope, $scope, $http, toaster) {
    $scope.TelephoneNo = '';
    $scope.Password = '';
    $scope.nodata = false;
    $scope.BookingItems = [];
    $scope.BookingObject;
    $scope.ReasonForDelay = {
        Message: '',
        ETATime: '',
        Reason: 'Traffic jam',
        BookingID: '',
        MessageType: '',
    }
    $scope.init = function () {
        $scope.Booking.Methods.GetBookings();
    };
    $scope.toastmessage = function (msg, color) {
        toaster.pop(color, msg);
    };
    $scope.Booking = {
        Methods: {
            GetBookings: function () {
                ShowLoader();
                var obj = JSON.parse(GetValueFromLocalStorage("CurrentUser"));
                var value = $scope.Booking.Methods.GetParameterByName("booking");
                if (value == undefined || value == '') {
                    $scope.Booking.Services.GetBookings(obj.ID);
                }
                else {
                    $scope.Booking.Services.GetBookingById(obj.ID, value);
                }
            },
            SetBookingInfo: function (data) {
                if (data == null || data == '') {
                    $scope.nodata = true;
                    alert($scope.nodata);
                }
                else {
                    $scope.BookingItems = JSON.parse(data);
                    for (var i = 0; i < $scope.BookingItems.length; i++) {
                        var transdate = new Date(parseInt($scope.BookingItems[i].TransactionDate.substr(6)));
                        $scope.BookingItems[i].TransactionDate = moment(transdate).format('YYYY.MM.DD hh:mma')
                    }
                }
                HideLoader();
            },
            SetBooking: function (data) {
                if (data != '') {
                    $scope.BookingObject = JSON.parse(data);
                    var transdate = new Date(parseInt($scope.BookingObject.TransactionDate.substr(6)));
                    $scope.BookingObject.TransactionDate = moment(transdate).format('YYYY.MM.DD hh:mma');
                }
                else {
                    $scope.nodata = true;
                }

            },
            SelectBooking: function (bookingId) {
                window.location.href = "/Admin/Booking?booking=" + bookingId;
            },
            GetParameterByName: function (name) {
                name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                    results = regex.exec(location.search);
                return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
            },
            MoveAround: function (operation) {
                if (operation == 3) {
                    window.location.href = '/Admin/ManageMessage?booking=' + $scope.BookingObject.ID;
                }
                else if (operation == 2) {
                    window.location.href = '/Admin/ReportDelay?booking=' + $scope.BookingObject.ID;
                }
            },
            SaveHere: function (messageType) {
                
                $scope.ReasonForDelay.BookingID = $scope.BookingObject.ID;
                $scope.ReasonForDelay.Reason = "I am on my way";
                $scope.ReasonForDelay.MessageType = messageType;
                $scope.ReasonForDelay.ETATime = '';
                $scope.Booking.Services.SaveReportDelay($scope.ReasonForDelay);
            },
            SaveReportDelay: function (messageType) {
                $scope.ReasonForDelay.BookingID = $scope.BookingObject.ID;
                if ($scope.ReasonForDelay.Reason == "freetext") {
                    if (validateData() == false) {
                        return;
                    }
                    $scope.ReasonForDelay.Reason = $scope.ReasonForDelay.Message;
                }
                $scope.ReasonForDelay.ETATime = $scope.mytime;
                var d = $scope.ReasonForDelay.ETATime;
                var n = d.getHours();
                var ampm = 'am';
                if (n > 12) {
                    am = 'pm';
                    n = n - 12;
                }

                var m = d.getMinutes();
                $scope.ReasonForDelay.ETATime = '';
                $scope.ReasonForDelay.ETATime = n + " : " + m + " " + ampm;
                $scope.ReasonForDelay.MessageType = messageType;
                $scope.Booking.Services.SaveReportDelay($scope.ReasonForDelay);
            },
            SetReportDelay: function (data) {
                if (data == '') {
                    $scope.toastmessage("There is some problem in saving data.", 'error');
                }
                else {
                    SaveToLocalStorage("statusmessage", "Status Updated");
                    window.location.href = "/Admin/ManageBooking";
                    //window.location.href = "/Admin/Booking?booking=" + bookingId;
                    //window.location.reload();
                }
                HideLoader();
            }
        },
        Services: {
            GetBookings: function (telephoneId) {
                $http.get('/api/Booking/GetBookings/' + telephoneId).success($scope.Booking.Methods.SetBookingInfo);
            },
            GetBookingById: function (telephoneId, bookingId) {
                $http.get('/api/Booking/GetBookingById/' + telephoneId + '/' + bookingId).success($scope.Booking.Methods.SetBooking);
            },
            SaveReportDelay: function (data) {
                $http.post('/api/ReasonForDelay/PutReasonForDelay', data).success($scope.Booking.Methods.SetReportDelay);
            }
        },
    };
    $scope.mytime = new Date();

    $scope.hstep = 1;
    $scope.mstep = 1;

    $scope.options = {
        hstep: [1, 2, 3],
        mstep: [1, 5, 10, 15, 25, 30]
    };

    $scope.ismeridian = true;
    $scope.toggleMode = function () {
        $scope.ismeridian = !$scope.ismeridian;
    };

    $scope.update = function () {
        var d = new Date();
        d.setHours(14);
        d.setMinutes(0);
        $scope.mytime = d;
    };

    $scope.changed = function () {
        //$log.log('Time changed to: ' + $scope.mytime);
    };

    $scope.clear = function () {
        $scope.mytime = null;
    };
    $scope.init();
}]);