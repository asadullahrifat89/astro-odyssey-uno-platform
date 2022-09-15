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

        void UpdateEnemy(Enemy enemy, double pointerX, out bool destroyed);

        void LevelUp();
    }
}
