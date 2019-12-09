    // Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
    // for details on configuring this project to bundle and minify static web assets.

    // Write your JavaScript code.

current_font_size = 100;
function SetFontSize(change) {
    current_font_size += change;
    document.getElementById("page_body").style.fontSize = current_font_size + "%";
}
function UpdateBoard(id, name) {
    document.getElementById("boardName").innerHTML = name;
    document.getElementById("boardView").src = "/BoardView/Index/" + id;
}
