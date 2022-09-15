using System;

namespace AstroOdyssey
{
    public class CollectibleFactory: ICollectibleFactory
    {
        #region Fields

        private GameEnvironment _gameEnvironment;

        private readonly Random _random = new Random();

        private int _collectibleSpawnCounter;
        private int _collectibleSpawnAfter = 600;
        private double _collectibleSpeed = 2;

        #endregion

        #region Ctor

        public CollectibleFactory()
        {

        }

        #endregion

        #region Methods

        #region Public

        public void SetGameEnvironment(GameEnvironment gameEnvironment)
        {
            _gameEnvironment = gameEnvironment;
        }

        /// <summary>
        /// Spawns a Collectible.
        /// </summary>
        public void SpawnCollectible(GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            _collectibleSpawnCounter -= 1;

            // when counter reaches zero, create a Collectible
            if (_collectibleSpawnCounter < 0)
            {
                GenerateCollectible();
                _collectibleSpawnCounter = _collectibleSpawnAfter;
                _collectibleSpawnAfter = _random.Next(500, 700);
            }
        }

        /// <summary>
        /// Generates a random Collectible.
        /// </summary>
        public void GenerateCollectible()
        {
            var collectible = new Collectible();

            collectible.SetAttributes(speed: _collectibleSpeed + _random.NextDouble(), scale: _gameEnvironment.GetGameObjectScale());
            collectible.AddToGameEnvironment(top: 0 - collectible.Height, left: _random.Next(10, (int)_gameEnvironment.Width - 100), gameEnvironment: _gameEnvironment);
        }

        /// <summary>
        /// Updates an Collectible. Moves the Collectible inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="collectible"></param>
        /// <param name="destroyed"></param>
        public void UpdateCollectible(Collectible collectible, out bool destroyed)
        {
            destroyed = false;

            // move Collectible down
            collectible.MoveY();

            // if Collectible or meteor object has gone beyond game view
            destroyed = _gameEnvironment.CheckAndAddDestroyableGameObject(collectible);
        }

        /// <summary>
        /// Levels up Collectibles.
        /// </summary>
        public void LevelUp()
        {
            var scale = _gameEnvironment.GetGameObjectScale();
            _collectibleSpeed += (1 * scale);
        } 

        #endregion

        #endregion
    }
}
