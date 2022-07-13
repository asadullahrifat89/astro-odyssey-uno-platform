var UnoAppManifest = {

    splashScreenImage: "Assets/SplashScreen.png",
    splashScreenColor: "transparent",
    displayName: "AstroOdyssey"

}

function playSound(src, volume) {
    var audio = new Audio(src);
    audio.volume = volume;
    audio.play();
}

function helloWorld(src) {
    alert(src);
}
