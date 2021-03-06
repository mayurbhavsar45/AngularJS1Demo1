/*!
 * validation.js for NatRIX Software Private Limited
 *
 * Copyright 2014 v1.3.0
 * Created By Amit Parekh
 * Created Date 10/07/2014
 * Modify By Amit Parekh
 * Modify Date 31/07/2014
 */

//------------------[ User defined functions ]------------------//
function validateData(requiredClass) {
    var cnt = 0;
    $.each($("." + (requiredClass != undefined ? requiredClass : "nat-required")), function () {
        if ($(this).val() == "") {
            showErrorMessage($(this), "requirederrormessage");
            cnt++;
        }
    });

    $.each($(".nat-custom"), function () {
        var customFunction = $(this).attr("customfunction");
        if (customFunction != undefined) {
            if ($(this).parent().find("span.nat-err-msg").length == 0 || $(this).parent().find("span.uniqueerrormessage").length == 1) {
                var result = window[customFunction]($(this).val());
                if (result) {
                    showErrorMessage($(this), "customerrormessage");
                    cnt++;
                }
                else {
                    hideErrorMessage($(this), "customerrormessage");
                }
            }
            else {
                if ($(this).parent().find("span.nat-err-msg").length > 1) {
                    hideErrorMessage($(this), "customerrormessage");
                }
            }
        }
        else {
            hideErrorMessage($(this), "customerrormessage");
        }
    });

    if (cnt > 0) {
        return false;
    }
    else {
        $.each($(".nat-err-msg"), function () {
            if ($(this).text() != "" && $(this).css('display') == 'block') {
                cnt++;
            }
        });
        if (cnt > 0) {
            return false;
        }
        else {
            return true;
        }
    }
}

function showErrorMessage(obj, errorMessageAttr) {
    obj.addClass("required-text");
    var errorMessage = obj.attr(errorMessageAttr);
    if (obj.parent().find("span." + errorMessageAttr).length == 0) {
        obj.parent().append("<span class='" + errorMessageAttr + " nat-err-msg'>" + errorMessage + "</span>");
    }
    else {
        obj.parent().find("span." + errorMessageAttr).text(errorMessage);
    }
}

function hideErrorMessage(obj, errorMessageAttr) {
    obj.parent().find("span." + errorMessageAttr).remove();
    if (obj.parent().find("span.nat-err-msg").length == 0) {
        obj.removeClass("required-text");
    }
}

function resetValidation() {
    $(".nat-err-msg").each(function () {
        $(this).remove();
    });
    $(".required-text").each(function () {
        $(this).removeClass("required-text");
    });
}

//------------------[ Validation Script ]------------------//
$(document).ready(function () {

    $.each($(".nat-compare"), function () {
        var basefield = $(this).attr("comparefieldid");
        $("#" + basefield).addClass("nat-compare-source");
        $("#" + basefield).attr("destfieldid", $(this).attr("id"));
    });

    $(".modal").on('hidden', function () {
        resetValidation();
    });

    //------------------[ focus Events ]------------------//

    $(".nat-required").focus(function () {
        hideErrorMessage($(this), "requirederrormessage");
    });

    //------------------[ keydown Events ]------------------//

    //A-Z : 65-90
    //0-9 : 48-57, Numpad 0-9 : 96-105
    //Space : 32
    //Backspace : 8
    //Delete : 46
    //Enter : 13
    //Tab : 9
    //Shift : 16
    //Esc : 27
    //Ctrl : 17
    //Arrow : 37-40
    //Function Keys (F1-F12) : 112-123

    $(".nat-alphabet").keydown(function (event) {
        if ($(this).attr("allowspace") == "true") {
            if (event.which == 32) {
                return true;
            }
        }
        if (!((event.which >= 65 && event.which <= 90) || event.which == 8 || event.which == 46 || event.which == 13 || event.which == 9 || event.which == 27 || event.which == 17 || (event.which >= 37 && event.which <= 40) || (event.which >= 112 && event.which <= 123))) {
            event.preventDefault();
        }
    });

    $(".nat-number").keydown(function (event) {
        if ($(this).attr("allowspace") == "true") {
            if (event.which == 32) {
                return true;
            }
        }
        if (!event.shiftKey || (event.shiftKey && event.which == 9)) {
            if (!((event.which >= 48 && event.which <= 57) || (event.which >= 96 && event.which <= 105) || event.which == 8 || event.which == 46 || event.which == 13 || event.which == 9 || event.which == 27 || event.which == 17 || (event.which >= 37 && event.which <= 40) || (event.which >= 112 && event.which <= 123))) {
                event.preventDefault();
            }
        }
        else {
            event.preventDefault();
        }
    });

    $(".nat-alphanumeric").keydown(function (event) {
        if ($(this).attr("allowspace") == "true") {
            if (event.which == 32) {
                return true;
            }
        }
        if (!event.shiftKey || (event.shiftKey && event.which == 9) || (event.shiftKey && (event.which >= 65 && event.which <= 90))) {
            if (!((event.which >= 65 && event.which <= 90) || (event.which >= 48 && event.which <= 57) || (event.which >= 96 && event.which <= 105) || event.which == 8 || event.which == 46 || event.which == 13 || event.which == 9 || event.which == 27 || event.which == 17 || (event.which >= 37 && event.which <= 40) || (event.which >= 112 && event.which <= 123))) {
                event.preventDefault();
            }
        }
        else {
            event.preventDefault();
        }
    });

    //------------------[ blur Events ]------------------//
    $(".nat-required").blur(function () {
        if ($(this).val() == "") {
            showErrorMessage($(this), "requirederrormessage");
        }
        else {
            hideErrorMessage($(this), "requirederrormessage");
        }
    });

    $(".nat-alphabet").blur(function () {
        if ($(this).val() != "") {
            if ($(this).attr("minlength") != undefined) {
                if ($(this).val().length < $(this).attr("minlength")) {
                    showErrorMessage($(this), "valuelengtherrormessage");
                }
                else {
                    hideErrorMessage($(this), "valuelengtherrormessage");
                }
            }
            var regExp = new RegExp(/^[a-zA-Z]+$/i);
            if ($(this).attr("allowspace") == "true") {
                regExp = new RegExp(/^[a-zA-Z\s]+$/i);
            }
            if (regExp.test($(this).val())) {
                hideErrorMessage($(this), "valueerrormessage");
            }
            else {
                showErrorMessage($(this), "valueerrormessage");
            }
        }
        else {
            hideErrorMessage($(this), "valuelimiterrormessage");
            hideErrorMessage($(this), "valueerrormessage");
        }
    });

    $(".nat-number").blur(function () {
        if ($(this).val() != "") {
            if ($(this).attr("minlength") != undefined) {
                if ($(this).val().length < $(this).attr("minlength")) {
                    showErrorMessage($(this), "valuelengtherrormessage");
                }
                else {
                    hideErrorMessage($(this), "valuelengtherrormessage");
                }
            }
            var regExp = new RegExp(/^[\d]+$/i);
            if ($(this).attr("allowspace") == "true") {
                regExp = new RegExp(/^[\d\s]+$/i);
            }
            if (regExp.test($(this).val())) {
                hideErrorMessage($(this), "valueerrormessage");
            }
            else {
                showErrorMessage($(this), "valueerrormessage");
            }
        }
        else {
            hideErrorMessage($(this), "valuelengtherrormessage");
            hideErrorMessage($(this), "valueerrormessage");
        }
    });

    $(".nat-alphanumeric").blur(function () {
        if ($(this).val() != "") {
            if ($(this).attr("minlength") != undefined) {
                if ($(this).val().length < $(this).attr("minlength")) {
                    showErrorMessage($(this), "valuelengtherrormessage");
                }
                else {
                    hideErrorMessage($(this), "valuelengtherrormessage");
                }
            }
            var regExp = new RegExp(/^[a-zA-Z\d]+$/i);
            if ($(this).attr("allowspace") == "true") {
                regExp = new RegExp(/^[a-zA-Z\d\s]+$/i);
            }
            if (regExp.test($(this).val())) {
                hideErrorMessage($(this), "valueerrormessage");
            }
            else {
                showErrorMessage($(this), "valueerrormessage");
            }
        }
        else {
            hideErrorMessage($(this), "valuelengtherrormessage");
            hideErrorMessage($(this), "valueerrormessage");
        }
    });

    $(".nat-string").blur(function () {
        if ($(this).val() != "") {
            if ($(this).attr("minlength") != undefined) {
                if ($(this).val().length < $(this).attr("minlength")) {
                    showErrorMessage($(this), "valuelengtherrormessage");
                }
                else {
                    hideErrorMessage($(this), "valuelengtherrormessage");
                }
            }
        }
        else {
            hideErrorMessage($(this), "valuelengtherrormessage");
        }
    });

    $(".nat-email").blur(function () {
        var RegPattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
        if ($(this).val() != "") {
            var minValues = $(this).attr("minvalues");
            var maxValues = $(this).attr("maxvalues");
            var values = $(this).val().split(",");

            if (minValues == undefined && maxValues == undefined) {
                if (values.length > 1) {
                    showErrorMessage($(this), "valueerrormessage");
                    return;
                }
            } else if (minValues != "" && minValues != undefined) {
                if (values.length < minValues) {
                    showErrorMessage($(this), "valuelimiterrormessage");
                    return;
                }
                else {
                    hideErrorMessage($(this), "valuelimiterrormessage");
                }
            } else if (maxValues != "" && maxValues != undefined) {
                if (values.length > maxValues) {
                    showErrorMessage($(this), "valuelimiterrormessage");
                    return;
                }
                else {
                    hideErrorMessage($(this), "valuelimiterrormessage");
                }
            }

            if (values.length > 0) {
                for (var i = 0; i < values.length; i++) {
                    if (!RegPattern.test(values[i].trim())) {
                        showErrorMessage($(this), "valueerrormessage");
                        return;
                    }
                }
                if (i == values.length) {
                    hideErrorMessage($(this), "valueerrormessage");
                }
            }
            else {
                hideErrorMessage($(this), "valueerrormessage");
            }
        }
        else {
            hideErrorMessage($(this), "valueerrormessage");
            hideErrorMessage($(this), "valuelimiterrormessage");
        }
    });

    $(".nat-url").blur(function () {

        var RegPattern = new RegExp(/(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?/);
        if ($(this).val() != "") {
            var minValues = $(this).attr("minvalues");
            var maxValues = $(this).attr("maxvalues");
            var values = $(this).val().split(",");

            if (minValues == undefined && maxValues == undefined) {
                if (values.length > 1) {
                    showErrorMessage($(this), "valueerrormessage");
                    return;
                }
            } else if (minValues != "" && minValues != undefined) {
                if (values.length < minValues) {
                    showErrorMessage($(this), "valuelimiterrormessage");
                    return;
                }
                else {
                    hideErrorMessage($(this), "valuelimiterrormessage");
                }
            } else if (maxValues != "" && maxValues != undefined) {
                if (values.length > maxValues) {
                    showErrorMessage($(this), "valuelimiterrormessage");
                    return;
                }
                else {
                    hideErrorMessage($(this), "valuelimiterrormessage");
                }
            }

            if (values.length > 0) {
                for (var i = 0; i < values.length; i++) {
                    if (!RegPattern.test(values[i].trim())) {
                        showErrorMessage($(this), "valueerrormessage");
                        return;
                    }
                }
                if (i == values.length) {
                    hideErrorMessage($(this), "valueerrormessage");
                }
            }
            else {
                hideErrorMessage($(this), "valueerrormessage");
            }
        }
        else {
            hideErrorMessage($(this), "valueerrormessage");
            hideErrorMessage($(this), "valuelimiterrormessage");
        }
    });

    $(".nat-regex").blur(function () {
        var RegPattern = new RegExp($(this).attr("valueexpression"));
        if ($(this).val() != "") {
            var minValues = $(this).attr("minvalues");
            var maxValues = $(this).attr("maxvalues");
            var values = $(this).val().split(",");

            if (minValues == undefined && maxValues == undefined) {
                if (values.length > 1) {
                    showErrorMessage($(this), "valueerrormessage");
                    return;
                }
            } else if (minValues != "" && minValues != undefined) {
                if (values.length < minValues) {
                    showErrorMessage($(this), "valuelimiterrormessage");
                    return;
                }
                else {
                    hideErrorMessage($(this), "valuelimiterrormessage");
                }
            } else if (maxValues != "" && maxValues != undefined) {
                if (values.length > maxValues) {
                    showErrorMessage($(this), "valuelimiterrormessage");
                    return;
                }
                else {
                    hideErrorMessage($(this), "valuelimiterrormessage");
                }
            }

            if (values.length > 0) {
                for (var i = 0; i < values.length; i++) {
                    if (!RegPattern.test(values[i].trim())) {
                        showErrorMessage($(this), "valueerrormessage");
                        return;
                    }
                }
                if (i == values.length) {
                    hideErrorMessage($(this), "valueerrormessage");
                }
            }
            else {
                hideErrorMessage($(this), "valueerrormessage");
            }
        }
        else {
            hideErrorMessage($(this), "valueerrormessage");
            hideErrorMessage($(this), "valuelimiterrormessage");
        }
    });

    $(".nat-compare").blur(function () {
        var basefield = $(this).attr("comparefieldid");
        if ($(this).val() != "") {
            if ($(this).val() != $("#" + basefield).val()) {
                showErrorMessage($(this), "compareerrormessage");
            }
            else {
                hideErrorMessage($(this), "compareerrormessage");
            }
        }
        else {
            hideErrorMessage($(this), "compareerrormessage");
        }
    });

    $(".nat-compare-source").blur(function () {
        var destfield = $(this).attr("destfieldid");
        if ($(this).val() != "") {
            if ($(this).val() != $("#" + destfield).val() && $("#" + destfield).val() != "") {
                showErrorMessage($("#" + destfield), "compareerrormessage");
            }
            else {
                hideErrorMessage($("#" + destfield), "compareerrormessage");
            }
        }
        else {
            hideErrorMessage($("#" + destfield), "compareerrormessage");
        }
    });

    $(".nat-unique").blur(function () {
        var uniqueFunction = $(this).attr("uniquefunction");
        if (uniqueFunction != undefined) {
            if ($(this).parent().find("span.nat-err-msg").length == 0 || $(this).parent().find("span.uniqueerrormessage").length == 1) {
                var result = window[uniqueFunction]($(this).val());
                if (result) {
                    showErrorMessage($(this), "uniqueerrormessage");
                }
                else {
                    hideErrorMessage($(this), "uniqueerrormessage");
                }
            }
            else {
                if ($(this).parent().find("span.nat-err-msg").length > 1) {
                    hideErrorMessage($(this), "uniqueerrormessage");
                }
            }
        }
        else {
            hideErrorMessage($(this), "uniqueerrormessage");
        }
    });

    $(".nat-custom").click(function () {
        var customFunction = $(this).attr("customfunction");
        if (customFunction != undefined) {
            var result = window[customFunction]($(this).val());
            if (result) {
                showErrorMessage($(this), "customerrormessage");
            }
            else {
                hideErrorMessage($(this), "customerrormessage");
            }
        }
        else {
            hideErrorMessage($(this), "customerrormessage");
        }
    });
});