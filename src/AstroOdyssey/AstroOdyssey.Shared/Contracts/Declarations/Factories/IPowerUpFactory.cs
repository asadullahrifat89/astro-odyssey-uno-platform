namespace AstroOdyssey
{
    public interface IPowerUpFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnPowerUp();

        void GeneratePowerUp();

        bool UpdatePowerUp(PowerUp powerUp);

        void LevelUp();

        void Reset();

    }
}
