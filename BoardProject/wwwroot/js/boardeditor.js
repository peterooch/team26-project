/************************************************************
 * BoardEditor JavaScript side, written to make the process
 * of adding, editing and removing* Boards/Tiles/Images as
 * seamless to make this the best possible experience.
 * Also practice in AJAX/JSON/JS sorcery
 * (*) Object refrences are being removed, not the objects
 ************************************************************/
/******************* GLOBALS ********************************/
/* These variables work like this:
 * DB -> C# Object -> JSON -> JavaScript Object -> JSON -> C# Object -> DB */
var currentTileObject = null;
var currentImageObject = null;
/******************* FUNCTIONS ******************************/
/* Multipurpose AJAX message dispatcher */
function AjaxDispatcher(method, target, callback_fn, payload) {
    var message = new XMLHttpRequest();
    message.onreadystatechange = callback_fn;
    message.open(method, target);
    if (method === "POST") {
        message.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        message.send(payload);
    }
    else {
        message.send();
    }
}
/* Add a brand new tile into the board editor tile container,
   new tile behavior is identical to currently showed tiles */
function AddTile(tileid, bg_color, tile_text, image_src, image_name) {
    var tile_container = document.querySelector(".tile_container");
    var outerdiv = document.createElement("div");
    outerdiv.style.padding = padding;
    outerdiv.className = "tile_outer_container";
    var innerdiv = document.createElement("div");
    innerdiv.className = "tiles";
    innerdiv.id = tileid;
    innerdiv.style.backgroundColor = "#" + bg_color;
    var center = document.createElement("center");
    var image_text = document.createTextNode(tile_text);
    center.appendChild(image_text);
    var span = document.createElement("span");
    span.style.backgroundColor = "white";
    var edit = document.createElement("div");
    edit.src = "/images/edit.png";
    edit.onclick = function (e) {
        GetTileData(e.target.parentElement.parentElement.parentElement.id);
    };
    span.appendChild(edit);
    var del = document.createElement("img");
    del.src = "/images/delete.png";
    del.onclick = function (e) {
        RemoveTile(e.target.parentElement.parentElement.parentElement.id);
    };
    span.appendChild(del);
    center.appendChild(span);
    innerdiv.appendChild(center);
    innerdiv.appendChild(document.createElement("br"));
    var img = document.createElement("img");
    img.src = image_src;
    img.alt = image_name;
    img.width = "24vw";
    img.height = "35vh";
    innerdiv.appendChild(img);
    outerdiv.appendChild(innerdiv);
    tile_container.appendChild(outerdiv);
}
/* Remove a tile from the board editor tile container */
function RemoveTile(tileid) {
    document.getElementById(tileid).parentElement.remove();
}
/* Serialize the data into BoardData JSON format and send it
   to the server for Deserialization */
function SaveBoard() {
    var board = {
        ID: document.getElementById("board_id").value,
        BoardName: document.getElementById("board_name").value,
        IsPublic: false,
        BoardHeader: document.getElementById("board_header").value,
        BackgroundColor: parseInt(document.getElementById("bg_color").value, 16),
        TextColor: parseInt(document.getElementById("tx_color").value, 16),
        FontSize: Number(document.getElementById("font_size").value),
        Spacing: Number(document.getElementById("spacing").value),
        TileIDs: ""
    };
    var tiles = document.querySelectorAll(".tiles");
    tiles.forEach(element => {
        board.TileIDs += element.id + ";";
    });
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200)
            window.location.href = "/Home/";
    }
    AjaxDispatcher("POST", "/ObjectManager/UpdateBoard/", callback, "Model=" + JSON.stringify(board));
}
/* Remove the currently selected board from the current user's list of boards */
function RemoveBoard() {
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200)
            window.location.href = "/Home/";
    }
    AjaxDispatcher("POST", "/ObjectManager/RemoveBoard/", callback, "ID=" + document.getElementById("board_id").value);
}
function RefreshEditor() {
    var container = document.querySelector(".tile_container");
    container.style.backgroundColor = "#" + document.getElementById("bg_color").value;
    container.style.color = "#" + document.getElementById("tx_color").value;
    container.style.fontSize = document.getElementById("font_size").value + "%";
    padding = document.getElementById("spacing").value + "%";
    document.querySelectorAll(".tile_outer_container").forEach(tile =>
        tile.style.padding = padding
    );
}
/* Set current tile object to one retreived from the server identified by tileid */
function GetTileData(tileid) {
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200){
            if (currentTileObject == null || currentTileObject.ID != Number(tileid)) {
                currentTileObject = JSON.parse(this.responseText);
                currentImageObject = currentImageObject.Source;
                /* Update Tile Modal info */
            }
            $("#tileModal").modal("show");
        }
    }
    AjaxDispatcher("GET", "/ObjectManager/TileJSON/" + tileid, callback);
}
/* Set current image object to one retreived from the server identified by imageid */
function GetImageData(imageid) {
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            if (currentImageObject == null || currentImageObject.ID != Number(imageid)) {
                currentImageObject = JSON.parse(this.responseText);
                /* Update Image Modal info */
            }
        }
    }
    AjaxDispatcher("GET", "/ObjectManager/ImageJSON/" + imageid, callback);
}
/* Send the current tile object to the server */
function PostTileData() {
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {

        }
    }
    AjaxDispatcher("POST", "/ObjectManager/UpdateTile/", callback, "Model=" + JSON.stringify(currentTileObject));
}
/* Send the current image object to server */
function PostImageData() {
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            /* Close Image Modal */
        }
    }
    AjaxDispatcher("POST", "/ObjectManager/UpdateImage/", callback, "Model=" + JSON.stringify(currentImageObject));
}
function ImageUpload() {
    var callback = function()
    {
        if (this.status == 200 && this.readyState == this.DONE) {
            /* responseText contains address for uploaded file */
        }
    }
    AjaxDispatcher("POST", "/ObjectManager/FileUpload", callback, new FormData(document.getElementById("image_form")));
}
