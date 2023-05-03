// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function Enable1() {
    $("#home").removeClass("fade").addClass("active");
    $("#menu1").removeClass("active").addClass("fade");
    $('#Titles').removeClass("active");
    $("#Titles").addClass("active");
    $("#AccNames").removeClass("active");
}
function Enable2() {
    $("#home").removeClass("active").addClass("fade");
    $("#menu1").removeClass("fade").addClass("active");
    $("#Titles").removeClass("active");
    $("#AccNames").addClass("active");
}

function On1() {
    $("#Pending-Table").removeClass("fade").addClass("active");
    $("#WaitingTake-Table").removeClass("active").addClass("fade");
    $("#WaitingReturn-Table").removeClass("active").addClass("fade");
    $("#Pending").addClass("active");
    $("#WaitingTake").removeClass("active");
    $("#WaitingReturn").removeClass("active");
}

function On2() {
    $("#Pending-Table").removeClass("active").addClass("fade");
    $("#WaitingTake-Table").removeClass("fade").addClass("active");
    $("#WaitingReturn-Table").removeClass("active").addClass("fade");
    $("#Pending").removeClass("active");
    $("#WaitingTake").addClass("active");
    $("#WaitingReturn").removeClass("active");
}

function On3() {
    $("#Pending-Table").removeClass("active").addClass("fade");
    $("#WaitingTake-Table").removeClass("active").addClass("fade");
    $("#WaitingReturn-Table").removeClass("fade").addClass("active");
    $("#Pending").removeClass("active");
    $("#WaitingTake").removeClass("active");
    $("#WaitingReturn").addClass("active");

}