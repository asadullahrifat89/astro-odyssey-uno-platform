namespace AstroOdyssey
{
    public interface IPowerUpFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnPowerUp();

        void GeneratePowerUp();

        void UpdatePowerUp(PowerUp powerUp, out bool destroyed);

        void LevelUp();

        void Reset();

    }
}
