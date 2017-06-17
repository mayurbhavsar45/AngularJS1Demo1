function ShowLoader() {
    $("#loaderDIV").fadeIn();
}
function HideLoader() {
    $("#loaderDIV").fadeOut();
}

function ShowStatusMessage(message) {
    $("#StatusMessage").text(message);
    $("#StatusMessage").fadeIn();
    setTimeout(function () { $("#StatusMessage").fadeOut(); }, 5000);
}

function SetFocusToControl(controlId) {
    document.getElementById(controlId).focus();
}

function SaveToLocalStorage(key, value) {
    window.sessionStorage.setItem(key, value);
}

function RemoveFromLocalStorage(key) {
    window.sessionStorage.removeItem(key);
}

function GetValueFromLocalStorage(key) {
    return window.sessionStorage.getItem(key);
}