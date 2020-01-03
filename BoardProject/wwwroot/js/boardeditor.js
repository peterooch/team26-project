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
function AjaxDispatcher(method, target, callback, payload) {
    var message = new XMLHttpRequest();
    message.onreadystatechange = callback;
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
    var outerdiv = document.createElement("DIV");
    outerdiv.style.padding = padding;
    var innerdiv = document.createElement("DIV");
    innerdiv.className = "tiles";
    innerdiv.id = tileid;
    innerdiv.style.backgroundColor = "#" + bg_color;
    var image_text = document.createTextNode(tile_text);
    innerdiv.appendChild(image_text);
    var edit = document.createElement("IMG");
    edit.src = "/images/edit.png";
    edit.onclick = function (e) {
        EditTile(e.target.parentElement.id);
    };
    innerdiv.appendChild(edit);
    var del = document.createElement("IMG");
    del.src = "/images/delete.png";
    del.onclick = function (e) {
        RemoveTile(e.target.parentElement.id);
    };
    innerdiv.appendChild(del);
    innerdiv.appendChild(document.createElement("BR"));
    var img = document.createElement("IMG");
    img.src = image_src;
    img.alt = image_name;
    img.style.width = "15vw";
    img.style.height = "26.25vh";
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
/* Set current tile object to one retreived from the server identified by tileid */
function GetTileData(tileid) {

    var callback = function () {
        if (this.readyState != this.DONE || this.status != 200)
            return;

        if (currentTileObject == null || currentTileObject.ID != Number(imageid)) {
            currentTileObject = JSON.parse(this.responseText);
            /* Update Tile Modal info */
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
            /* Close Tile Modal */
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
