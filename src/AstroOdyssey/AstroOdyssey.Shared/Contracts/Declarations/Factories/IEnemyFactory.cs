namespace AstroOdyssey
{
    public interface IEnemyFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        Enemy EngageBossEnemy(GameLevel gameLevel);

        void DisengageBossEnemy();

        void SpawnEnemy(GameLevel gameLevel);

        void GenerateEnemy(GameLevel gameLevel);

        void DestroyEnemy(Enemy enemy);

        void UpdateEnemy(Enemy enemy, double pointerX, out bool destroyed);

        void LevelUp();
    }
}
