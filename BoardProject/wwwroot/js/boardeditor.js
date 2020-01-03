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
    };;
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
function RemoveTile(tileid) {
    document.getElementById(tileid).parentElement.remove();
}
