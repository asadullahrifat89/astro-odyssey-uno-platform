namespace AstroOdyssey
{
    public interface ICollectibleFactory 
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnCollectible(GameLevel gameLevel);

        void GenerateCollectible();

        void UpdateCollectible(Collectible collectible, out bool destroyed);

        void LevelUp();

        void Reset();
    }
}
