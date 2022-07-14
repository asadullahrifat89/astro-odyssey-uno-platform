var UnoAppManifest = {

    splashScreenImage: "Assets/SplashScreen.png",
    splashScreenColor: "transparent",
    displayName: "AstroOdyssey"
}

const lfSource = "Assets/Sounds/shoot02wav-14562.mp3";
const lfpuSource = "Assets/Sounds/plasmablaster-37114.mp3";
const lhSource = "Assets/Sounds/explosion-sfx-43814.mp3";
const puSource = "Assets/Sounds/spellcast-46164.mp3";
const pdSource = "Assets/Sounds/power-down-7103.mp3";
const edSource = "Assets/Sounds/explosion-36210.mp3";
const mdSource = "Assets/Sounds/explosion-36210.mp3";
const luSource = "Assets/Sounds/8-bit-powerup-6768.mp3";
const hgSource = "Assets/Sounds/scale-e6-14577.mp3";
const hlSource = "Assets/Sounds/explosion-39897.mp3";

const bgAudio = new Audio();
const lfAudio = new Audio();
const lfpuAudio = new Audio();
const lhAudio = new Audio();
const puAudio = new Audio();
const pdAudio = new Audio();
const edAudio = new Audio();
const mdAudio = new Audio();
const luAudio = new Audio();
const hgAudio = new Audio();
const hlAudio = new Audio();

function playGameSound(baseUrl, soundType) {
    switch (soundType) {
        case "BACKGROUND_MUSIC": {

            let musicTrack = Math.floor((Math.random() * 12));
            let src = "";

            switch (musicTrack) {
                case 1: { src = baseUrl.concat("/", "Assets/Sounds/slow-trap-18565.mp3"); } break;
                case 2: { src = baseUrl.concat("/", "Assets/Sounds/space-chillout-14194.mp3"); } break;
                case 3: { src = baseUrl.concat("/", "Assets/Sounds/cinematic-space-drone-10623.mp3"); } break;
                case 4: { src = baseUrl.concat("/", "Assets/Sounds/slow-thoughtful-sad-piano-114586.mp3"); } break;
                case 5: { src = baseUrl.concat("/", "Assets/Sounds/space-age-10714.mp3"); } break;
                case 6: { src = baseUrl.concat("/", "Assets/Sounds/drone-space-main-9706.mp3"); } break;
                case 7: { src = baseUrl.concat("/", "Assets/Sounds/cyberpunk-2099-10701.mp3"); } break;
                case 8: { src = baseUrl.concat("/", "Assets/Sounds/insurrection-10941.mp3"); } break;
                case 9: { src = baseUrl.concat("/", "Assets/Sounds/space-trip-114102.mp3"); } break;
                case 10: { src = baseUrl.concat("/", "Assets/Sounds/dark-matter-10710.mp3"); } break;
                case 11: { src = baseUrl.concat("/", "Assets/Sounds/music-807dfe09ce23793891674eb022b38c1b.mp3"); } break;
                default:
            }

            bgAudio.src = src;
            bgAudio.volume = 0.3;
            bgAudio.loop = true;
            playSound(bgAudio);
        } break;
        case "LASER_FIRE": {
            if (lfAudio.src.length == 0)
                lfAudio.src = baseUrl.concat("/", lfSource);
            lfAudio.volume = 0.2;
            playSound(lfAudio);
        } break;
        case "LASER_FIRE_POWERED_UP": {
            if (lfpuAudio.src.length == 0)
                lfpuAudio.src = baseUrl.concat("/", lfpuSource);
            lfpuAudio.volume = 1.0;
            playSound(lfpuAudio);
        } break;
        case "LASER_HIT": {
            if (lhAudio.src.length == 0)
                lhAudio.src = baseUrl.concat("/", lhSource);
            lhAudio.volume = 0.6;
            playSound(lhAudio);
        } break;
        case "POWER_UP": {
            if (puAudio.src.length == 0)
                puAudio.src = baseUrl.concat("/", puSource);
            puAudio.volume = 1.0;
            playSound(puAudio);
        } break;
        case "POWER_DOWN": {
            if (pdAudio.src.length == 0)
                pdAudio.src = baseUrl.concat("/", pdSource);
            pdAudio.volume = 1.0;
            playSound(pdAudio);
        } break;
        case "ENEMY_DESTRUCTION": {
            if (edAudio.src.length == 0)
                edAudio.src = baseUrl.concat("/", edSource);
            edAudio.volume = 0.8;
            playSound(edAudio);
        } break;
        case "METEOR_DESTRUCTION": {
            if (mdAudio.src.length == 0)
                mdAudio.src = baseUrl.concat("/", mdSource);
            mdAudio.volume = 0.8;
            playSound(mdAudio);
        } break;
        case "LEVEL_UP": {
            if (luAudio.src.length == 0)
                luAudio.src = baseUrl.concat("/", luSource);
            luAudio.volume = 1.0;
            playSound(luAudio);
        } break;
        case "HEALTH_GAIN": {
            if (hgAudio.src.length == 0)
                hgAudio.src = baseUrl.concat("/", hgSource);
            hgAudio.volume = 1.0;
            playSound(hgAudio);
        } break;
        case "HEALTH_LOSS": {
            if (hlAudio.src.length == 0)
                hlAudio.src = baseUrl.concat("/", hlSource);
            hlAudio.volume = 1.0;
            playSound(hlAudio);
        } break;
        default: {

        }
    }
}

function stopSound() {
    pauseSound(bgAudio);
}

function playSound(audio) {
    audio.currentTime = 0; // Reset time
    audio.play();
}

function pauseSound(audio) {
    audio.pause(); // Stop playing
    audio.currentTime = 0; // Reset time
}

function helloWorld(message) {
    alert(message);
}
