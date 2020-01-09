/********************************************************************
 * BoardEditor JavaScript side, written to make the process
 * of adding, editing and removing* Boards/Tiles/Images as
 * seamless to make this the best possible experience.
 * Also practice in AJAX/JSON/JS sorcery
 * (*) Object refrences are being removed, not the objects
 *******************************************************************/
/******************* GLOBALS ***************************************/
/* These variables work like this:
 * DB -> C# Object -> JSON -> JavaScript Object -> JSON -> C# Object -> DB */
var currentTileObject = null;
var currentImageObject = null;
/******************* FUNCTIONS *************************************/
$(document).ready(function () {
    PopulateLists();
});
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
/************** Board Editor functions ******************************/
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
    var edit = document.createElement("img");
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
    var img = document.createElement("img");
    img.className = "dragme";
    img.src = image_src;
    img.alt = image_name;
    img.style.width = "24vw";
    img.style.height = "35vh";
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
/************** Tile Modal functions *******************************/
/* Set current tile object to one retreived from the server identified by tileid */
function GetTileData(tileid) {
    var modal_callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            currentTileObject = JSON.parse(this.responseText);
            currentImageObject = currentTileObject.Source;
            document.getElementById("tilename").value = currentTileObject.TileName;
            var colorStr = ("000000" + currentTileObject.BackgroundColor.toString(16).toUpperCase()).substr(-6);
            document.getElementById("tile_color").value = colorStr;
            document.getElementById("tile_color").style.backgroundColor = "#" + colorStr;
            document.getElementById("tile_text").value = currentTileObject.TileText;
            document.getElementById("tile_preview").style.backgroundColor = "#" + colorStr;
            document.getElementById("tile_text_disp").innerHTML = currentTileObject.TileText;
            if (currentImageObject != null) {
                document.getElementById("tile_image_file").src = currentImageObject.Source;
                document.getElementById("tile_image_file").style.display = "initial";
            }
            if (currentTileObject.Source != null)
                $("#tile_image").val(currentTileObject.Source.ID.toString());
            switch (currentTileObject.ActionType) {
                case 1:
                    $("#tile_gif").val(currentTileObject.ActionContext);
                    $('#actionTab a[href="#gif"]').tab("show");
                    break;
                case 2:
                    $("#tile_url").val(currentTileObject.ActionContext);
                    $('#actionTab a[href="#link"]').tab("show");
                    break;
                case 3:
                    $("#tile_board").val(currentTileObject.ActionContext);
                    $('#actionTab a[href="#anotherboard"]').tab("show");
                    break;
                case 0:
                default:
                    $('#actionTab a[href="#nothing"]').tab("show");
                    break;
            }
            $("#tileModal").modal("show");
        }
    }
    AjaxDispatcher("GET", "/ObjectManager/TileJSON/" + tileid, modal_callback);
}
function RefreshModalPreview(){
    document.getElementById("tile_preview").style.backgroundColor = "#" + $("#tile_color").val();
    document.getElementById("tile_text_disp").innerHTML = $("#tile_text").val();
    var callback = function() {
        if (this.readyState == this.DONE && this.status == 200) {
            currentImageObject = JSON.parse(this.responseText);
            currentTileObject.Source = currentImageObject;
            if (currentImageObject != null) {
                document.getElementById("tile_image_file").src = currentImageObject.Source;
                document.getElementById("tile_image_file").style.display = "initial";
            }
        }
    }
    AjaxDispatcher("GET","/ObjectManager/ImageJSON/" + $("#tile_image").val(), callback);
}
function PickActionType(type) {
    if (currentTileObject != null)
        currentTileObject.ActionType = type;
}
function PopulateLists() {
    var boardlist_callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            if (this.responseText === "null")
                return;
            var boardlist = JSON.parse(this.responseText);
            var boardselect = document.getElementById("tile_board");
            var prompt = document.importNode(boardselect.firstElementChild, true);
            boardselect.innerHTML = "";
            boardselect.appendChild(prompt);
            for (var entry in boardlist) {
                boardselect.innerHTML += '<option value="' + entry + '">' + boardlist[entry] + '</option>';
            }
        }
    }
    var catlist_callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            var catlist = JSON.parse(this.responseText);
            var elements = document.querySelectorAll(".image-cat");

            for (var i = 0; i < elements.length; i++) {
                var prompt = document.importNode(elements[i].firstElementChild, true);
                elements[i].innerHTML = "";
                elements[i].appendChild(prompt);
                for (var entry in catlist) {
                    elements[i].innerHTML += '<option value="' + catlist[entry] + '">' + catlist[entry] + '</option>';
                }
            }
        }
    }
    AjaxDispatcher("GET", "/ObjectManager/GetBoardList", boardlist_callback);
    AjaxDispatcher("GET", "/ObjectManager/GetImageCategories", catlist_callback);
    PickImagesByCategory("");
}
/* Use caching to reduce parsing */
var imagelist = null;
function PickImagesByCategory(category, select_img_id = 0) {
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            if (imagelist == null)
                imagelist = JSON.parse(this.responseText);
        }
        /* Check if the list is cached */
        if (imagelist == null)
            return;
        var elements = document.querySelectorAll(".image-list");

        for (var i = 0; i < elements.length; i++) {
            var prompt = document.importNode(elements[i].firstElementChild, true);
            elements[i].innerHTML = "";
            elements[i].appendChild(prompt);
            /* This bit is kinda slow and janky */
            for (var entry in imagelist) {
                elements[i].innerHTML += '<option value="' + entry + '">' + imagelist[entry] + '</option>';
            }
        }
        if (select_img_id != 0) {
            $("#tile_image").val(select_img_id);
        }
    }
    AjaxDispatcher("GET", "/ObjectManager/GetImageList/" + category, callback);
}
/* Send the current tile object to the server */
function PostTileData() {
    currentTileObject.TileName = document.getElementById("tilename").value;
    currentTileObject.BackgroundColor = parseInt(document.getElementById("tile_color").value, 16)
    currentTileObject.TileText = document.getElementById("tile_text").value;
    switch (currentTileObject.ActionType) {
        case 1:
            if ($("#tile_gif").val() != "0") {
                currentTileObject.ActionContext = $("#tile_gif").val();
                break;
            }
        case 2:
            if ($("#tile_url").val() != "") {
                currentTileObject.ActionContext = $("#tile_url").val();
                break;
            }
        case 3:
            if ($("#tile_board").val() != "0") {
                currentTileObject.ActionContext = $("#tile_board").val();
                break;
            }
        case 0:
        default:
            currentTileObject.ActionType = 0;
            currentTileObject.ActionContext = "";
        break;
    }
    currentTileObject.SourceID = $("#tile_image").val();
    GetImageData(currentTileObject.SourceID.toString());
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            var tile = document.getElementById(currentTileObject.ID.toString());
            if (tile != null)
                RemoveTile(tile.id);
            AddTile(currentTileObject.ID.toString(),
                document.getElementById("tile_color").value,
                currentTileObject.TileText,
                currentImageObject.Source,
                currentImageObject.ImageName);
            /* We are done, close modal */
            $("#tileModal").modal("hide");
        }
    }
    AjaxDispatcher("POST", "/ObjectManager/UpdateTile/", callback, "Model=" + JSON.stringify(currentTileObject));
}
/************* Image Modal functions *******************************/
/* Set current image object to one retreived from the server identified by imageid */
var ImageUpdateType = 1;
var tModalWasOpen = false;
function GetImageData(imageid) {
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            if (currentImageObject == null || currentImageObject.ID != Number(imageid)) {
                currentImageObject = JSON.parse(this.responseText);
                if (imageid === "") {
                    ImageUpdateType = 1;
                    $("#image_name").val(currentImageObject.ImageName);
                    document.getElementsByName("image_id")[0].value = currentImageObject.ID.toString();
                    $("#tileModal").modal("hide");
                    tModalWasOpen = true;
                    $("#imageModal").modal("show");
                }
            }
        }
    }
    AjaxDispatcher("GET", "/ObjectManager/ImageJSON/" + imageid, callback);
}
/* Send the current image object to server */
function PostImageData() {
    if (ImageUpdateType == 1)
        currentImageObject.ImageName = $("#image_name").val();
    else {
        DuplictateImageWithNewName($("#edited_image_name").val(), currentImageObject.ID);
        return;
    }
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            $("#tileModal").modal("hide");
            if (tModalWasOpen == true) {
                imagelist = null;
                PickImagesByCategory("", currentImageObject.ID)
                tModalWasOpen = false;
                $("#imageModal").modal("hide");
                document.getElementById("img_preview").src = "";
                document.getElementById("img_preview").style.display = "none";
                document.getElementById("tile_image_file").src = currentImageObject.Source;
                document.getElementById("tile_image_file").style.display = "initial";
                currentTileObject.Source = currentImageObject;
                $('#sourceTab a[href="#current-image"]').tab("show");
                $("#tileModal").modal("show");
            }
        }
    }
    AjaxDispatcher("POST", "/ObjectManager/UpdateImage/", callback, "Model=" + JSON.stringify(currentImageObject));
}
function DuplictateImageWithNewName(name, newid) {
    $("#image_name").val(name);
    var callback = function () {
        if (this.readyState == this.DONE && this.status == 200) {
            imageObject = JSON.parse(this.responseText);

            if (imageObject != null) {
                currentImageObject.Source = imageObject.Source;
                currentImageObject.ID = newid;
                ImageUpdateType = 1;
                PostImageData();
            }
        }
    }
    AjaxDispatcher("GET", "/ObjectManager/ImageJSON/" + $("#image_library").val(), callback);
}
function ImageUpload() { 
    var message = new XMLHttpRequest();
    message.open("POST", "/ObjectManager/FileUpload");
    message.onreadystatechange = function () {
        if (this.status == 200 && this.readyState == this.DONE) {
            currentImageObject.Source = this.responseText;
            document.getElementById("img_preview").src = this.responseText;
            document.getElementById("img_preview").style.display = "initial";
        }
    }
    message.send(new FormData(document.getElementById("image_form")));
}
