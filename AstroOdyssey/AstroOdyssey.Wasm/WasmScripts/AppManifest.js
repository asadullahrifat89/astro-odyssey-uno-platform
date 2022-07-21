var UnoAppManifest = {

    splashScreenImage: "Assets/SplashScreen.png",
    splashScreenColor: "transparent",
    displayName: "AstroOdyssey"
}

const roundsFireSource = "Assets/Sounds/laser-blast-descend_Gy7C5dEO_NWM.mp3";
const rapidShotRoundsFireSource = "Assets/Sounds/alien-computer-program-deactivate_GkreEFV__NWM.mp3";
const deadShotRoundsFireSource = "Assets/Sounds/magnetic-destroy-shot_fkxD6SV__NWM.mp3";
const sonicShotRoundsFireSource = "Assets/Sounds/punchy-laser-shot_f11BarNO_NWM.mp3";

const enemyRoundsFireSource = "Assets/Sounds/laser-descend_GJPs9OE__NWM.mp3";

const roundHitSource = "Assets/Sounds/explosion-sfx-43814.mp3";

const powerUpSource = "Assets/Sounds/spellcast-46164.mp3";
const powerDownSource = "Assets/Sounds/power-down-7103.mp3";

const enemyDestroyedSource = "Assets/Sounds/magical-impact-small-fast-projectile_z1rrOFEd_NWM.mp3";
const meteorDestroyedSource = "Assets/Sounds/magical-impact-small-fast-projectile_z1rrOFEd_NWM.mp3";

const levelUpSource = "Assets/Sounds/8-bit-powerup-6768.mp3";

const healthGainSource = "Assets/Sounds/scale-e6-14577.mp3";
const healthLossSource = "Assets/Sounds/rocket-missile-launcher_MyHKjH4__NWM.mp3";

const gameStartSource = "Assets/Sounds/space-jet-flyby_MkgS2BVu_NWM.mp3";
const gameOverSource = "Assets/Sounds/videogame-death-sound-43894.mp3";

const bossAppearanceSource = "Assets/Sounds/dark-sitar-7546.mp3";

const bgAudio = new Audio();
const rfAudio = new Audio();
const erfAudio = new Audio();

const rapidsrfAudio = new Audio();
const deadsrfAudio = new Audio();
const sonicsrfAudio = new Audio();

const rhAudio = new Audio();
const puAudio = new Audio();
const pdAudio = new Audio();
const edAudio = new Audio();
const mdAudio = new Audio();
const luAudio = new Audio();
const hgAudio = new Audio();
const hlAudio = new Audio();

const gsAudio = new Audio();
const goAudio = new Audio();

const baAudio = new Audio();

function playGameSound(baseUrl, soundType) {
    switch (soundType) {
        case "GAME_START": {
            if (gsAudio.src.length == 0) {
                gsAudio.src = baseUrl.concat("/", gameStartSource);
                gsAudio.volume = 1.0;
                setAudioAttributes(gsAudio);
            }
            playSound(gsAudio);
        } break;
        case "PLAYER_ROUNDS_FIRE": {
            if (rfAudio.src.length == 0) {
                rfAudio.src = baseUrl.concat("/", roundsFireSource);
                rfAudio.volume = 0.1;
                setAudioAttributes(rfAudio);
            }
            playSound(rfAudio);
        } break;
        case "PLAYER_RAPIDSHOT_ROUNDS_FIRE": {
            if (rapidsrfAudio.src.length == 0) {
                rapidsrfAudio.src = baseUrl.concat("/", rapidShotRoundsFireSource);
                rapidsrfAudio.volume = 0.2;
                setAudioAttributes(rapidsrfAudio);
            }
            playSound(rapidsrfAudio);
        } break;
        case "PLAYER_DEADSHOT_ROUNDS_FIRE": {
            if (deadsrfAudio.src.length == 0) {
                deadsrfAudio.src = baseUrl.concat("/", deadShotRoundsFireSource);
                deadsrfAudio.volume = 0.2;
                setAudioAttributes(deadsrfAudio);
            }
            playSound(deadsrfAudio);
        } break;
        case "PLAYER_SONICSHOT_ROUNDS_FIRE": {
            if (sonicsrfAudio.src.length == 0) {
                sonicsrfAudio.src = baseUrl.concat("/", sonicShotRoundsFireSource);
                sonicsrfAudio.volume = 0.2;
                setAudioAttributes(sonicsrfAudio);
            }
            playSound(sonicsrfAudio);
        } break;
        case "ENEMY_ROUNDS_FIRE": {
            if (erfAudio.src.length == 0) {
                erfAudio.src = baseUrl.concat("/", enemyRoundsFireSource);
                erfAudio.volume = 0.1;
                setAudioAttributes(erfAudio);
            }
            playSound(erfAudio);
        } break;
        case "ROUNDS_HIT": {
            if (rhAudio.src.length == 0) {
                rhAudio.src = baseUrl.concat("/", roundHitSource);
                rhAudio.volume = 0.6;
                setAudioAttributes(rhAudio);
            }
            playSound(rhAudio);
        } break;
        case "POWER_UP": {
            if (puAudio.src.length == 0) {
                puAudio.src = baseUrl.concat("/", powerUpSource);
                puAudio.volume = 1.0;
                setAudioAttributes(puAudio);
            }
            playSound(puAudio);
        } break;
        case "POWER_DOWN": {
            if (pdAudio.src.length == 0) {
                pdAudio.src = baseUrl.concat("/", powerDownSource);
                pdAudio.volume = 1.0;
                setAudioAttributes(pdAudio);
            }
            playSound(pdAudio);
        } break;
        case "ENEMY_DESTRUCTION": {
            if (edAudio.src.length == 0) {
                edAudio.src = baseUrl.concat("/", enemyDestroyedSource);
                edAudio.volume = 0.4;
                setAudioAttributes(edAudio);
            }
            playSound(edAudio);
        } break;
        case "METEOR_DESTRUCTION": {
            if (mdAudio.src.length == 0) {
                mdAudio.src = baseUrl.concat("/", meteorDestroyedSource);
                mdAudio.volume = 0.3;
                setAudioAttributes(mdAudio);
            }
            playSound(mdAudio);
        } break;
        case "LEVEL_UP": {
            if (luAudio.src.length == 0) {
                luAudio.src = baseUrl.concat("/", levelUpSource);
                luAudio.volume = 0.6;
                setAudioAttributes(luAudio);
            }
            playSound(luAudio);
        } break;
        case "HEALTH_GAIN": {
            if (hgAudio.src.length == 0) {
                hgAudio.src = baseUrl.concat("/", healthGainSource);
                hgAudio.volume = 1.0;
                setAudioAttributes(hgAudio);
            }
            playSound(hgAudio);
        } break;
        case "HEALTH_LOSS": {
            if (hlAudio.src.length == 0) {
                hlAudio.src = baseUrl.concat("/", healthLossSource);
                hlAudio.volume = 1.0;
                setAudioAttributes(hlAudio);
            }
            playSound(hlAudio);
        } break;
        case "GAME_OVER": {
            if (goAudio.src.length == 0) {
                goAudio.src = baseUrl.concat("/", gameOverSource);
                goAudio.volume = 1.0;
                setAudioAttributes(goAudio);
            }
            playSound(goAudio);
        } break;
        case "BOSS_APPEARANCE": {
            if (baAudio.src.length == 0) {
                baAudio.src = baseUrl.concat("/", bossAppearanceSource);
                baAudio.volume = 1.0;
                setAudioAttributes(baAudio);
            }
            playSound(baAudio);
        } break;
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
            setAudioAttributes(bgAudio);

            bgAudio.onended = function () {
                playGameSound(baseUrl, soundType);
            };
            
            playSound(bgAudio);
        } break;
        default: {

        }
    }
}

function setAudioAttributes(audio) {
    audio.style.display = "none";
    audio.setAttribute("controls", "none");
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
