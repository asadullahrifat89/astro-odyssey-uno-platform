namespace SpaceShooterGame
{
    public interface IMeteorFactory
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnMeteor(GameLevel gameLevel);

        void GenerateMeteor(GameLevel gameLevel);

        void DestroyMeteor(Meteor meteor);

        bool UpdateMeteor(Meteor meteor);

        void LevelUp();

        void Reset();
    }
}
