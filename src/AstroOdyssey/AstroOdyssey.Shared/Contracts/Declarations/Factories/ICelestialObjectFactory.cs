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

        void UpdateCelestialObject(CelestialObject celestialObject, out bool destroyed);

        void LevelUp();
    }
}
