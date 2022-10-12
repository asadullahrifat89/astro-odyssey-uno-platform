namespace SpaceShooterGame
{
    public interface IPlayerProjectileFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void IncreaseProjectilePower(Player player);

        void DecreaseProjectilePower(Player player);

        void SpawnProjectile(
            bool isPoweredUp,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType);

        void GenerateProjectile(
            bool isPoweredUp,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType);

        bool UpdateProjectile(PlayerProjectile projectile);

        (double Score, GameObject DestroyedObject) CheckCollision(PlayerProjectile projectile);

        void PowerUp(PowerUpType powerUpType, Player player);

        void PowerDown(PowerUpType powerUpType, Player player);

        void LevelUp(Player player);

        void RageUp(Player player);

        void RageDown(Player player);
    }
}
