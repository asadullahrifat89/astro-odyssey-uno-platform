namespace AstroOdyssey
{
    public interface IPlayerFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        Player SpawnPlayer(double pointerX, PlayerShip ship);

        double GetOptimalPlayerY(Player player);

        double UpdatePlayer(Player player, double pointerX, bool moveLeft, bool moveRight);

        double UpdateAcceleration(Player player, double pointerX);

        (bool PowerDown, double PowerRemaining) PowerUpCoolDown(Player player);

        void RageUp(Player player);

        (bool RageDown, double RageRemaining) RageUpCoolDown(Player player);

        bool CheckCollision(Player player, GameObject gameObject);

        void DamageRecoveryCoolDown(Player player);
    }
}
