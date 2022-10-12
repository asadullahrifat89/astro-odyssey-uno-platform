namespace SpaceShooterGame
{
    public interface IEnemyProjectileFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnProjectile(Enemy enemy, GameLevel gameLevel);

        bool UpdateProjectile(EnemyProjectile projectile);
    }
}
