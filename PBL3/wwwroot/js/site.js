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