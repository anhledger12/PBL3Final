// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function Enable1(id1, id2, id3, id4) {
    $(id1).addClass("active");
    $(id2).removeClass("active");
    $(id3).removeClass("fade").addClass("active");
    $(id4).removeClass("active").addClass("fade");
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