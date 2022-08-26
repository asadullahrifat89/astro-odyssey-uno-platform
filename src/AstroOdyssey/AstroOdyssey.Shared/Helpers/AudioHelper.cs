using System;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public static class AudioHelper
    {
        #region Fields        

        private static Random random = new Random();

        private static AudioPlayer GAME_INTRO = null;
        private static AudioPlayer MENU_SELECT = null;
        private static AudioPlayer BACKGROUND_MUSIC = null;
        private static AudioPlayer PLAYER_ROUNDS_FIRE = null;
        private static AudioPlayer PLAYER_RAPID_SHOT_ROUNDS_FIRE = null;
        private static AudioPlayer PLAYER_DEAD_SHOT_ROUNDS_FIRE = null;
        private static AudioPlayer PLAYER_DOOM_SHOT_ROUNDS_FIRE = null;
        private static AudioPlayer ENEMY_ROUNDS_FIRE = null;
        private static AudioPlayer ENEMY_DESTRUCTION = null;
        private static AudioPlayer METEOR_DESTRUCTION = null;
        private static AudioPlayer BOSS_APPEARANCE = null;
        private static AudioPlayer BOSS_DESTRUCTION = null;
        private static AudioPlayer ROUNDS_HIT = null;
        private static AudioPlayer POWER_UP = null;
        private static AudioPlayer POWER_DOWN = null;
        private static AudioPlayer LEVEL_UP = null;
        private static AudioPlayer HEALTH_GAIN = null;
        private static AudioPlayer HEALTH_LOSS = null;
        private static AudioPlayer GAME_START = null;
        private static AudioPlayer GAME_OVER = null;

        #endregion     

        #region Methods

        public static void PlaySound(SoundType soundType)
        {
            var baseUrl = App.GetBaseUrl();

            switch (soundType)
            {
                case SoundType.GAME_INTRO:
                    {
                        var musicTrack = random.Next(1, 3);
                        var src = "";

                        var musicPath = "Assets/Sounds/Intro/";

                        switch (musicTrack)
                        {
                            case 1: { src = musicPath + "fashion-hip-hop-rock-stylish-boy-111449.mp3"; } break;
                            case 2: { src = musicPath + "inspiring-motivational-rock-inspire-mesenses-111448.mp3"; } break;
                            default:
                                break;
                        }

                        var source = string.Concat(baseUrl, "/", src);

                        if (GAME_INTRO is null)
                        {
                            GAME_INTRO = new AudioPlayer(
                                source: source,
                                volume: 0.5,
                                loop: true);
                        }
                        else
                        {
                            GAME_INTRO.SetSource(source);
                        }

                        GAME_INTRO.Play();
                    }
                    break;
                case SoundType.MENU_SELECT:
                    {
                        if (MENU_SELECT is null)
                        {
                            MENU_SELECT = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/game-start-6104.mp3"),
                                volume: 1);
                        }

                        MENU_SELECT.Play();
                    }
                    break;
                case SoundType.BOSS_APPEARANCE:
                    {
                        var musicTrack = random.Next(1, 5);
                        var src = "";

                        var musicPath = "Assets/Sounds/Boss/";

                        switch (musicTrack)
                        {
                            case 1: { src = musicPath + "despair-metal-trailer-109943.mp3"; } break;
                            case 2: { src = musicPath + "frantic-15190.mp3"; } break;
                            case 3: { src = musicPath + "metal-dark-matter-111451.mp3"; } break;
                            case 4: { src = musicPath + "rage-15292.mp3"; } break;
                            default:
                                break;
                        }

                        var source = string.Concat(baseUrl, "/", src);

                        if (BOSS_APPEARANCE is null)
                        {
                            BOSS_APPEARANCE = new AudioPlayer(
                                source: source,
                                volume: 0.5,
                                loop: true);
                        }
                        else
                        {
                            BOSS_APPEARANCE.SetSource(source);
                        }

                        BOSS_APPEARANCE.Play();
                    }
                    break;
                case SoundType.BACKGROUND_MUSIC:
                    {
                        var musicTrack = random.Next(1, 10);
                        var src = "";

                        var musicPath = "Assets/Sounds/Music/";

                        switch (musicTrack)
                        {
                            case 1: { src = musicPath + "action-stylish-rock-dedication-15038.mp3"; } break;
                            case 2: { src = musicPath + "electronic-rock-king-around-here-15045.mp3"; } break;                            
                            case 3: { src = musicPath + "powerful-stylish-stomp-rock-lets-go-114255.mp3"; } break;
                            case 4: { src = musicPath + "rockstar-trailer-109945.mp3"; } break;                            
                            case 5: { src = musicPath + "stomping-rock-four-shots-111444.mp3"; } break;
                            case 6: { src = musicPath + "stylish-rock-beat-trailer-116346.mp3"; } break;
                            case 7: { src = musicPath + "modern-fashion-promo-rock-18397.mp3"; } break;
                            case 8: { src = musicPath + "hard-rock-21056.mp3"; } break;
                            case 9: { src = musicPath + "crag-hard-rock-14401.mp3"; } break;
                            default:
                                break;
                        }

                        var source = string.Concat(baseUrl, "/", src);

                        if (BACKGROUND_MUSIC is null)
                        {
                            BACKGROUND_MUSIC = new AudioPlayer(
                                source: source,
                                volume: 0.4,
                                loop: true);

                            //BACKGROUND_MUSIC.OnCompleted += (s, e) =>
                            //{
                            //    PlaySound(SoundType.BACKGROUND_MUSIC);
                            //};
                        }
                        else
                        {
                            BACKGROUND_MUSIC.SetSource(source);
                        }

                        BACKGROUND_MUSIC.Play();
                    }
                    break;
                case SoundType.PLAYER_ROUNDS_FIRE:
                    {
                        if (PLAYER_ROUNDS_FIRE is null)
                        {
                            PLAYER_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/laser-blast-descend_Gy7C5dEO_NWM.mp3"),
                                volume: 0.1);
                        }

                        PLAYER_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_RAPID_SHOT_ROUNDS_FIRE:
                    {
                        if (PLAYER_RAPID_SHOT_ROUNDS_FIRE is null)
                        {
                            PLAYER_RAPID_SHOT_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/alien-computer-program-deactivate_GkreEFV__NWM.mp3"),
                                volume: 0.2);
                        }

                        PLAYER_RAPID_SHOT_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_DEAD_SHOT_ROUNDS_FIRE:
                    {
                        if (PLAYER_DEAD_SHOT_ROUNDS_FIRE is null)
                        {
                            PLAYER_DEAD_SHOT_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/magnetic-destroy-shot_fkxD6SV__NWM.mp3"),
                                volume: 0.2);
                        }

                        PLAYER_DEAD_SHOT_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_DOOM_SHOT_ROUNDS_FIRE:
                    {
                        if (PLAYER_DOOM_SHOT_ROUNDS_FIRE is null)
                        {
                            PLAYER_DOOM_SHOT_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/punchy-laser-shot_f11BarNO_NWM.mp3"),
                                volume: 0.2);
                        }

                        PLAYER_DOOM_SHOT_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.ENEMY_ROUNDS_FIRE:
                    {
                        if (ENEMY_ROUNDS_FIRE is null)
                        {
                            ENEMY_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/laser-descend_GJPs9OE__NWM.mp3"),
                                volume: 0.1);
                        }

                        ENEMY_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.ENEMY_DESTRUCTION:
                    {
                        if (ENEMY_DESTRUCTION is null)
                        {
                            ENEMY_DESTRUCTION = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/magical-impact-small-fast-projectile_z1rrOFEd_NWM.mp3"),
                                volume: 0.4);
                        }

                        ENEMY_DESTRUCTION.Play();
                    }
                    break;
                case SoundType.METEOR_DESTRUCTION:
                    {
                        if (METEOR_DESTRUCTION is null)
                        {
                            METEOR_DESTRUCTION = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/magical-impact-small-fast-projectile_z1rrOFEd_NWM.mp3"),
                                volume: 0.3);
                        }

                        METEOR_DESTRUCTION.Play();
                    }
                    break;
                case SoundType.BOSS_DESTRUCTION:
                    {
                        if (BOSS_DESTRUCTION is null)
                        {
                            BOSS_DESTRUCTION = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/halloween-impact-05-93808.mp3"),
                                volume: 1.0);
                        }

                        BOSS_DESTRUCTION.Play();
                    }
                    break;
                case SoundType.ROUNDS_HIT:
                    {
                        if (ROUNDS_HIT is null)
                        {
                            ROUNDS_HIT = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/explosion-firework-boom-single_GkGH0sE__NWM.mp3"),
                                volume: 0.3);
                        }

                        ROUNDS_HIT.Play();
                    }
                    break;
                case SoundType.POWER_UP:
                    {
                        if (POWER_UP is null)
                        {
                            POWER_UP = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/spellcast-46164.mp3"),
                                volume: 1.0);
                        }

                        POWER_UP.Play();
                    }
                    break;
                case SoundType.POWER_DOWN:
                    {
                        if (POWER_DOWN is null)
                        {
                            POWER_DOWN = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/power-down-7103.mp3"),
                                volume: 1.0);
                        }

                        POWER_DOWN.Play();
                    }
                    break;
                case SoundType.METEOR_INCOMING:
                    {
                        if (LEVEL_UP is null)
                        {
                            LEVEL_UP = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/asteroid-incoming-effect.mp3"),
                                volume: 0.8);
                        }

                        LEVEL_UP.Play();
                    }
                    break;
                case SoundType.HEALTH_GAIN:
                    {
                        if (HEALTH_GAIN is null)
                        {
                            HEALTH_GAIN = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/scale-e6-14577.mp3"),
                                volume: 1.0);
                        }

                        HEALTH_GAIN.Play();
                    }
                    break;
                case SoundType.HEALTH_LOSS:
                    {
                        if (HEALTH_LOSS is null)
                        {
                            HEALTH_LOSS = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/rocket-missile-launcher_MyHKjH4__NWM.mp3"),
                                volume: 1.0);
                        }

                        HEALTH_LOSS.Play();
                    }
                    break;
                case SoundType.GAME_START:
                    {
                        if (GAME_START is null)
                        {
                            GAME_START = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/space-jet-flyby_MkgS2BVu_NWM.mp3"),
                                volume: 1.0);
                        }

                        GAME_START.Play();
                    }
                    break;
                case SoundType.GAME_OVER:
                    {
                        if (GAME_OVER is null)
                        {
                            GAME_OVER = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/videogame-death-sound-43894.mp3"),
                                volume: 1.0);
                        }

                        GAME_OVER.Play();
                    }
                    break;
                default:
                    {
                        // App.PlaySound(baseUrl, soundType);
                    }
                    break;
            }
        }

        private static void BACKGROUND_MUSIC_onended(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void StopSound()
        {
            if (GAME_INTRO is not null)
            {
                GAME_INTRO.Stop();
            }

            if (BACKGROUND_MUSIC is not null)
            {
                BACKGROUND_MUSIC.Stop();
            }

            if (BOSS_APPEARANCE is not null)
            {
                BOSS_APPEARANCE.Stop();
            }
        }

        public static void PauseSound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.MENU_SELECT:
                    break;
                case SoundType.BACKGROUND_MUSIC:
                    {
                        if (BACKGROUND_MUSIC is not null)
                        {
                            BACKGROUND_MUSIC.Pause();
                        }
                    }
                    break;
                case SoundType.PLAYER_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_RAPID_SHOT_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_DEAD_SHOT_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_DOOM_SHOT_ROUNDS_FIRE:
                    break;
                case SoundType.ENEMY_ROUNDS_FIRE:
                    break;
                case SoundType.ENEMY_DESTRUCTION:
                    break;
                case SoundType.METEOR_DESTRUCTION:
                    break;
                case SoundType.BOSS_APPEARANCE:
                    {
                        if (BOSS_APPEARANCE is not null)
                        {
                            BOSS_APPEARANCE.Pause();
                        }
                    }
                    break;
                case SoundType.BOSS_DESTRUCTION:
                    break;
                case SoundType.ROUNDS_HIT:
                    break;
                case SoundType.POWER_UP:
                    break;
                case SoundType.POWER_DOWN:
                    break;
                case SoundType.METEOR_INCOMING:
                    break;
                case SoundType.HEALTH_GAIN:
                    break;
                case SoundType.HEALTH_LOSS:
                    break;
                case SoundType.GAME_START:
                    break;
                case SoundType.GAME_OVER:
                    break;
                default:
                    break;
            }
        }

        public static void ResumeSound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.MENU_SELECT:
                    break;
                case SoundType.BACKGROUND_MUSIC:
                    {
                        if (BACKGROUND_MUSIC is not null)
                        {
                            BACKGROUND_MUSIC.Resume();
                        }
                    }
                    break;
                case SoundType.PLAYER_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_RAPID_SHOT_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_DEAD_SHOT_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_DOOM_SHOT_ROUNDS_FIRE:
                    break;
                case SoundType.ENEMY_ROUNDS_FIRE:
                    break;
                case SoundType.ENEMY_DESTRUCTION:
                    break;
                case SoundType.METEOR_DESTRUCTION:
                    break;
                case SoundType.BOSS_APPEARANCE:
                    {
                        if (BOSS_APPEARANCE is not null)
                        {
                            BOSS_APPEARANCE.Resume();
                        }
                    }
                    break;
                case SoundType.BOSS_DESTRUCTION:
                    break;
                case SoundType.ROUNDS_HIT:
                    break;
                case SoundType.POWER_UP:
                    break;
                case SoundType.POWER_DOWN:
                    break;
                case SoundType.METEOR_INCOMING:
                    break;
                case SoundType.HEALTH_GAIN:
                    break;
                case SoundType.HEALTH_LOSS:
                    break;
                case SoundType.GAME_START:
                    break;
                case SoundType.GAME_OVER:
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
