using System;
using System.Linq;

namespace SpaceShooterGame
{
    public static class AudioHelper
    {
        #region Fields        

        private static Random random = new();

        private static AudioPlayer INTRO = null;
        private static AudioPlayer MENU_SELECT = null;
        private static AudioPlayer BACKGROUND = null;
        private static AudioPlayer PLAYER_ROUNDS_FIRE = null;
        private static AudioPlayer PLAYER_BLAZE_BLITZ_ROUNDS_FIRE = null;
        private static AudioPlayer PLAYER_PLASMA_BOMB_ROUNDS_FIRE = null;
        private static AudioPlayer PLAYER_BEAM_CANNON_ROUNDS_FIRE = null;
        private static AudioPlayer PLAYER_SONIC_BLAST_ROUNDS_FIRE = null;
        private static AudioPlayer ENEMY_ROUNDS_FIRE = null;
        private static AudioPlayer ENEMY_DESTRUCTION = null;
        private static AudioPlayer METEOR_DESTRUCTION = null;
        private static AudioPlayer BOSS_APPEARANCE = null;
        private static AudioPlayer BOSS_DESTRUCTION = null;
        private static AudioPlayer ROUNDS_HIT = null;
        private static AudioPlayer POWER_UP = null;
        private static AudioPlayer POWER_DOWN = null;
        private static AudioPlayer RAGE_UP = null;
        private static AudioPlayer RAGE_DOWN = null;
        private static AudioPlayer LEVEL_UP = null;
        private static AudioPlayer HEALTH_GAIN = null;
        private static AudioPlayer HEALTH_LOSS = null;
        private static AudioPlayer COLLECTIBLE_COLLECTED = null;
        private static AudioPlayer GAME_START = null;
        private static AudioPlayer GAME_OVER = null;
        private static AudioPlayer SCORE_MULTIPLIER_ON = null;
        private static AudioPlayer SCORE_MULTIPLIER_OFF = null;

        #endregion     

        #region Methods

        public static void LoadGameSounds(Action completed = null)
        {
            var baseUrl = AssetHelper.GetBaseUrl();

            GAME_START ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.GAME_START).Value}");
            GAME_OVER ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.GAME_OVER).Value}");
            MENU_SELECT ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.MENU_SELECT).Value}");
            BOSS_DESTRUCTION ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.BOSS_DESTRUCTION).Value}");

            PLAYER_ROUNDS_FIRE ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.PLAYER_ROUNDS_FIRE).Value}", volume: 0.1);
            PLAYER_BLAZE_BLITZ_ROUNDS_FIRE ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE).Value}", volume: 0.2);
            PLAYER_PLASMA_BOMB_ROUNDS_FIRE ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE).Value}", volume: 0.3);
            PLAYER_BEAM_CANNON_ROUNDS_FIRE ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.PLAYER_BEAM_CANNON_ROUNDS_FIRE).Value}", volume: 0.3);
            PLAYER_SONIC_BLAST_ROUNDS_FIRE ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.PLAYER_SONIC_BLAST_ROUNDS_FIRE).Value}", volume: 0.5);

            ENEMY_ROUNDS_FIRE ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.ENEMY_ROUNDS_FIRE).Value}", volume: 0.1);
            ROUNDS_HIT ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.ROUNDS_HIT).Value}", volume: 0.3);

            METEOR_DESTRUCTION ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.METEOR_DESTRUCTION).Value}", volume: 0.3);
            ENEMY_DESTRUCTION ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.ENEMY_DESTRUCTION).Value}", volume: 0.4);

            LEVEL_UP ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.ENEMY_INCOMING).Value}", volume: 0.8);
            COLLECTIBLE_COLLECTED ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.COLLECTIBLE_COLLECTED).Value}", volume: 0.4);

            POWER_UP ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.POWER_UP).Value}");
            POWER_DOWN ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.POWER_DOWN).Value}");

            RAGE_UP ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.RAGE_UP).Value}");
            RAGE_DOWN ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.RAGE_DOWN).Value}");

            HEALTH_GAIN ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.HEALTH_GAIN).Value}");
            HEALTH_LOSS ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.HEALTH_LOSS).Value}");

            SCORE_MULTIPLIER_ON ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.SCORE_MULTIPLIER_ON).Value}");
            SCORE_MULTIPLIER_OFF ??= new AudioPlayer(source: $"{baseUrl}/{Constants.SOUND_TEMPLATES.FirstOrDefault(x => x.Key == SoundType.SCORE_MULTIPLIER_OFF).Value}");

            completed?.Invoke();
        }

        public static void PlaySound(SoundType soundType)
        {
            var baseUrl = AssetHelper.GetBaseUrl();

            switch (soundType)
            {
                case SoundType.INTRO:
                    {
                        var gameIntros = Constants.SOUND_TEMPLATES.Where(x => x.Key == SoundType.INTRO).Select(v => v.Value).ToArray();
                        var musicTrack = random.Next(0, gameIntros.Length);
                        var src = gameIntros[musicTrack];

                        var source = string.Concat(baseUrl, "/", src);

                        if (INTRO is null)
                        {
                            INTRO = new AudioPlayer(
                                source: source,
                                volume: 0.5,
                                loop: true);
                        }
                        else
                        {
                            INTRO.SetSource(source);
                        }

                        INTRO.Play();
                    }
                    break;
                case SoundType.BACKGROUND:
                    {
                        var backgrounds = Constants.SOUND_TEMPLATES.Where(x => x.Key == SoundType.BACKGROUND).Select(v => v.Value).ToArray();
                        var musicTrack = random.Next(0, backgrounds.Length);
                        var src = backgrounds[musicTrack];

                        var source = string.Concat(baseUrl, "/", src);

                        if (BACKGROUND is null)
                        {
                            BACKGROUND = new AudioPlayer(
                                source: source,
                                volume: 0.4,
                                loop: true);
                        }
                        else
                        {
                            BACKGROUND.SetSource(source);
                        }

                        BACKGROUND.Play();
                    }
                    break;
                case SoundType.BOSS_APPEARANCE:
                    {
                        var bossAppearances = Constants.SOUND_TEMPLATES.Where(x => x.Key == SoundType.BOSS_APPEARANCE).Select(v => v.Value).ToArray();
                        var musicTrack = random.Next(0, bossAppearances.Length);
                        var src = bossAppearances[musicTrack];

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
                case SoundType.GAME_START:
                    {
                        GAME_START.Play();
                    }
                    break;
                case SoundType.GAME_OVER:
                    {
                        GAME_OVER.Play();
                    }
                    break;
                case SoundType.MENU_SELECT:
                    {
                        MENU_SELECT.Play();
                    }
                    break;
                case SoundType.BOSS_DESTRUCTION:
                    {
                        BOSS_DESTRUCTION.Play();
                    }
                    break;
                case SoundType.PLAYER_ROUNDS_FIRE:
                    {
                        PLAYER_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE:
                    {
                        PLAYER_BLAZE_BLITZ_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE:
                    {
                        PLAYER_PLASMA_BOMB_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_BEAM_CANNON_ROUNDS_FIRE:
                    {
                        PLAYER_BEAM_CANNON_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_SONIC_BLAST_ROUNDS_FIRE:
                    {
                        PLAYER_SONIC_BLAST_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.ENEMY_ROUNDS_FIRE:
                    {
                        ENEMY_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.METEOR_DESTRUCTION:
                    {
                        METEOR_DESTRUCTION.Play();
                    }
                    break;
                case SoundType.ROUNDS_HIT:
                    {
                        ROUNDS_HIT.Play();
                    }
                    break;
                case SoundType.POWER_UP:
                    {
                        POWER_UP.Play();
                    }
                    break;
                case SoundType.POWER_DOWN:
                    {
                        POWER_DOWN.Play();
                    }
                    break;
                case SoundType.RAGE_UP:
                    {
                        RAGE_UP.Play();
                    }
                    break;
                case SoundType.RAGE_DOWN:
                    {
                        RAGE_DOWN.Play();
                    }
                    break;
                case SoundType.ENEMY_INCOMING:
                    {
                        LEVEL_UP.Play();
                    }
                    break;
                case SoundType.ENEMY_DESTRUCTION:
                    {
                        ENEMY_DESTRUCTION.Play();
                    }
                    break;
                case SoundType.HEALTH_GAIN:
                    {
                        HEALTH_GAIN.Play();
                    }
                    break;
                case SoundType.HEALTH_LOSS:
                    {
                        HEALTH_LOSS.Play();
                    }
                    break;
                case SoundType.COLLECTIBLE_COLLECTED:
                    {
                        COLLECTIBLE_COLLECTED.Play();
                    }
                    break;
                case SoundType.SCORE_MULTIPLIER_ON:
                    {
                        SCORE_MULTIPLIER_ON.Play();
                    }
                    break;
                case SoundType.SCORE_MULTIPLIER_OFF:
                    {
                        SCORE_MULTIPLIER_OFF.Play();
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
        }

        public static void StopSound()
        {
            if (INTRO is not null)
                INTRO.Stop();

            if (BACKGROUND is not null)
                BACKGROUND.Stop();

            if (BOSS_APPEARANCE is not null)
                BOSS_APPEARANCE.Stop();
        }

        public static void PauseSound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.BACKGROUND:
                    {
                        if (BACKGROUND is not null)
                            BACKGROUND.Pause();
                    }
                    break;
                case SoundType.BOSS_APPEARANCE:
                    {
                        if (BOSS_APPEARANCE is not null)
                            BOSS_APPEARANCE.Pause();
                    }
                    break;
                default:
                    break;
            }
        }

        public static void ResumeSound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.BACKGROUND:
                    {
                        if (BACKGROUND is not null)
                            BACKGROUND.Resume();
                    }
                    break;
                case SoundType.BOSS_APPEARANCE:
                    {
                        if (BOSS_APPEARANCE is not null)
                            BOSS_APPEARANCE.Resume();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion
    }

    public enum SoundType
    {
        INTRO,
        MENU_SELECT,
        BACKGROUND,
        PLAYER_ROUNDS_FIRE,
        PLAYER_BLAZE_BLITZ_ROUNDS_FIRE,
        PLAYER_PLASMA_BOMB_ROUNDS_FIRE,
        PLAYER_BEAM_CANNON_ROUNDS_FIRE,
        PLAYER_SONIC_BLAST_ROUNDS_FIRE,
        ENEMY_ROUNDS_FIRE,
        ENEMY_DESTRUCTION,
        METEOR_DESTRUCTION,
        BOSS_APPEARANCE,
        BOSS_DESTRUCTION,
        ROUNDS_HIT,
        POWER_UP,
        POWER_DOWN,
        RAGE_UP,
        RAGE_DOWN,
        ENEMY_INCOMING,
        HEALTH_GAIN,
        HEALTH_LOSS,
        COLLECTIBLE_COLLECTED,
        GAME_START,
        GAME_OVER,
        SCORE_MULTIPLIER_ON,
        SCORE_MULTIPLIER_OFF
    }
}
