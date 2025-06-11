let preloader = document.getElementById("preloader");
let menuItems = document.getElementById("menu-items");




//Toggling menubar

menuItems.style.maxHeight = "0px";

function menuToggle() {
    if (menuItems.style.maxHeight == "0px") {
        menuItems.style.maxHeight = "200px";
    } else {
        menuItems.style.maxHeight = "0px";
    }
}



window.addEventListener("load", function () {

    //Preloader
    setTimeout(function () {
        preloader.style.opacity = "0";
        preloader.style.transition = "opacity 1s ease-in-out";

        setTimeout(function () {
            preloader.style.display = "none";
        }, 500);
    }, 1500);
});





