namespace AstroOdyssey
{
    public interface IPlayerProjectileFactory 
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void IncreaseProjectilePower();

        void DecreaseProjectilePower();

        void SpawnProjectile(
            bool isPoweredUp,
            bool firingProjectiles,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType);

        void GenerateProjectile(
            bool isPoweredUp,
            Player player,
            GameLevel gameLevel,
            PowerUpType powerUpType);

        void UpdateProjectile(PlayerProjectile projectile, out bool destroyed);

        void CollidePlayerProjectile(
            PlayerProjectile projectile,
            out double score,
            out GameObject destroyedObject);

        void PowerUp(PowerUpType powerUpType);

        void PowerDown(PowerUpType powerUpType);

        void LevelUp();

        void RageUp(Player player);

        void RageDown(Player player);
    }
}
