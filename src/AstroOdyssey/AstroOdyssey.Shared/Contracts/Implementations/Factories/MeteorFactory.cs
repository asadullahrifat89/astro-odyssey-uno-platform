using System;

namespace AstroOdyssey
{
    public class MeteorFactory : IMeteorFactory
    {
        #region Fields

        private GameEnvironment _gameEnvironment;

        private readonly Random _random = new Random();

        private int _rotatedMeteorSpawnCounter = 10;
        private int _rotatedMeteorSpawnAfter = 10;

        private int _overPoweredMeteorSpawnCounter = 15;
        private int _overPoweredMeteorSpawnAfter = 15;

        private double _meteorSpawnCounter;
        private double _meteorSpawnAfter = 55;
        private double _meteorSpeed = 1.5;

        private readonly IAudioHelper _audioHelper;
        #endregion

        #region Ctor

        public MeteorFactory(IAudioHelper audioHelper)
        {
            _audioHelper = audioHelper;
        }

        #endregion

        #region Methods

        #region Public

        public void Reset()
        {
            _rotatedMeteorSpawnCounter = 10;
            _rotatedMeteorSpawnAfter = 10;

            _overPoweredMeteorSpawnCounter = 15;
            _overPoweredMeteorSpawnAfter = 15;

            _meteorSpawnAfter = 55;
            _meteorSpeed = 1.5;
        }

        /// <summary>
        /// Sets the game environment.
        /// </summary>
        /// <param name="gameEnvironment"></param>
        public void SetGameEnvironment(GameEnvironment gameEnvironment)
        {
            _gameEnvironment = gameEnvironment;
        }

        /// <summary>
        /// Spawns a meteor.
        /// </summary>
        public void SpawnMeteor(GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            _meteorSpawnCounter -= 1;

            // when counter reaches zero, create a meteor
            if (_meteorSpawnCounter < 0)
            {
                GenerateMeteor(gameLevel);
                _meteorSpawnCounter = _meteorSpawnAfter;
            }
        }

        /// <summary>
        /// Generates a random meteor.
        /// </summary>
        public void GenerateMeteor(GameLevel gameLevel)
        {
            var meteor = new Meteor();

            meteor.SetAttributes(speed: _meteorSpeed + _random.NextDouble(), scale: _gameEnvironment.GetGameObjectScale());

            _rotatedMeteorSpawnCounter--;
            _overPoweredMeteorSpawnCounter--;

            double left = 0;
            double top = 0;

            // generate large but slower and stronger enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _overPoweredMeteorSpawnCounter <= 0)
            {
                SetOverPoweredMeteor(meteor);
            }

            // generate side ways flying meteors after level 2
            if (gameLevel > GameLevel.Level_2 && _rotatedMeteorSpawnCounter <= 0)
            {
                SetSideWaysMovingMeteor(meteor, ref left, ref top);
            }
            else
            {
                left = _random.Next(10, (int)_gameEnvironment.Width - 70);
                top = 0 - meteor.Height;
            }

            meteor.AddToGameEnvironment(top: 0 - meteor.Height, left: _random.Next(10, (int)_gameEnvironment.Width - 100), gameEnvironment: _gameEnvironment);
        }

        /// <summary>
        /// Destroys a meteor. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="meteor"></param>
        public void DestroyMeteor(Meteor meteor)
        {
            meteor.IsMarkedForFadedDestruction = true;
            _audioHelper.PlaySound(SoundType.METEOR_DESTRUCTION);
        }

        /// <summary>
        /// Updates an meteor. Moves the meteor inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="meteor"></param>
        /// <param name="destroyed"></param>
        public bool UpdateMeteor(Meteor meteor)
        {
            bool destroyed = false;

            meteor.CoolDownProjectileImpactEffect();

            // move meteor down
            meteor.Rotate();
            meteor.MoveY();
            meteor.MoveX();

            // if the object is marked for lazy destruction then do not destroy immidiately
            if (meteor.IsMarkedForFadedDestruction)
                return false;

            // if meteor or meteor object has gone beyond game view
            destroyed = _gameEnvironment.CheckAndAddDestroyableGameObject(meteor);

            return destroyed;
        }

        /// <summary>
        /// Levels up meteors.
        /// </summary>
        public void LevelUp()
        {
            var scale = _gameEnvironment.GetGameObjectScale();

            if (_meteorSpawnAfter > 2)
                _meteorSpawnAfter -= 6 * scale;

            _meteorSpeed += (1 * scale);
        }

        #endregion

        #region Private

        /// <summary>
        /// Generates a meteor that is overpowered. Slower but stronger and bigger meteors.
        /// </summary>
        /// <param name="meteor"></param>
        private void SetOverPoweredMeteor(Meteor meteor)
        {
            _overPoweredMeteorSpawnCounter = _overPoweredMeteorSpawnAfter;
            meteor.OverPower();
            _overPoweredMeteorSpawnAfter = _random.Next(10, 20);
        }

        /// <summary>
        ///  Generates a meteor that moves sideways from one direction.
        /// </summary>
        /// <param name="meteor"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void SetSideWaysMovingMeteor(Meteor meteor, ref double left, ref double top)
        {
            meteor.XDirection = (XDirection)_random.Next(1, Enum.GetNames<XDirection>().Length);
            _rotatedMeteorSpawnCounter = _rotatedMeteorSpawnAfter;

            switch (meteor.XDirection)
            {
                case XDirection.LEFT:
                    meteor.Rotation = (meteor.Rotation + 1) * -1;
                    left = _gameEnvironment.Width;
                    break;
                case XDirection.RIGHT:
                    meteor.Rotation = meteor.Rotation + 1;
                    left = 0 - meteor.Width + 10;
                    break;
                default:
                    break;
            }

            // side ways meteors fly at a higher speed
            meteor.Speed++;
            //#if DEBUG
            //            Console.WriteLine("Meteor XDirection: " + meteor.XDirection + ", " + "X: " + left + " " + "Y: " + top);
            //#endif
            top = _random.Next(0, (int)_gameEnvironment.Height / 3);
            meteor.Rotate();

            // randomize next x flying meteor pop up
            _rotatedMeteorSpawnAfter = _random.Next(5, 15);
        }

        #endregion

        #endregion
    }
}
