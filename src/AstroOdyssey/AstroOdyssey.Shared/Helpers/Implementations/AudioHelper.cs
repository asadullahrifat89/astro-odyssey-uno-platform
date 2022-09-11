using System;

namespace AstroOdyssey
{
    public class AudioHelper: IAudioHelper
    {
        #region Fields        

        private Random random = new Random();

        private AudioPlayer GAME_INTRO = null;
        private AudioPlayer MENU_SELECT = null;
        private AudioPlayer BACKGROUND_MUSIC = null;
        private AudioPlayer PLAYER_ROUNDS_FIRE = null;
        private AudioPlayer PLAYER_BLAZE_BLITZ_ROUNDS_FIRE = null;
        private AudioPlayer PLAYER_PLASMA_BOMB_ROUNDS_FIRE = null;
        private AudioPlayer PLAYER_BEAM_CANNON_ROUNDS_FIRE = null;
        private AudioPlayer PLAYER_SONIC_BLAST_ROUNDS_FIRE = null;
        private AudioPlayer ENEMY_ROUNDS_FIRE = null;
        private AudioPlayer ENEMY_DESTRUCTION = null;
        private AudioPlayer METEOR_DESTRUCTION = null;
        private AudioPlayer BOSS_APPEARANCE = null;
        private AudioPlayer BOSS_DESTRUCTION = null;
        private AudioPlayer ROUNDS_HIT = null;
        private AudioPlayer POWER_UP = null;
        private AudioPlayer POWER_DOWN = null;
        private AudioPlayer RAGE_UP = null;
        private AudioPlayer RAGE_DOWN = null;
        private AudioPlayer LEVEL_UP = null;
        private AudioPlayer HEALTH_GAIN = null;
        private AudioPlayer HEALTH_LOSS = null;
        private AudioPlayer COLLECTIBLE_COLLECTED = null;
        private AudioPlayer GAME_START = null;
        private AudioPlayer GAME_OVER = null;

        #endregion     

        #region Methods

        public void PlaySound(SoundType soundType)
        {
            var baseUrl = App.GetBaseUrl();

            switch (soundType)
            {
                case SoundType.GAME_INTRO:
                    {
                        var musicTrack = random.Next(0, GameObjectTemplates.GAME_INTRO_MUSIC_TEMPLATES.Length);
                        var src = GameObjectTemplates.GAME_INTRO_MUSIC_TEMPLATES[musicTrack];

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
                case SoundType.GAME_START:
                    {
                        if (GAME_START is null)
                        {
                            GAME_START = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Game/space-jet-flyby_MkgS2BVu_NWM.mp3"),
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
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Game/videogame-death-sound-43894.mp3"),
                                volume: 1.0);
                        }

                        GAME_OVER.Play();
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
                        var musicTrack = random.Next(0, GameObjectTemplates.BOSS_APPEARANCE_MUSIC_TEMPLATES.Length);
                        var src = GameObjectTemplates.BOSS_APPEARANCE_MUSIC_TEMPLATES[musicTrack];

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
                case SoundType.BACKGROUND_MUSIC:
                    {
                        var musicTrack = random.Next(0, GameObjectTemplates.BACKGROUND_MUSIC_TEMPLATES.Length);
                        var src = GameObjectTemplates.BACKGROUND_MUSIC_TEMPLATES[musicTrack];

                        var source = string.Concat(baseUrl, "/", src);

                        if (BACKGROUND_MUSIC is null)
                        {
                            BACKGROUND_MUSIC = new AudioPlayer(
                                source: source,
                                volume: 0.4,
                                loop: true);
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
                case SoundType.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE:
                    {
                        if (PLAYER_BLAZE_BLITZ_ROUNDS_FIRE is null)
                        {
                            PLAYER_BLAZE_BLITZ_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/alien-computer-program-deactivate_GkreEFV__NWM.mp3"),
                                volume: 0.2);
                        }

                        PLAYER_BLAZE_BLITZ_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE:
                    {
                        if (PLAYER_PLASMA_BOMB_ROUNDS_FIRE is null)
                        {
                            PLAYER_PLASMA_BOMB_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/magnetic-destroy-shot_fkxD6SV__NWM.mp3"),
                                volume: 0.3);
                        }

                        PLAYER_PLASMA_BOMB_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_BEAM_CANNON_ROUNDS_FIRE:
                    {
                        if (PLAYER_BEAM_CANNON_ROUNDS_FIRE is null)
                        {
                            PLAYER_BEAM_CANNON_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/punchy-laser-shot_f11BarNO_NWM.mp3"),
                                volume: 0.3);
                        }

                        PLAYER_BEAM_CANNON_ROUNDS_FIRE.Play();
                    }
                    break;
                case SoundType.PLAYER_SONIC_BLAST_ROUNDS_FIRE:
                    {
                        if (PLAYER_SONIC_BLAST_ROUNDS_FIRE is null)
                        {
                            PLAYER_SONIC_BLAST_ROUNDS_FIRE = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/plasmablaster-37114.mp3"),
                                volume: 0.5);
                        }

                        PLAYER_SONIC_BLAST_ROUNDS_FIRE.Play();
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
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Power/spellcast-46164.mp3"),
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
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Power/power-down-7103.mp3"),
                                volume: 1.0);
                        }

                        POWER_DOWN.Play();
                    }
                    break;
                case SoundType.RAGE_UP:
                    {
                        if (RAGE_UP is null)
                        {
                            RAGE_UP = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Rage/audioblocks-hi-tech-metal-whoosh_HZs-SOQFvI_NWM.mp3"),
                                volume: 1.0);
                        }

                        RAGE_UP.Play();
                    }
                    break;
                case SoundType.RAGE_DOWN:
                    {
                        if (RAGE_DOWN is null)
                        {
                            RAGE_DOWN = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Rage/audioblocks-hi-tech-metal-whoosh-2_rZh-HuQFwI_NWM.mp3"),
                                volume: 1.0);
                        }

                        RAGE_DOWN.Play();
                    }
                    break;
                case SoundType.ENEMY_INCOMING:
                    {
                        if (LEVEL_UP is null)
                        {
                            LEVEL_UP = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Enemy/asteroid-incoming-effect.mp3"),
                                volume: 0.8);
                        }

                        LEVEL_UP.Play();
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
                case SoundType.HEALTH_GAIN:
                    {
                        if (HEALTH_GAIN is null)
                        {
                            HEALTH_GAIN = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Health/scale-e6-14577.mp3"),
                                volume: 1.0);
                        }

                        HEALTH_GAIN.Play();
                    }
                    break;
                case SoundType.COLLECTIBLE_COLLECTED:
                    {
                        if (COLLECTIBLE_COLLECTED is null)
                        {
                            COLLECTIBLE_COLLECTED = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Collectible/8-bit-powerup-6768.mp3"),
                                volume: 0.4);
                        }

                        COLLECTIBLE_COLLECTED.Play();
                    }
                    break;
                case SoundType.HEALTH_LOSS:
                    {
                        if (HEALTH_LOSS is null)
                        {
                            HEALTH_LOSS = new AudioPlayer(
                                source: string.Concat(baseUrl, "/", "Assets/Sounds/Health/rocket-missile-launcher_MyHKjH4__NWM.mp3"),
                                volume: 1.0);
                        }

                        HEALTH_LOSS.Play();
                    }
                    break;
                default:
                    {
                        // App.PlaySound(baseUrl, soundType);
                    }
                    break;
            }
        }

        public void StopSound()
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

        public void PauseSound(SoundType soundType)
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
                case SoundType.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_BEAM_CANNON_ROUNDS_FIRE:
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
                case SoundType.ENEMY_INCOMING:
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

        public void ResumeSound(SoundType soundType)
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
                case SoundType.PLAYER_BLAZE_BLITZ_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_PLASMA_BOMB_ROUNDS_FIRE:
                    break;
                case SoundType.PLAYER_BEAM_CANNON_ROUNDS_FIRE:
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
                case SoundType.ENEMY_INCOMING:
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

        private void BACKGROUND_MUSIC_onended(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public enum SoundType
    {
        GAME_INTRO,
        MENU_SELECT,
        BACKGROUND_MUSIC,
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
    }
}
