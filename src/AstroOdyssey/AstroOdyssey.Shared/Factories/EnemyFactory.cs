using System;

namespace AstroOdyssey
{
    public class EnemyFactory
    {
        #region Fields

        private readonly GameEnvironment _gameEnvironment;

        private readonly Random _random = new Random();

        private int _xFlyingEnemySpawnCounter = 10;
        private int _xFlyingEnemySpawnDelay = 10;

        private int _overPoweredEnemySpawnCounter = 15;
        private int _overPoweredEnemySpawnDelay = 15;

        private int _evadingEnemySpawnCounter = 3;
        private int _evadingEnemySpawnDelay = 3;

        private int _firingEnemySpawnCounter = 7;
        private int _firingEnemySpawnDelay = 7;

        private int _targetingEnemySpawnDelay = 10;
        private int _targetingEnemySpawnCounter = 10;

        private int _hoveringEnemySpawnDelay = 12;
        private int _hoveringEnemySpawnCounter = 12;

        private double _enemySpawnCounter;
        private double _enemySpawnDelay = 48;
        private double _enemySpeed = 2;

        #endregion

        #region Ctor

        public EnemyFactory(GameEnvironment gameEnvironment)
        {
            this._gameEnvironment = gameEnvironment;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Engages a boss.
        /// </summary>
        /// <param name="gameLevel"></param>
        public Enemy EngageBossEnemy(GameLevel gameLevel)
        {
            var enemy = new Enemy();

            enemy.SetAttributes(
                speed: _enemySpeed + _random.Next(0, 4),
                scale: _gameEnvironment.GetGameObjectScale());

            enemy.IsBoss = true;
            enemy.IsOverPowered = true;

            enemy.Height = enemy.Height * 2 + (int)gameLevel / 3 + 0.25d;
            enemy.Width = enemy.Width * 2 + (int)gameLevel / 3 + 0.25d;
            enemy.HalfWidth = enemy.Width / 2;
            enemy.Speed--;

            enemy.Health = 50 * (int)gameLevel;

            SetProjectileFiringEnemy(enemy);
            SetHoveringEnemy(enemy);

            // appear in the middle of the screen
            double left = _gameEnvironment.HalfWidth + enemy.HalfWidth;
            double top = 0 - enemy.Height;

            if (enemy.IsHovering)
                enemy.XDirection = (XDirection)_random.Next(1, Enum.GetNames<XDirection>().Length);

            enemy.AddToGameEnvironment(
                top: top,
                left: left,
                gameEnvironment: _gameEnvironment);

            _gameEnvironment.IsBossEngaged = true;

            AudioHelper.StopSound();
            AudioHelper.PlaySound(SoundType.BOSS_APPEARANCE);

            return enemy;
        }

        /// <summary>
        /// Disengages a boss.
        /// </summary>
        public void DisengageBossEnemy()
        {
            _gameEnvironment.IsBossEngaged = false;
            AudioHelper.StopSound();
            AudioHelper.PlaySound(SoundType.BOSS_DESTRUCTION);
            AudioHelper.PlaySound(SoundType.BACKGROUND_MUSIC);
        }

        /// <summary>
        /// Spawns an enemy.
        /// </summary>
        public void SpawnEnemy(GameLevel gameLevel)
        {
            if ((int)gameLevel > 0)
            {
                // each frame progress decreases this counter
                _enemySpawnCounter -= 1;

                // when counter reaches zero, create an enemy
                if (_enemySpawnCounter < 0)
                {
                    GenerateEnemy(gameLevel);
                    _enemySpawnCounter = _enemySpawnDelay;
                }
            }
        }

        /// <summary>
        /// Generates a random Enemy.
        /// </summary>
        public void GenerateEnemy(GameLevel gameLevel)
        {
            var enemy = new Enemy();

            enemy.SetAttributes(speed: _enemySpeed + _random.Next(0, 4), scale: _gameEnvironment.GetGameObjectScale());

            double left = 0;
            double top = 0;

            _xFlyingEnemySpawnCounter--;
            _overPoweredEnemySpawnCounter--;
            _evadingEnemySpawnCounter--;
            _firingEnemySpawnCounter--;
            _targetingEnemySpawnCounter--;
            _hoveringEnemySpawnCounter--;

            // spawn evading enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _evadingEnemySpawnCounter <= 0)
            {
                SetEvadingEnemy(enemy);
            }

            // spawn following enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _targetingEnemySpawnCounter <= 0)
            {
                SetPlayerTargetingEnemy(enemy);
            }

            // spawn following enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _hoveringEnemySpawnCounter <= 0)
            {
                SetHoveringEnemy(enemy);
            }

            // generate large but slower and stronger enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _overPoweredEnemySpawnCounter <= 0)
            {
                SetOverPoweredEnemy(enemy);
            }

            // spawn blaster shooting enemies after level 2
            if (gameLevel > GameLevel.Level_2 && _firingEnemySpawnCounter <= 0)
            {
                SetProjectileFiringEnemy(enemy);
            }

            // generate side ways flying enemies after level 2
            if (gameLevel > GameLevel.Level_2 && _xFlyingEnemySpawnCounter <= 0)
            {
                SetSideWaysMovingEnemy(enemy, ref left, ref top);
            }
            else
            {
                if (enemy.IsHovering)
                {
                    // appear in the middle of the screen
                    left = _gameEnvironment.HalfWidth + enemy.HalfWidth;
                    enemy.XDirection = XDirection.LEFT;
                }
                else
                {
                    left = _random.Next(10, (int)_gameEnvironment.Width - (int)enemy.Width);
                }

                top = 0 - enemy.Height;
            }

            enemy.AddToGameEnvironment(top: top, left: left, gameEnvironment: _gameEnvironment);
        }

        /// <summary>
        /// Generates an enemy that hovers across the screen.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetHoveringEnemy(Enemy enemy)
        {
            _hoveringEnemySpawnCounter = _hoveringEnemySpawnDelay;
            enemy.IsHovering = true;
            _hoveringEnemySpawnDelay = _random.Next(9, 13);

            // hovering enemies do not evade or collide with the player
            enemy.IsEvading = false;
            enemy.IsPlayerColliding = false;
        }

        /// <summary>
        /// Generates an enemy that evades player fire.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetEvadingEnemy(Enemy enemy)
        {
            _evadingEnemySpawnCounter = _evadingEnemySpawnDelay;
            enemy.IsEvading = true;
            _evadingEnemySpawnDelay = _random.Next(1, 4);
        }

        /// <summary>
        /// Generates an enemy that collides with the player.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetPlayerTargetingEnemy(Enemy enemy)
        {
            _targetingEnemySpawnCounter = _targetingEnemySpawnDelay;
            enemy.IsPlayerColliding = true;
            _targetingEnemySpawnDelay = _random.Next(7, 12);

            // player targeting enemies do not evade on hit
            enemy.IsEvading = false;
        }

        /// <summary>
        /// Generates an enemy that fires projectiles.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetProjectileFiringEnemy(Enemy enemy)
        {
            _firingEnemySpawnCounter = _firingEnemySpawnDelay;
            enemy.IsProjectileFiring = true;
            _firingEnemySpawnDelay = _random.Next(1, 8);
        }

        /// <summary>
        /// Generates an enemy that is overpowered. Bigger and stronger but slower enemies.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetOverPoweredEnemy(Enemy enemy)
        {
            _overPoweredEnemySpawnCounter = _overPoweredEnemySpawnDelay;
            enemy.OverPower();
            _overPoweredEnemySpawnDelay = _random.Next(10, 20);
        }

        /// <summary>
        /// Generates an enemy that moves sideways from one direction.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void SetSideWaysMovingEnemy(Enemy enemy, ref double left, ref double top)
        {
            enemy.XDirection = (XDirection)_random.Next(1, Enum.GetNames<XDirection>().Length);
            _xFlyingEnemySpawnCounter = _xFlyingEnemySpawnDelay;

            enemy.YDirection = (YDirection)_random.Next(0, Enum.GetNames<YDirection>().Length);

            // enemies can not go up
            if (enemy.YDirection == YDirection.UP)
                enemy.YDirection = YDirection.DOWN;

            switch (enemy.XDirection)
            {
                case XDirection.LEFT:
                    enemy.Rotation = enemy.YDirection == YDirection.NONE ? 0 : 50;
                    left = _gameEnvironment.Width;
                    break;
                case XDirection.RIGHT:
                    enemy.Rotation = enemy.YDirection == YDirection.NONE ? 0 : -50;
                    left = 0 - enemy.Width + 10;
                    break;
                default:
                    break;
            }
            //#if DEBUG
            //            Console.WriteLine("Enemy XDirection: " + enemy.XDirection + ", " + "X: " + left + " " + "Y: " + top);
            //#endif
            top = _random.Next(0, (int)_gameEnvironment.Height / 3);
            enemy.Rotate();

            // sideways flying enemies do not follow player or evade on hit
            enemy.IsPlayerColliding = false;
            enemy.IsEvading = false;
            enemy.IsHovering = false;

            // randomize next x flying enemy pop up
            _xFlyingEnemySpawnDelay = _random.Next(5, 15);
        }

        /// <summary>
        /// Destroys a Enemy. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="enemy"></param>
        public void DestroyEnemy(Enemy enemy)
        {
            enemy.IsMarkedForFadedDestruction = true;
            AudioHelper.PlaySound(SoundType.ENEMY_DESTRUCTION);
        }

        /// <summary>
        /// Updates an enemy. Moves the enemy inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="destroyed"></param>
        public void UpdateEnemy(Enemy enemy, double pointerX, out bool destroyed)
        {
            destroyed = false;

            enemy.CoolDownProjectileImpactEffect();

            if (enemy.IsBoss)
            {
                // move boss down upto a certain point
                if (enemy.GetY() + enemy.HalfWidth <= _gameEnvironment.Height / 4)
                {
                    enemy.MoveY();
                }
            }
            else
            {
                // move enemy down
                enemy.MoveY();
            }

            if (enemy.IsPlayerColliding)
            {
                var enemyX = enemy.GetX();

                var enemyWidthHalf = enemy.HalfWidth;

                // move right
                if (pointerX - enemyWidthHalf > enemyX + enemy.Speed)
                {
                    if (enemyX + enemyWidthHalf < _gameEnvironment.Width)
                    {
                        SetEnemyX(enemy: enemy, left: enemyX + enemy.Speed);
                    }
                }

                // move left
                if (pointerX - enemyWidthHalf < enemyX - enemy.Speed)
                {
                    SetEnemyX(enemy: enemy, left: enemyX - enemy.Speed);
                }
            }
            else
            {
                enemy.MoveX();

                if (enemy.IsHovering)
                {
                    // change direction of x axis movement
                    if (enemy.GetX() + enemy.HalfWidth >= _gameEnvironment.Width - 50 || enemy.GetX() + enemy.HalfWidth <= 50)
                        enemy.XDirection = enemy.XDirection == XDirection.LEFT ? XDirection.RIGHT : XDirection.LEFT;
                }
            }

            // if the object is marked for lazy destruction then do not destroy immidiately
            if (enemy.IsMarkedForFadedDestruction)
                return;

            // if object has gone beyond game view
            destroyed = _gameEnvironment.CheckAndAddDestroyableGameObject(enemy);
        }

        /// <summary>
        /// Sets X axis position of an enemy.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="left"></param>
        private void SetEnemyX(Enemy enemy, double left)
        {
            enemy.SetX(left);
        }

        /// <summary>
        /// Levels up enemies.
        /// </summary>
        public void LevelUp()
        {
            var delayScale = 3 + _gameEnvironment.GetGameObjectScale();
            _enemySpawnDelay -= delayScale;
            _enemySpeed += 1;
        }

        #endregion
    }
}
