    // Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
    // for details on configuring this project to bundle and minify static web assets.

    // Write your JavaScript code.

var current_font_size = 100;
var boardView = document.getElementById("boardView");
var boardName = document.getElementById("boardName");
var bBoardNameUpdate = false;
function SetFontSize(change) {
    current_font_size += change;
    /* Update parent window / header font size */
    document.getElementById("page_body").style.fontSize = current_font_size + "%";
    /* Update BoardView iframe font size */
    boardView.contentDocument.body.style.fontSize = current_font_size + "%";

    /* Post updated font size info to update user preference */
    var ajax_query = new XMLHttpRequest();
    ajax_query.open("POST", "/UserPref/AjaxUpdate");
    ajax_query.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    ajax_query.send("FontSize=" + current_font_size);
}
function UpdateBoard(id, name) {
    boardName.innerHTML = name;
    boardView.src = "/BoardView/Index/" + id;
    bBoardNameUpdate = false;
}
document.getElementById("page_body").onload = function () {
    /* Pull current font size from style data */
    var font_size = document.getElementById("page_body").style.fontSize;
    current_font_size = Number(font_size.substr(0, font_size.indexOf("%")));
    if (boardView != null)
        boardView.contentDocument.body.style.fontSize = font_size;
}
if (boardView != null) {
    boardView.onload = function () {
        if (bBoardNameUpdate == true) {
            boardName.innerHTML = boardView.contentDocument.title;
        }
        else {
            bBoardNameUpdate = true;
        }
    }
}
/* A tile Click function to be used with interactive tiles */
function TileOnClick(type, context, tileid) {
    switch (type) {
        case "PlayGif":
            /* Change tile image source to specified resource */
            document.getElementById(tileid).src = context;
            break;
        case "ExternalLink":
            /* Open an external link in a new tab/window */
            window.open(context, "ExternalTileLink");
            break;
        case "SwitchBoard":
            /* Switch to differently identified board */
            window.location.href = "/BoardView/Index/" + context;
            break;
        default:
            break;
    }

    /* Post tile click info to log click */
    var ajax_query = new XMLHttpRequest();
    ajax_query.open("POST", "/BoardView/LogTileClick");
    ajax_query.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    ajax_query.send("TileID=" + tileid);
}
