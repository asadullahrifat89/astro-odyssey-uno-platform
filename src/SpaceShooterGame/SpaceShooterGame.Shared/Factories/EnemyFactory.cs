using System;
using System.Linq;

namespace SpaceShooterGame
{
    public class EnemyFactory
    {
        #region Fields

        private static GameEnvironment _gameView;
        private static readonly Random _random = new();

        private static int _xFlyingEnemySpawnCounter = 10;
        private static int _xFlyingEnemySpawnAfter = 10;

        private static int _overPoweredEnemySpawnCounter = 15;
        private static int _overPoweredEnemySpawnAfter = 15;

        private static int _evadingEnemySpawnCounter = 3;
        private static int _evadingEnemySpawnAfter = 3;

        private static int _firingEnemySpawnCounter = 7;
        private static int _firingEnemySpawnAfter = 7;

        private static int _targetingEnemySpawnAfter = 10;
        private static int _targetingEnemySpawnCounter = 10;

        private static int _hoveringEnemySpawnAfter = 12;
        private static int _hoveringEnemySpawnCounter = 12;

        private static double _enemySpeed = 2;
        private static double _maxEnemyCount = 7;
        private static int _enemySpawnCounter = 0;

        #endregion

        #region Methods

        #region Public

        public static void Reset()
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

            _enemySpeed = 2;

            _maxEnemyCount = 7;
            _enemySpawnCounter = 0;
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
        /// Spawns an enemy.
        /// </summary>
        public static void SpawnEnemy(GameLevel gameLevel)
        {
            if (_gameView.GetGameObjects<Enemy>().Count() < (_gameView.IsBossEngaged ? _maxEnemyCount - 2 : _maxEnemyCount))
            {
                _enemySpawnCounter--;

                if (_enemySpawnCounter < 0)
                {
                    GenerateEnemy(gameLevel);
                    _enemySpawnCounter = 10;
                }
            }
        }

        /// <summary>
        /// Generates a random Enemy.
        /// </summary>
        public static void GenerateEnemy(GameLevel gameLevel)
        {
            _xFlyingEnemySpawnCounter--;
            _overPoweredEnemySpawnCounter--;
            _evadingEnemySpawnCounter--;
            _firingEnemySpawnCounter--;
            _targetingEnemySpawnCounter--;
            _hoveringEnemySpawnCounter--;

            var enemy = new Enemy();

            enemy.SetAttributes(
                speed: _enemySpeed + _random.Next(0, 3),
                gameLevel: gameLevel,
                scale: _gameView.GameObjectScale);

            double left = 0;
            double top;

            // spawn evading enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _evadingEnemySpawnCounter <= 0)
                SetEvadingEnemy(enemy);

            // spawn following enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _targetingEnemySpawnCounter <= 0)
                SetPlayerTargetingEnemy(enemy);

            // spawn following enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _hoveringEnemySpawnCounter <= 0)
                SetHoveringEnemy(enemy);

            // generate large but slower and stronger enemies after level 3
            if (gameLevel > GameLevel.Level_3 && _overPoweredEnemySpawnCounter <= 0)
                SetOverPoweredEnemy(enemy);

            // spawn blaster shooting enemies after level 2
            if (gameLevel > GameLevel.Level_2 && _firingEnemySpawnCounter <= 0)
                SetProjectileFiringEnemy(enemy);

            // generate side ways flying enemies after level 2
            if (gameLevel > GameLevel.Level_2 && _xFlyingEnemySpawnCounter <= 0)
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
                        left = _gameView.Width;
                        break;
                    case XDirection.RIGHT:
                        enemy.Rotation = enemy.YDirection == YDirection.NONE ? 0 : -50;
                        left = 0 - enemy.Width + 10;
                        break;
                    default:
                        break;
                }

                top = _random.Next(0, (int)_gameView.Height / 3);
                enemy.Rotate();

                // sideways flying enemies do not follow player or evade on hit
                enemy.IsPlayerColliding = false;
                enemy.IsEvading = false;
                enemy.IsHovering = false;

                // randomize next x flying enemy pop up
                _xFlyingEnemySpawnAfter = _random.Next(5, 15);
            }
            else
            {
                if (enemy.IsHovering)
                {
                    // appear in the middle of the screen
                    left = _gameView.HalfWidth + enemy.HalfWidth;
                    enemy.XDirection = XDirection.LEFT;
                }
                else
                {
                    left = _random.Next(10, (int)_gameView.Width - (int)enemy.Width);
                }

                top = _random.Next(100 * (int)_gameView.GameObjectScale, (int)_gameView.Height) * -1;
            }

            enemy.AddToGameEnvironment(
                top: top,
                left: left,
                gameEnvironment: _gameView);
        }

        /// <summary>
        /// Destroys a Enemy. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="enemy"></param>
        public static void DestroyByPlayerProjectle(Enemy enemy)
        {
            enemy.IsDestroyedByCollision = true;
            AudioHelper.PlaySound(SoundType.ENEMY_DESTRUCTION);
        }

        /// <summary>
        /// Updates an enemy. Moves the enemy inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="destroyed"></param>
        public static bool UpdateEnemy(
            Enemy enemy,
            GameLevel gameLevel,
            double pointerX)
        {
            bool destroyed = false;

            if (enemy.IsDestroyedByCollision)
            {
                enemy.Explode();

                if (enemy.HasExploded)
                {
                    _gameView.AddDestroyableGameObject(enemy);
                    destroyed = true;
                }
            }
            else
            {
                enemy.CoolDownProjectileImpactEffect();

                if (enemy.IsBoss)
                {
                    // move boss down upto a certain point
                    if (enemy.GetY() + enemy.HalfWidth <= _gameView.Height / 4)
                        enemy.MoveY();
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
                        if (enemyX + enemyWidthHalf < _gameView.Width)
                            SetEnemyX(enemy: enemy, left: enemyX + enemy.Speed);

                    }

                    // move left
                    if (pointerX - enemyWidthHalf < enemyX - enemy.Speed)
                        SetEnemyX(enemy: enemy, left: enemyX - enemy.Speed);
                }
                else
                {
                    enemy.MoveX();

                    // hovering enemies x direction change
                    if (enemy.IsHovering)
                    {
                        // change direction of x axis movement
                        if (enemy.GetX() + enemy.Width >= _gameView.Width - 25 || enemy.GetX() <= 25)
                            enemy.XDirection = enemy.XDirection == XDirection.LEFT ? XDirection.RIGHT : XDirection.LEFT;
                    }
                }

                // if enemey has gone beyond game view naturally, recycle it
                if (_gameView.IsRecyclable(enemy))
                {
                    RecycleEnemy(enemy, gameLevel);
                    destroyed = true;
                }
            }

            return destroyed;
        }

        /// <summary>
        /// Engages a boss.
        /// </summary>
        /// <param name="gameLevel"></param>
        public static Enemy EngageBoss(GameLevel gameLevel)
        {
            var boss = new Enemy();

            boss.SetAttributes(
                speed: _enemySpeed + 1,
                gameLevel: gameLevel,
                scale: _gameView.GameObjectScale,
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
            double left = _gameView.HalfWidth - boss.HalfWidth;
            double top = 0 - boss.Height;

            if (boss.IsHovering)
                boss.XDirection = (XDirection)_random.Next(1, Enum.GetNames<XDirection>().Length);

            boss.AddToGameEnvironment(
                top: top,
                left: left,
                gameEnvironment: _gameView);

            _gameView.IsBossEngaged = true;

            AudioHelper.StopSound();
            AudioHelper.PlaySound(SoundType.BOSS_APPEARANCE);

            return boss;
        }

        /// <summary>
        /// Disengages a boss.
        /// </summary>
        public static void DisengageBoss()
        {
            _gameView.IsBossEngaged = false;

            AudioHelper.StopSound();
            AudioHelper.PlaySound(SoundType.BOSS_DESTRUCTION);
            AudioHelper.PlaySound(SoundType.BACKGROUND);
        }

        /// <summary>
        /// Levels up enemies.
        /// </summary>
        public static void LevelUp()
        {
            var scale = _gameView.GameObjectScale;
            _enemySpeed += (1.3 * scale);
        }

        #endregion

        #region Private

        /// <summary>
        /// Generates an enemy that hovers across the screen.
        /// </summary>
        /// <param name="enemy"></param>
        private static void SetHoveringEnemy(Enemy enemy)
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
        private static void SetEvadingEnemy(Enemy enemy)
        {
            _evadingEnemySpawnCounter = _evadingEnemySpawnAfter;
            enemy.IsEvading = true;
            _evadingEnemySpawnAfter = _random.Next(1, 4);
        }

        /// <summary>
        /// Generates an enemy that collides with the player.
        /// </summary>
        /// <param name="enemy"></param>
        private static void SetPlayerTargetingEnemy(Enemy enemy)
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
        private static void SetProjectileFiringEnemy(Enemy enemy)
        {
            _firingEnemySpawnCounter = _firingEnemySpawnAfter;
            enemy.IsProjectileFiring = true;
            _firingEnemySpawnAfter = _random.Next(1, 8);
        }

        /// <summary>
        /// Generates an enemy that is overpowered. Bigger and stronger but slower enemies.
        /// </summary>
        /// <param name="enemy"></param>
        private static void SetOverPoweredEnemy(Enemy enemy)
        {
            _overPoweredEnemySpawnCounter = _overPoweredEnemySpawnAfter;
            enemy.OverPower();
            _overPoweredEnemySpawnAfter = _random.Next(10, 15);
        }

        /// <summary>
        /// Sets X axis position of an enemy.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="left"></param>
        private static void SetEnemyX(
            Enemy enemy,
            double left)
        {
            enemy.SetX(left);
        }

        private static void RecycleEnemy(
            Enemy enemy,
            GameLevel gameLevel)
        {
            enemy.SetAttributes(
                 speed: _enemySpeed + _random.Next(0, 3),
                 gameLevel: gameLevel,
                 scale: _gameView.GameObjectScale,
                 recycle: true);

            enemy.SetPosition(
                left: _random.Next(0, (int)_gameView.Width) - (100 * _gameView.GameObjectScale),
                top: _random.Next(100 * (int)_gameView.GameObjectScale, (int)_gameView.Height) * -1);
        }

        #endregion

        #endregion
    }
}
