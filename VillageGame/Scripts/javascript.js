let wells = document.getElementById("wells").innerText;
let houses = document.getElementById("houses").innerText;
let castles = document.getElementById("castles").innerText;
let farms = document.getElementById("farms").innerText;

let wood = document.getElementById("wood").innerText;
let houseError = document.getElementById("houseError");
let wellError = document.getElementById("wellError");
let farmError = document.getElementById("farmError");
let castleError = document.getElementById("castleError");


let houseButton = document.getElementById("houseSubmit");
let wellButton = document.getElementById("wellSubmit");
let farmButton = document.getElementById("farmSubmit");
let castleButton = document.getElementById("castleSubmit");

houseButton.addEventListener('click', function (event) {
    if (wood < 5) {
        event.preventDefault();
        houseError.innerText = "Not enough wood, need 5";
        setTimeout(function() {
            houseError.innerText = "";
        }, 3000)
    }
});

wellButton.addEventListener('click', function (event) {
    if (wood < 6) {
        event.preventDefault();
        wellError.innerText = "Not enough wood, need 6";
        setTimeout(function () {
            wellError.innerText = "";
        }, 3000)
    }
})

farmButton.addEventListener('click', function (event) {
    if (wood < 8) {
        event.preventDefault();
        farmError.innerText = "Not enough wood, need 8.";
        setTimeout(function () {
            farmError.innerText = "";
        }, 3000)
    }
})

castleButton.addEventListener('click', function (event) {
    if (wood < 10 || stone < 10) {
        event.preventDefault();
    }
    else {
        return;
    }
    if (wood < 10 && stone < 10) {
        castleError.innerText = "Not enough wood or stone, need 10 each";
    }
    else if (wood < 10) {
        castleError.innerText = "Not enough wood, need 10";
    }
    else if (stone < 10) {
        castleError.innerText = "not enough stone, need 10";
    }
    setTimeout(function () {
        farmError.innerText = "";
    }, 3000)
})