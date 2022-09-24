namespace AstroOdyssey
{
    public interface IEnemyFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        Enemy EngageBoss(GameLevel gameLevel);

        void DisengageBoss();

        void SpawnEnemy(GameLevel gameLevel);

        void GenerateEnemy(GameLevel gameLevel);

        void DestroyEnemy(Enemy enemy);

        bool UpdateEnemy(Enemy enemy, double pointerX);

        void LevelUp();

        void Reset();
    }
}
