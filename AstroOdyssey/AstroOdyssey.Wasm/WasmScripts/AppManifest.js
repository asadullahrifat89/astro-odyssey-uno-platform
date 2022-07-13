var UnoAppManifest = {

    splashScreenImage: "Assets/SplashScreen.png",
    splashScreenColor: "transparent",
    displayName: "AstroOdyssey"
}

const myAudios = [];

function playSound(src, volume, loop) {
    var myAudio = new Audio(src);
    myAudio.volume = volume;
    myAudio.loop = loop;
    myAudio.play();

    myAudios.push(myAudio);
}

function stopSounds() {    
    myAudios.every(pauseSound);
}

function pauseSound(audio) {
    audio.pause(); // Stop playing
    audio.currentTime = 0; // Reset time
}

function helloWorld(message) {
    alert(message);
}
