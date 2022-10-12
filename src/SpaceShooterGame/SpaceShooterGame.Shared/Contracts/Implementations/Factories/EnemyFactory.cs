using System;

namespace SpaceShooterGame
{
    public class EnemyFactory : IEnemyFactory
    {
        #region Fields

        private GameEnvironment _gameEnvironment;
        private readonly Random _random = new Random();

        private int _xFlyingEnemySpawnCounter = 10;
        private int _xFlyingEnemySpawnAfter = 10;

        private int _overPoweredEnemySpawnCounter = 15;
        private int _overPoweredEnemySpawnAfter = 15;

        private int _evadingEnemySpawnCounter = 3;
        private int _evadingEnemySpawnAfter = 3;

        private int _firingEnemySpawnCounter = 7;
        private int _firingEnemySpawnAfter = 7;

        private int _targetingEnemySpawnAfter = 10;
        private int _targetingEnemySpawnCounter = 10;

        private int _hoveringEnemySpawnAfter = 12;
        private int _hoveringEnemySpawnCounter = 12;

        private double _enemySpawnCounter;
        private double _enemySpawnAfter = 48;
        private double _enemySpeed = 2;

        private readonly IAudioHelper _audioHelper;
        #endregion

        #region Ctor

        public EnemyFactory(IAudioHelper audioHelper)
        {
            _audioHelper = audioHelper;
        }

        #endregion

        #region Methods

        #region Public

        public void Reset()
        {
            _xFlyingEnemySpawnCounter = 10;
            _xFlyingEnemySpawnAfter = 10;

            _overPoweredEnemySpawnCounter = 15;
            _overPoweredEnemySpawnAfter = 15;

            _evadingEnemySpawnCounter = 3;
            _evadingEnemySpawnAfter = 3;

            _firingEnemySpawnCounter = 7;
            _firingEnemySpawnAfter = 7;

            _targetingEnemySpawnAfter = 10;
            _targetingEnemySpawnCounter = 10;

            _hoveringEnemySpawnAfter = 12;
            _hoveringEnemySpawnCounter = 12;

            _enemySpawnAfter = 48;
            _enemySpeed = 2;
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
        /// Engages a boss.
        /// </summary>
        /// <param name="gameLevel"></param>
        public Enemy EngageBoss(GameLevel gameLevel)
        {
            var boss = new Enemy();

            boss.SetAttributes(
                speed: _enemySpeed + 1,
                gameLevel: gameLevel,
                scale: _gameEnvironment.GetGameObjectScale(),
                isBoss: true);

            SetProjectileFiringEnemy(boss);
            SetHoveringEnemy(boss);

            // draw between player targeting and hovering enemy
            //var firingType = _random.Next(0, 2);

            //switch (firingType)
            //{
            //    case 0: { SetHoveringEnemy(enemy); } break;
            //    case 1: { SetPlayerTargetingEnemy(enemy); enemy.Speed--; } break;
            //    default:
            //        break;
            //}

            // appear in the middle of the screen
            double left = _gameEnvironment.HalfWidth - boss.HalfWidth;
            double top = 0 - boss.Height;

            if (boss.IsHovering)
                boss.XDirection = (XDirection)_random.Next(1, Enum.GetNames<XDirection>().Length);

            boss.AddToGameEnvironment(
                top: top,
                left: left,
                gameEnvironment: _gameEnvironment);

            _gameEnvironment.IsBossEngaged = true;

            _audioHelper.StopSound();
            _audioHelper.PlaySound(SoundType.BOSS_APPEARANCE);

            return boss;
        }

        /// <summary>
        /// Disengages a boss.
        /// </summary>
        public void DisengageBoss()
        {
            _gameEnvironment.IsBossEngaged = false;
            _audioHelper.StopSound();
            _audioHelper.PlaySound(SoundType.BOSS_DESTRUCTION);
            _audioHelper.PlaySound(SoundType.BACKGROUND_MUSIC);
        }

        /// <summary>
        /// Spawns an enemy.
        /// </summary>
        public void SpawnEnemy(GameLevel gameLevel)
        {
            // each frame progress decreases this counter
            _enemySpawnCounter -= 1;

            // when counter reaches zero, create an enemy
            if (_enemySpawnCounter < 0)
            {
                GenerateEnemy(gameLevel);
                _enemySpawnCounter = _enemySpawnAfter;
            }
        }

        /// <summary>
        /// Generates a random Enemy.
        /// </summary>
        public void GenerateEnemy(GameLevel gameLevel)
        {
            var enemy = new Enemy();

            enemy.SetAttributes(
                speed: _enemySpeed + _random.Next(0, 3),
                gameLevel: gameLevel,
                scale: _gameEnvironment.GetGameObjectScale());

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
        /// Destroys a Enemy. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="enemy"></param>
        public void DestroyEnemy(Enemy enemy)
        {
            enemy.IsMarkedForFadedDestruction = true;
            _audioHelper.PlaySound(SoundType.ENEMY_DESTRUCTION);
        }

        /// <summary>
        /// Updates an enemy. Moves the enemy inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="destroyed"></param>
        public bool UpdateEnemy(Enemy enemy, double pointerX)
        {
            bool destroyed = false;

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

            // player colliding enemies
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

                // hovering enemies x direction change
                if (enemy.IsHovering)
                {
                    // change direction of x axis movement
                    if (enemy.GetX() + enemy.Width >= _gameEnvironment.Width - 25 || enemy.GetX() <= 25)
                        enemy.XDirection = enemy.XDirection == XDirection.LEFT ? XDirection.RIGHT : XDirection.LEFT;
                }
            }

            // if the object is marked for lazy destruction then do not destroy immidiately
            if (enemy.IsMarkedForFadedDestruction)
                return false;

            // if object has gone beyond game view
            destroyed = _gameEnvironment.CheckAndAddDestroyableGameObject(enemy);

            return destroyed;
        }

        /// <summary>
        /// Levels up enemies.
        /// </summary>
        public void LevelUp()
        {
            var scale = _gameEnvironment.GetGameObjectScale();

            // increase enemy spwan rate
            if (_enemySpawnAfter > 2)
                _enemySpawnAfter -= 6 * scale;

            _enemySpeed += (1 * scale);
        }

        #endregion

        #region Private

        /// <summary>
        /// Generates an enemy that hovers across the screen.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetHoveringEnemy(Enemy enemy)
        {
            _hoveringEnemySpawnCounter = _hoveringEnemySpawnAfter;
            enemy.IsHovering = true;
            _hoveringEnemySpawnAfter = _random.Next(9, 13);

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
            _evadingEnemySpawnCounter = _evadingEnemySpawnAfter;
            enemy.IsEvading = true;
            _evadingEnemySpawnAfter = _random.Next(1, 4);
        }

        /// <summary>
        /// Generates an enemy that collides with the player.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetPlayerTargetingEnemy(Enemy enemy)
        {
            _targetingEnemySpawnCounter = _targetingEnemySpawnAfter;
            enemy.IsPlayerColliding = true;
            _targetingEnemySpawnAfter = _random.Next(7, 12);

            // player targeting enemies do not evade on hit
            enemy.IsEvading = false;
        }

        /// <summary>
        /// Generates an enemy that fires projectiles.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetProjectileFiringEnemy(Enemy enemy)
        {
            _firingEnemySpawnCounter = _firingEnemySpawnAfter;
            enemy.IsProjectileFiring = true;
            _firingEnemySpawnAfter = _random.Next(1, 8);
        }

        /// <summary>
        /// Generates an enemy that is overpowered. Bigger and stronger but slower enemies.
        /// </summary>
        /// <param name="enemy"></param>
        private void SetOverPoweredEnemy(Enemy enemy)
        {
            _overPoweredEnemySpawnCounter = _overPoweredEnemySpawnAfter;
            enemy.OverPower();
            _overPoweredEnemySpawnAfter = _random.Next(10, 15);
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
            _xFlyingEnemySpawnCounter = _xFlyingEnemySpawnAfter;

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
            _xFlyingEnemySpawnAfter = _random.Next(5, 15);
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

        #endregion

        #endregion
    }
}
