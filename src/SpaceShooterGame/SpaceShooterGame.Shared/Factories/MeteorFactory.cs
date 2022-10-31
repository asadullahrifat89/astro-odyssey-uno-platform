using System;
using System.Linq;

namespace SpaceShooterGame
{
    public static class MeteorFactory
    {
        #region Fields

        private static GameEnvironment _gameView;

        private static readonly Random _random = new();

        private static int _rotatedMeteorSpawnCounter = 10;
        private static int _rotatedMeteorSpawnAfter = 10;

        private static int _overPoweredMeteorSpawnCounter = 15;
        private static int _overPoweredMeteorSpawnAfter = 15;

        private static double _meteorSpeed = 1.5;
        private static double _maxMeteorCount = 7;
        private static int _meteorSpawnCounter = 10;

        #endregion

        #region Methods

        #region Public

        public static void Reset()
        {
            _rotatedMeteorSpawnCounter = 10;
            _rotatedMeteorSpawnAfter = 10;

            _overPoweredMeteorSpawnCounter = 15;
            _overPoweredMeteorSpawnAfter = 15;

            _meteorSpeed = 1.5;

            _maxMeteorCount = 7;
            _meteorSpawnCounter = 10;
        }

        /// <summary>
        /// Sets the game environment.
        /// </summary>
        /// <param name="gameView"></param>
        public static void SetGameEnvironment(GameEnvironment gameView)
        {
            _gameView = gameView;
        }

        /// <summary>
        /// Spawns a meteor.
        /// </summary>
        public static void SpawnMeteor(int gameLevel)
        {
            if (_gameView.GetGameObjects<Meteor>().Count() < (_gameView.IsBossEngaged ? _maxMeteorCount - 2 : _maxMeteorCount))
            {
                _meteorSpawnCounter--;

                if (_meteorSpawnCounter < 0)
                {
                    GenerateMeteor(gameLevel);
                    _meteorSpawnCounter = 10;
                }
            }
        }

        /// <summary>
        /// Generates a random meteor.
        /// </summary>
        public static void GenerateMeteor(int gameLevel)
        {
            _rotatedMeteorSpawnCounter--;
            _overPoweredMeteorSpawnCounter--;

            var meteor = new Meteor();

            meteor.SetAttributes(
                speed: _meteorSpeed + _random.NextDouble(),
                scale: _gameView.GameObjectScale);

            double left = 0;
            double top;

            // generate large but slower and stronger enemies after level 3
            if (gameLevel > 3 && _overPoweredMeteorSpawnCounter <= 0)
                SetOverPoweredMeteor(meteor);

            // generate side ways flying meteors after level 2
            if (gameLevel > 2 && _rotatedMeteorSpawnCounter <= 0)
            {
                meteor.XDirection = (XDirection)_random.Next(1, Enum.GetNames<XDirection>().Length);
                _rotatedMeteorSpawnCounter = _rotatedMeteorSpawnAfter;

                switch (meteor.XDirection)
                {
                    case XDirection.LEFT:
                        {
                            meteor.Rotation = (meteor.Rotation + 1) * -1;
                            left = _gameView.Width;
                        }
                        break;
                    case XDirection.RIGHT:
                        {
                            meteor.Rotation = meteor.Rotation + 1;
                            left = 0 - meteor.Width + 10;
                        }
                        break;
                    default:
                        break;
                }

                // side ways meteors fly at a higher speed
                meteor.Speed++;

                top = _random.Next(0, (int)_gameView.Height / 3);
                meteor.Rotate();

                // randomize next x flying meteor pop up
                _rotatedMeteorSpawnAfter = _random.Next(5, 15);
            }
            else
            {
                left = _random.Next(10, (int)_gameView.Width - 70);
                top = _random.Next(100 * (int)_gameView.GameObjectScale, (int)_gameView.Height) * -1;
            }

            meteor.AddToGameEnvironment(
                top: top,
                left: left,
                gameEnvironment: _gameView);
        }

        /// <summary>
        /// Destroys a meteor. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="meteor"></param>
        public static void DestroyByPlayerProjectle(Meteor meteor)
        {
            meteor.IsDestroyedByCollision = true;
            AudioHelper.PlaySound(SoundType.METEOR_DESTRUCTION);
        }

        /// <summary>
        /// Updates an meteor. Moves the meteor inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="meteor"></param>
        /// <param name="destroyed"></param>
        public static bool UpdateMeteor(Meteor meteor)
        {
            bool destroyed = false;

            // only remove meteors that are shot down by the player
            if (meteor.IsDestroyedByCollision)
            {
                meteor.Explode();

                if (meteor.HasExploded)
                {
                    _gameView.AddDestroyableGameObject(meteor);
                    destroyed = true;
                }
            }
            else
            {
                meteor.CoolDownProjectileImpactEffect();

                // move meteor down
                meteor.Rotate();
                meteor.MoveY();
                meteor.MoveX();

                // if meteor has gone beyond game view naturally, recycle it
                if (_gameView.IsRecyclable(meteor) || (meteor.IsImpactedByProjectile && meteor.GetY() + meteor.Height < 0))
                {
                    RecycleMeteor(meteor);
                    destroyed = true;
                }
            }

            return destroyed;
        }

        /// <summary>
        /// Levels up meteors.
        /// </summary>
        public static void LevelUp()
        {
            var scale = _gameView.GameObjectScale;
            _meteorSpeed += (1.3 * scale);
        }

        #endregion

        #region Private

        /// <summary>
        /// Generates a meteor that is overpowered. Slower but stronger and bigger meteors.
        /// </summary>
        /// <param name="meteor"></param>
        private static void SetOverPoweredMeteor(Meteor meteor)
        {
            meteor.OverPower();
            _overPoweredMeteorSpawnCounter = _overPoweredMeteorSpawnAfter;
            _overPoweredMeteorSpawnAfter = _random.Next(10, 20);
        }

        private static void RecycleMeteor(Meteor meteor)
        {
            meteor.SetAttributes(
                speed: _meteorSpeed + _random.NextDouble(),
                scale: _gameView.GameObjectScale,
                recycle: true);

            meteor.SetPosition(
                left: _random.Next(0, (int)_gameView.Width) - (100 * _gameView.GameObjectScale),
                top: _random.Next(100 * (int)_gameView.GameObjectScale, (int)_gameView.Height) * -1);
        }

        #endregion

        #endregion
    }
}
