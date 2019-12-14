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
}
