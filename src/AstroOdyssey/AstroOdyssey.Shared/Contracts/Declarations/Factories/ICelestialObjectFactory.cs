namespace AstroOdyssey
{
    public interface ICelestialObjectFactory
    {
        void SetGameEnvironments(GameEnvironment starView, GameEnvironment planetView);

        void StartSpaceWarp();

        void StopSpaceWarp();

        void SpawnCelestialObject();

        void GenerateStar();

        void GeneratePlanet();

        bool UpdateCelestialObject(CelestialObject celestialObject);

        void LevelUp();

        void Reset();
    }
}
