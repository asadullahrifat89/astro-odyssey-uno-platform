using System;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public static class AudioHelper
    {
        #region Fields        

        private static Random random = new Random();

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
                case SoundType.BACKGROUND_MUSIC:
                    {
                        var musicTrack = random.Next(1, 12);
                        var src = "";

                        switch (musicTrack)
                        {
                            case 1: { src = "Assets/Sounds/slow-trap-18565.mp3"; } break;
                            case 2: { src = "Assets/Sounds/space-chillout-14194.mp3"; } break;
                            case 3: { src = "Assets/Sounds/cinematic-space-drone-10623.mp3"; } break;
                            case 4: { src = "Assets/Sounds/slow-thoughtful-sad-piano-114586.mp3"; } break;
                            case 5: { src = "Assets/Sounds/space-age-10714.mp3"; } break;
                            case 6: { src = "Assets/Sounds/drone-space-main-9706.mp3"; } break;
                            case 7: { src = "Assets/Sounds/cyberpunk-2099-10701.mp3"; } break;
                            case 8: { src = "Assets/Sounds/insurrection-10941.mp3"; } break;
                            case 9: { src = "Assets/Sounds/space-trip-114102.mp3"; } break;
                            case 10: { src = "Assets/Sounds/dark-matter-10710.mp3"; } break;
                            case 11: { src = "Assets/Sounds/music-807dfe09ce23793891674eb022b38c1b.mp3"; } break;
                            default:
                                break;
                        }

                        var source = string.Concat(baseUrl, "/", src);

                        if (BACKGROUND_MUSIC is null)
                        {
                            BACKGROUND_MUSIC = new AudioPlayer(
                                source: source,
                                volume: 0.3);

                            BACKGROUND_MUSIC.OnCompleted += (s, e) =>
                            {
                                PlaySound(soundType);
                            };
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
                case SoundType.BOSS_APPEARANCE:
                    {
                        if (BOSS_APPEARANCE is null)
                        {
                            BOSS_APPEARANCE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/dark-sitar-7546.mp3"),
                                volume: 1.0,
                                loop: true);
                        }

                        BOSS_APPEARANCE.Play();
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
                    if (BACKGROUND_MUSIC is not null)
                    {
                        BACKGROUND_MUSIC.Pause();
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
                    if (BOSS_APPEARANCE is not null)
                    {
                        BOSS_APPEARANCE.Pause();
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
                    if (BACKGROUND_MUSIC is not null)
                    {
                        BACKGROUND_MUSIC.Play();
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
                    if (BOSS_APPEARANCE is not null)
                    {
                        BOSS_APPEARANCE.Play();
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
