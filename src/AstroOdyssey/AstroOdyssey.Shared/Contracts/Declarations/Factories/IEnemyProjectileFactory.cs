namespace AstroOdyssey
{
    public interface IEnemyProjectileFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnProjectile(Enemy enemy, GameLevel gameLevel);

        void UpdateProjectile(EnemyProjectile projectile, out bool destroyed);
    }
}
