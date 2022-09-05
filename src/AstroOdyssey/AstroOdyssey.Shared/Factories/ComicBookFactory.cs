using System;

namespace AstroOdyssey
{
    public class ComicBookFactory
    {
        #region Fields

        private readonly GameEnvironment _gameEnvironment;

        private readonly Random _random = new Random();

        private int _comicBookSpawnCounter;
        private int _comicBookSpawnDelay = 1000;
        private double _comicBookSpeed = 2;

        #endregion

        #region Ctor

        public ComicBookFactory(GameEnvironment gameEnvironment)
        {
            this._gameEnvironment = gameEnvironment;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Spawns a ComicBook.
        /// </summary>
        public void SpawnComicBook(GameLevel gameLevel)
        {
            if (gameLevel >= GameLevel.Level_1)
            {
                // each frame progress decreases this counter
                _comicBookSpawnCounter -= 1;

                // when counter reaches zero, create a ComicBook
                if (_comicBookSpawnCounter < 0)
                {
                    GenerateComicBook();
                    _comicBookSpawnCounter = _comicBookSpawnDelay;
                    _comicBookSpawnDelay = _random.Next(900, 1001);
                }
            }
        }

        /// <summary>
        /// Generates a random ComicBook.
        /// </summary>
        public void GenerateComicBook()
        {
            var ComicBook = new ComicBook();

            ComicBook.SetAttributes(speed: _comicBookSpeed + _random.NextDouble(), scale: _gameEnvironment.GetGameObjectScale());
            ComicBook.AddToGameEnvironment(top: 0 - ComicBook.Height, left: _random.Next(10, (int)_gameEnvironment.Width - 100), gameEnvironment: _gameEnvironment);

            // change the next ComicBook spawn time
            _comicBookSpawnDelay = _random.Next(1000, 1500);
        }

        /// <summary>
        /// Updates an ComicBook. Moves the ComicBook inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="ComicBook"></param>
        /// <param name="destroyed"></param>
        public void UpdateComicBook(ComicBook ComicBook, out bool destroyed)
        {
            destroyed = false;

            // move ComicBook down
            ComicBook.MoveY();

            // if ComicBook or meteor object has gone beyond game view
            destroyed = _gameEnvironment.CheckAndAddDestroyableGameObject(ComicBook);
        }

        /// <summary>
        /// Levels up ComicBooks.
        /// </summary>
        public void LevelUp()
        {
            _comicBookSpeed += 1;
        }

        #endregion
    }
}
