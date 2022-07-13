var UnoAppManifest = {

    splashScreenImage: "Assets/SplashScreen.png",
    splashScreenColor: "transparent",
    displayName: "AstroOdyssey"
}

const myAudios = [];

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

const BACKGROUND_MUSIC = new Audio();
const LASER_FIRE = new Audio();
const LASER_FIRE_POWERED_UP = new Audio();
const LASER_HIT = new Audio();
const POWER_UP = new Audio();
const POWER_DOWN = new Audio();
const ENEMY_DESTRUCTION = new Audio();
const METEOR_DESTRUCTION = new Audio();
const LEVEL_UP = new Audio();
const HEALTH_GAIN = new Audio();
const HEALTH_LOSS = new Audio();

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

            BACKGROUND_MUSIC.src = src;
            BACKGROUND_MUSIC.volume = 0.4;
            BACKGROUND_MUSIC.loop = true;
            playSound(BACKGROUND_MUSIC);
        } break;
        case "LASER_FIRE": {
            LASER_FIRE.src = baseUrl.concat("/", lfSource);
            LASER_FIRE.volume = 0.2;
            playSound(LASER_FIRE);
        } break;
        case "LASER_FIRE_POWERED_UP": {
            LASER_FIRE_POWERED_UP.src = baseUrl.concat("/", lfpuSource);
            LASER_FIRE_POWERED_UP.volume = 1.0;
            playSound(LASER_FIRE_POWERED_UP);
        } break;
        case "LASER_HIT": {
            LASER_HIT.src = baseUrl.concat("/", lhSource);
            LASER_HIT.volume = 0.6;
            playSound(LASER_HIT);
        } break;
        case "POWER_UP": {
            POWER_UP.src = baseUrl.concat("/", puSource);
            POWER_UP.volume = 1.0;
            playSound(POWER_UP);
        } break;
        case "POWER_DOWN": {
            POWER_DOWN.src = baseUrl.concat("/", pdSource);
            POWER_DOWN.volume = 1.0;
            playSound(POWER_DOWN);
        } break;
        case "ENEMY_DESTRUCTION": {
            ENEMY_DESTRUCTION.src = baseUrl.concat("/", edSource);
            ENEMY_DESTRUCTION.volume = 0.8;
            playSound(ENEMY_DESTRUCTION);
        } break;
        case "METEOR_DESTRUCTION": {
            METEOR_DESTRUCTION.src = baseUrl.concat("/", mdSource);
            METEOR_DESTRUCTION.volume = 0.8;
            playSound(METEOR_DESTRUCTION);
        } break;
        case "LEVEL_UP": {
            LEVEL_UP.src = baseUrl.concat("/", luSource);
            LEVEL_UP.volume = 1.0;
            playSound(LEVEL_UP);
        } break;
        case "HEALTH_GAIN": {
            HEALTH_GAIN.src = baseUrl.concat("/", hgSource);
            HEALTH_GAIN.volume = 1.0;
            playSound(HEALTH_GAIN);
        } break;
        case "HEALTH_LOSS": {
            HEALTH_LOSS.src = baseUrl.concat("/", hlSource);
            HEALTH_LOSS.volume = 1.0;
            playSound(HEALTH_LOSS);
        } break;
        default: {

        }
    }
}

function stopSound() {
    pauseSound(BACKGROUND_MUSIC);
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
