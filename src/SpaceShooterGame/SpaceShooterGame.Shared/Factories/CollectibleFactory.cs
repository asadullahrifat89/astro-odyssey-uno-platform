using System;

namespace SpaceShooterGame
{
    public static class CollectibleFactory
    {
        #region Fields

        private static GameEnvironment _gameView;

        private static readonly Random _random = new();

        private static int _collectibleSpawnCounter;
        private static int _collectibleSpawnAfter = 500;
        private static double _collectibleSpeed = 2;

        #endregion      

        #region Methods

        #region Public

        public static void Reset()
        {
            _collectibleSpawnAfter = 500;
            _collectibleSpeed = 2;
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
        /// Spawns a Collectible.
        /// </summary>
        public static void SpawnCollectible(GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            _collectibleSpawnCounter -= 1;

            // when counter reaches zero, create a Collectible
            if (_collectibleSpawnCounter < 0)
            {
                GenerateCollectible();
                _collectibleSpawnCounter = _collectibleSpawnAfter;
                //_collectibleSpawnAfter = _random.Next(500, 600);
            }
        }

        /// <summary>
        /// Generates a random Collectible.
        /// </summary>
        public static void GenerateCollectible()
        {
            var collectible = new Collectible();

            collectible.SetAttributes(speed: _collectibleSpeed + _random.NextDouble(), scale: _gameView.GameObjectScale);
            collectible.AddToGameEnvironment(top: 0 - collectible.Height, left: _random.Next(10, (int)_gameView.Width - 100), gameEnvironment: _gameView);
        }

        /// <summary>
        /// Updates an Collectible. Moves the Collectible inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="collectible"></param>
        /// <param name="destroyed"></param>
        public static bool UpdateCollectible(Collectible collectible)
        {
            bool destroyed = false;

            // move Collectible down
            collectible.MoveY();

            // if Collectible or meteor object has gone beyond game view
            destroyed = _gameView.CheckAndAddDestroyableGameObject(collectible);

            return destroyed;
        }

        /// <summary>
        /// Levels up Collectibles.
        /// </summary>
        public static void LevelUp()
        {
            if (_collectibleSpawnAfter > 500 / 3)
            {
                var scale = _gameView.GameObjectScale;
                _collectibleSpawnAfter -= 15;
                _collectibleSpeed += (1 * scale);
            }
        }

        #endregion

        #endregion
    }
}
